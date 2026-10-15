using HMI_Master_KX7500.Model.ShellRecipe;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace HMI_Master_KX7500.Service.ShellRecipe
{
    /// <summary>
    /// Robot points per recipe (MT_001), requested by robot team (CK). Create Points writes 2 files for the robot:
    /// 1. installation file (PolyScope *.installation = gzip XML): copy of the 33 Black points under the recipe prefix
    ///    (Point_1b..Point_21b -> Blue_P01..Blue_P21, Point_1bEntry..Point_21bEntry -> Blue_P22..Blue_P33), nothing else changed.
    /// 2. recipe_points.script: rcp_height(n) / rcp_entry(n) return the point set of recipe ID n (= shell_int on the robot).
    /// </summary>
    public class RobotPointService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const int MaxPrefixLength = 5;
        public const string ScriptFileName = "recipe_points.script";

        // screws that have an entry point above the screw, P22..P33 in this order; the other screws enter at the screw point
        private static readonly int[] entryScrews = { 1, 2, 3, 7, 8, 10, 16, 17, 18, 19, 20, 21 };
        private const int screwCount = 21;

        // reference = Black set, P01..P33
        private static readonly string[] referencePoints = Enumerable.Range(1, screwCount).Select(x => $"Point_{x}b")
            .Concat(entryScrews.Select(x => $"Point_{x}bEntry")).ToArray();

        // point sets that existed before the prefix points (robot program 05092022), index 0 = screw 1
        private const int builtInBlackID = 0;
        private const int builtInTransparentID = 1;
        private static readonly string[] blackHeight = Enumerable.Range(1, screwCount).Select(x => $"Point_{x}b").ToArray();
        private static readonly string[] blackEntry =
        {
            "Point_1bEntry", "Point_2bEntry", "Point_3bEntry", "Point_4b", "Point_5b", "Point_6b", "Point_7bEntry", "Point_8bEntry", "Point_9b", "Point_10bEntry",
            "Point_11b", "Point_12b", "Point_13b", "Point_14b", "Point_15b", "Point_16bEntry", "Point_17bEntry", "Point_18bEntry", "Point_19bEntry", "Point_20bEntry", "Point_21bEntry"
        };
        private static readonly string[] transparentHeight = Enumerable.Range(1, screwCount).Select(x => $"Point_t{x}").ToArray();
        // screw 10 uses the Black entry point, as in the robot program
        private static readonly string[] transparentEntry =
        {
            "Point_t1e", "Point_t2e", "Point_t3e", "Point_t4", "Point_t5", "Point_t6", "Point_t7Entry", "Point_t8Entry", "Point_t9", "Point_10bEntry",
            "Point_t11", "Point_t12", "Point_t13", "Point_t14", "Point_t15", "Point_t16Entry", "Point_t17Entry", "Point_t18Entry", "Point_t19Entry", "Point_t20Entry", "Point_t21Entry"
        };

        // UR point names are URScript variables: letter first, then letters or digits (HMI adds the _)
        private static readonly Regex validPrefix = new Regex(@"^[A-Za-z][A-Za-z0-9]{0," + (MaxPrefixLength - 1) + "}$");

        #region PublicMethods
        // "Blue_" and "Blue" are the same prefix
        public static string NormalizePrefix(string prefix)
        {
            return (prefix ?? string.Empty).Trim().TrimEnd('_');
        }

        public static bool IsValidPrefix(string prefix)
        {
            return validPrefix.IsMatch(NormalizePrefix(prefix));
        }

        // P01..P33
        public static string GetPointName(string prefix, int number)
        {
            return $"{NormalizePrefix(prefix)}_P{number:00}";
        }

        public static bool IsBuiltInSet(ShellRecipeModel recipe)
        {
            return string.IsNullOrEmpty(NormalizePrefix(recipe.RobotPointPrefix))
                && (recipe.RecipeID == builtInBlackID || recipe.RecipeID == builtInTransparentID);
        }

        // text for the Recipe page: which robot points the recipe uses
        public static string GetPointInfo(ShellRecipeModel recipe)
        {
            if (recipe == null) { return string.Empty; }

            string prefix = NormalizePrefix(recipe.RobotPointPrefix);
            if (!string.IsNullOrEmpty(prefix))
            {
                return $"Robot set {recipe.RecipeID}: {GetPointName(prefix, 1)} ... {GetPointName(prefix, referencePoints.Length)}";
            }
            if (recipe.RecipeID == builtInBlackID)
            {
                return $"Robot set {recipe.RecipeID}: built-in Black points (Point_1b ...)";
            }
            if (recipe.RecipeID == builtInTransparentID)
            {
                return $"Robot set {recipe.RecipeID}: built-in Transparent points (Point_t1 ...)";
            }
            return "No prefix: the robot has no points for this recipe.";
        }

        /// <summary>
        /// Reads sourceFile (installation copied from the robot), adds the points of prefix when they are not there yet
        /// (existing points are kept as they are, they may be adjusted already), writes installationFile and scriptFile.
        /// sourceFile and installationFile may be the same file. Returns false with the reason in message.
        /// </summary>
        public static bool CreatePoints(string sourceFile, string installationFile, string scriptFile, string prefix,
            List<ShellRecipeModel> recipes, out string message)
        {
            message = string.Empty;

            try
            {
                if (!IsValidPrefix(prefix))
                {
                    message = $"Invalid prefix \"{prefix}\".";
                    return false;
                }
                prefix = NormalizePrefix(prefix);

                XDocument installation;
                using (FileStream file = File.OpenRead(sourceFile))
                using (GZipStream gzip = new GZipStream(file, CompressionMode.Decompress))
                {
                    installation = XDocument.Load(gzip);
                }

                XElement features = installation.Root?.Name == "Installation" ? installation.Root.Element("Features") : null;
                if (features == null)
                {
                    message = "File is not a robot installation file.";
                    return false;
                }

                // each point is <GeomPoseNode><GeomPoseNode id=".." name="..">..</GeomPoseNode></GeomPoseNode>
                List<XElement> pointWrappers = features.Elements("GeomPoseNode").ToList();
                Dictionary<string, XElement> points = pointWrappers
                    .Where(x => x.Element("GeomPoseNode")?.Attribute("name") != null)
                    .GroupBy(x => (string)x.Element("GeomPoseNode").Attribute("name"))
                    .ToDictionary(x => x.Key, x => x.First());

                List<string> missingReferences = referencePoints.Where(x => !points.ContainsKey(x)).ToList();
                if (missingReferences.Count > 0)
                {
                    message = $"Black reference points not found in the installation file: {string.Join(", ", missingReferences)}.";
                    return false;
                }

                HashSet<string> existingNames = new HashSet<string>(
                    features.Descendants().Select(x => (string)x.Attribute("name")).Where(x => x != null),
                    StringComparer.OrdinalIgnoreCase);
                List<string> newNames = Enumerable.Range(1, referencePoints.Length).Select(x => GetPointName(prefix, x)).ToList();
                int existingCount = newNames.Count(x => existingNames.Contains(x));

                if (existingCount > 0 && existingCount < newNames.Count)
                {
                    message = $"Only {existingCount} of {newNames.Count} points with prefix \"{prefix}\" exist in the installation file. Please check the robot points.";
                    return false;
                }

                if (existingCount == 0)
                {
                    // new points go after the last existing point
                    XElement insertAfter = pointWrappers.Last();
                    for (int i = 0; i < referencePoints.Length; i++)
                    {
                        XElement copy = new XElement(points[referencePoints[i]]);
                        XElement point = copy.Element("GeomPoseNode");
                        point.SetAttributeValue("name", newNames[i]);
                        point.SetAttributeValue("id", Guid.NewGuid().ToString());

                        insertAfter.AddAfterSelf(copy);
                        insertAfter = copy;
                        existingNames.Add(newNames[i]);
                    }
                }

                writeInstallation(installation, installationFile);

                List<string> skipped;
                File.WriteAllText(scriptFile, buildScript(recipes, existingNames, out skipped), new UTF8Encoding(false));

                message = existingCount == 0
                    ? $"{newNames.Count} points created ({newNames.First()} ... {newNames.Last()})."
                    : $"Points {newNames.First()} ... {newNames.Last()} already exist, kept as they are.";
                if (skipped.Count > 0)
                {
                    message += "\n\nNot in the script (no robot points):\n" + string.Join("\n", skipped);
                }

                logger.Info($"Robot points: prefix {prefix}, {(existingCount == 0 ? "created" : "already exist")}, {sourceFile} -> {installationFile}, {scriptFile}");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                message = ex.Message;
            }

            return false;
        }
        #endregion

        #region PrivateMethods
        // same layout as PolyScope: no declaration, 2 space indent, LF, UTF-8 without BOM
        private static void writeInstallation(XDocument installation, string file)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                Encoding = new UTF8Encoding(false)
            };

            // build in memory first, the source file can be the target file
            using (MemoryStream buffer = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(buffer, CompressionMode.Compress, true))
                using (XmlWriter writer = XmlWriter.Create(gzip, settings))
                {
                    installation.Save(writer);
                }
                File.WriteAllBytes(file, buffer.ToArray());
            }
        }

        // recipe_points.script: one branch per recipe that has all its points in the installation
        private static string buildScript(List<ShellRecipeModel> recipes, HashSet<string> pointNames, out List<string> skipped)
        {
            skipped = new List<string>();
            List<Tuple<ShellRecipeModel, string, string[], string[]>> sets = new List<Tuple<ShellRecipeModel, string, string[], string[]>>();

            foreach (ShellRecipeModel recipe in recipes.OrderBy(x => x.RecipeID))
            {
                string prefix = NormalizePrefix(recipe.RobotPointPrefix);
                string comment = (recipe.RecipeName ?? string.Empty).Replace("\r", " ").Replace("\n", " ");
                string[] height;
                string[] entry;

                if (!string.IsNullOrEmpty(prefix))
                {
                    height = Enumerable.Range(1, screwCount).Select(x => GetPointName(prefix, x)).ToArray();
                    entry = Enumerable.Range(1, screwCount).Select(x => Array.IndexOf(entryScrews, x) >= 0
                        ? GetPointName(prefix, screwCount + 1 + Array.IndexOf(entryScrews, x))
                        : GetPointName(prefix, x)).ToArray();
                    comment = $"{prefix} ({comment})";
                }
                else if (recipe.RecipeID == builtInBlackID)
                {
                    height = blackHeight;
                    entry = blackEntry;
                }
                else if (recipe.RecipeID == builtInTransparentID)
                {
                    height = transparentHeight;
                    entry = transparentEntry;
                }
                else
                {
                    skipped.Add($"{recipe.DisplayName} (no prefix)");
                    continue;
                }

                if (height.Concat(entry).Any(x => !pointNames.Contains(x)))
                {
                    skipped.Add($"{recipe.DisplayName} (points not in the installation file)");
                    continue;
                }

                sets.Add(Tuple.Create(recipe, comment, height, entry));
            }

            StringBuilder script = new StringBuilder();
            script.Append("# Robot point sets for the A300 recipes. Generated by HMI ").Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Append(": do not edit by hand.\n");
            script.Append("# shell_int (W0102, from HMI Shell_Colour_ID EM4250 = Recipe ID) selects the set.\n");
            script.Append("# P1-P21 = screw 1-21, P22-P33 = entry above screw 1,2,3,7,8,10,16-21 (other screws enter at the screw point).\n");
            appendFunction(script, "rcp_height", sets.Select(x => Tuple.Create(x.Item1.RecipeID, x.Item2, x.Item3)));
            appendFunction(script, "rcp_entry", sets.Select(x => Tuple.Create(x.Item1.RecipeID, x.Item2, x.Item4)));

            return script.ToString();
        }

        private static void appendFunction(StringBuilder script, string name, IEnumerable<Tuple<int, string, string[]>> sets)
        {
            script.Append("\n");
            script.Append($"def {name}(n):\n");

            bool first = true;
            foreach (Tuple<int, string, string[]> set in sets)
            {
                script.Append($"  {(first ? "if" : "elif")} n == {set.Item1}:   # {set.Item2}\n");
                script.Append($"    return [p[0,0,0,0,0,0], {string.Join(", ", set.Item3)}]\n");
                first = false;
            }
            if (!first)
            {
                script.Append("  end\n");
            }

            script.Append("  popup(str_cat(\"Unknown robot point set \", n), \"Recipe\", error=True, blocking=True)\n");
            script.Append("  halt\n");
            script.Append("end\n");
        }
        #endregion
    }
}
