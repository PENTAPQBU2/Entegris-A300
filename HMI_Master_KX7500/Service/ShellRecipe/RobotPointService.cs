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
    /// 1. installation file (PolyScope *.installation = gzip XML):
    ///    - the old Black / Transparent points are renamed (Point_1b -> Black_P01, Point_1bEntry -> Black_P01Entry,
    ///      Point_t1 -> Transparent_P01, Point_t1e -> Transparent_P01Entry, ...), Transparent_P10Entry = copy of Black_P10Entry
    ///    - copy of the 33 Black points under the recipe prefix (Black_P01..P21 -> Blue_P01..P21, Black_PxxEntry -> Blue_PxxEntry)
    /// 2. recipe_points.script: rcp_height(n) / rcp_entry(n) return the point set of recipe ID n (= shell_int on the robot).
    /// </summary>
    public class RobotPointService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const int MaxPrefixLength = 5;
        public const string ScriptFileName = "recipe_points.script";

        // screws that have an entry point above the screw, rcp_entry index 1..12 in this order
        private static readonly int[] entryScrews = { 1, 2, 3, 7, 8, 10, 16, 17, 18, 19, 20, 21 };
        private const int screwCount = 21;

        // point sets that existed before the prefix points (robot program 05092022), used by recipe 0 / 1 without prefix
        private const int builtInBlackID = 0;
        private const int builtInTransparentID = 1;
        private const string blackPrefix = "Black";
        private const string transparentPrefix = "Transparent";

        // reference = Black set, the prefix points are a copy of it
        private static readonly string[] referencePoints = GetPointNames(blackPrefix);

        // old robot point names -> new names, renamed in the installation file
        private static readonly Dictionary<string, string> builtInRenames = buildRenames();

        // screw 10 Transparent has no own entry point, the robot program uses the Black one
        private static readonly string transparentEntry10 = GetEntryPointName(transparentPrefix, 10);
        private static readonly string blackEntry10 = GetEntryPointName(blackPrefix, 10);

        // UR point names are URScript variables: letter first, then letters or digits (HMI adds the _)
        private static readonly Regex validPrefix = new Regex(@"^[A-Za-z][A-Za-z0-9]{0," + (MaxPrefixLength - 1) + "}$");

        // last entry point of a set made by the earlier version (<prefix>_P22..P33 = entry points)
        private static readonly Regex oldEntryName = new Regex(@"^([A-Za-z][A-Za-z0-9]*)_P33$");

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

        // screw point P01..P21
        public static string GetPointName(string prefix, int screw)
        {
            return $"{NormalizePrefix(prefix)}_P{screw:00}";
        }

        // entry point above the screw P01Entry..P21Entry (only screws 1,2,3,7,8,10,16-21 have one)
        public static string GetEntryPointName(string prefix, int screw)
        {
            return $"{GetPointName(prefix, screw)}Entry";
        }

        // the 33 points of a set: 21 screw points, then the 12 entry points
        public static string[] GetPointNames(string prefix)
        {
            return Enumerable.Range(1, screwCount).Select(x => GetPointName(prefix, x))
                .Concat(entryScrews.Select(x => GetEntryPointName(prefix, x))).ToArray();
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
                return $"Robot set {recipe.RecipeID}: {GetPointName(prefix, 1)} ... {GetPointName(prefix, screwCount)}, " +
                    $"{GetEntryPointName(prefix, entryScrews.First())} ... {GetEntryPointName(prefix, entryScrews.Last())}";
            }
            if (recipe.RecipeID == builtInBlackID)
            {
                return $"Robot set {recipe.RecipeID}: built-in Black points ({GetPointName(blackPrefix, 1)} ...)";
            }
            if (recipe.RecipeID == builtInTransparentID)
            {
                return $"Robot set {recipe.RecipeID}: built-in Transparent points ({GetPointName(transparentPrefix, 1)} ...)";
            }
            return "No prefix: the robot has no points for this recipe.";
        }

        /// <summary>
        /// Reads sourceFile (installation copied from the robot), renames the old Black / Transparent points,
        /// adds the points of prefix when they are not there yet (existing points are kept as they are, they may be adjusted already),
        /// writes installationFile and scriptFile. sourceFile and installationFile may be the same file. Returns false with the reason in message.
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
                List<XElement> pointWrappers = features.Elements("GeomPoseNode").Where(x => x.Element("GeomPoseNode")?.Attribute("name") != null).ToList();
                HashSet<string> existingNames = new HashSet<string>(
                    features.Descendants().Select(x => (string)x.Attribute("name")).Where(x => x != null),
                    StringComparer.OrdinalIgnoreCase);

                // old names -> Black_.. / Transparent_.. (skipped when the new name exists already)
                int renamedCount = 0;
                foreach (XElement wrapper in pointWrappers)
                {
                    XElement point = wrapper.Element("GeomPoseNode");
                    if (builtInRenames.TryGetValue((string)point.Attribute("name"), out string newName) && !existingNames.Contains(newName))
                    {
                        existingNames.Remove((string)point.Attribute("name"));
                        point.SetAttributeValue("name", newName);
                        existingNames.Add(newName);
                        renamedCount++;
                    }
                }

                Dictionary<string, XElement> points = pointWrappers
                    .GroupBy(x => (string)x.Element("GeomPoseNode").Attribute("name"))
                    .ToDictionary(x => x.Key, x => x.First());

                // sets made by the earlier version: <prefix>_P22..P33 = entry points -> <prefix>_PxxEntry (poses kept)
                List<string> migratedPrefixes = new List<string>();
                foreach (string oldPrefix in points.Keys.Select(x => oldEntryName.Match(x)).Where(x => x.Success).Select(x => x.Groups[1].Value).ToList())
                {
                    string[] oldNames = Enumerable.Range(screwCount + 1, entryScrews.Length).Select(x => GetPointName(oldPrefix, x)).ToArray();
                    string[] entryNames = entryScrews.Select(x => GetEntryPointName(oldPrefix, x)).ToArray();
                    if (!oldNames.All(x => points.ContainsKey(x)) || entryNames.Any(x => existingNames.Contains(x)))
                    {
                        continue;
                    }

                    for (int i = 0; i < oldNames.Length; i++)
                    {
                        XElement wrapper = points[oldNames[i]];
                        wrapper.Element("GeomPoseNode").SetAttributeValue("name", entryNames[i]);
                        points.Remove(oldNames[i]);
                        points[entryNames[i]] = wrapper;
                        existingNames.Remove(oldNames[i]);
                        existingNames.Add(entryNames[i]);
                    }
                    migratedPrefixes.Add(oldPrefix);
                }

                List<string> missingReferences = referencePoints.Where(x => !points.ContainsKey(x)).ToList();
                if (missingReferences.Count > 0)
                {
                    message = $"Black reference points not found in the installation file: {string.Join(", ", missingReferences)}.";
                    return false;
                }

                XElement insertAfter = pointWrappers.Last();

                // Transparent_P10Entry = copy of Black_P10Entry, only when the Transparent set is in the file
                bool transparentEntry10Added = false;
                if (existingNames.Contains(GetPointName(transparentPrefix, 1)) && !existingNames.Contains(transparentEntry10))
                {
                    insertAfter = insertCopy(points[blackEntry10], transparentEntry10, insertAfter);
                    existingNames.Add(transparentEntry10);
                    transparentEntry10Added = true;
                }

                string[] newNames = GetPointNames(prefix);
                int existingCount = newNames.Count(x => existingNames.Contains(x));

                if (existingCount > 0 && existingCount < newNames.Length)
                {
                    message = $"Only {existingCount} of {newNames.Length} points with prefix \"{prefix}\" exist in the installation file. Please check the robot points.";
                    return false;
                }

                if (existingCount == 0)
                {
                    // new points go after the last existing point
                    for (int i = 0; i < referencePoints.Length; i++)
                    {
                        insertAfter = insertCopy(points[referencePoints[i]], newNames[i], insertAfter);
                        existingNames.Add(newNames[i]);
                    }
                }

                writeInstallation(installation, installationFile);

                List<string> skipped;
                File.WriteAllText(scriptFile, buildScript(recipes, existingNames, out skipped), new UTF8Encoding(false));

                message = existingCount == 0
                    ? $"{newNames.Length} points created ({newNames.First()} ... {newNames.Last()})."
                    : $"Points {newNames.First()} ... {newNames.Last()} already exist, kept as they are.";
                if (renamedCount > 0)
                {
                    message += $"\n{renamedCount} old points renamed (Point_1b -> {GetPointName(blackPrefix, 1)}, Point_t1 -> {GetPointName(transparentPrefix, 1)}, ...).";
                }
                if (migratedPrefixes.Count > 0)
                {
                    message += $"\nOld entry points P22..P33 renamed to PxxEntry: {string.Join(", ", migratedPrefixes)}.";
                }
                if (transparentEntry10Added)
                {
                    message += $"\n{transparentEntry10} added (copy of {blackEntry10}).";
                }
                if (skipped.Count > 0)
                {
                    message += "\n\nNot in the script (no robot points):\n" + string.Join("\n", skipped);
                }

                logger.Info($"Robot points: prefix {prefix}, {(existingCount == 0 ? "created" : "already exist")}, {renamedCount} renamed, " +
                    $"{sourceFile} -> {installationFile}, {scriptFile}");
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
        private static Dictionary<string, string> buildRenames()
        {
            Dictionary<string, string> renames = new Dictionary<string, string>();
            for (int screw = 1; screw <= screwCount; screw++)
            {
                renames.Add($"Point_{screw}b", GetPointName(blackPrefix, screw));
                renames.Add($"Point_t{screw}", GetPointName(transparentPrefix, screw));
            }
            foreach (int screw in entryScrews)
            {
                renames.Add($"Point_{screw}bEntry", GetEntryPointName(blackPrefix, screw));
            }

            // Transparent entry names in the robot program 05092022, screw 10 has none (Black is used)
            foreach (int screw in new[] { 1, 2, 3 })
            {
                renames.Add($"Point_t{screw}e", GetEntryPointName(transparentPrefix, screw));
            }
            foreach (int screw in new[] { 7, 8, 16, 17, 18, 19, 20, 21 })
            {
                renames.Add($"Point_t{screw}Entry", GetEntryPointName(transparentPrefix, screw));
            }
            return renames;
        }

        private static XElement insertCopy(XElement source, string name, XElement insertAfter)
        {
            XElement copy = new XElement(source);
            XElement point = copy.Element("GeomPoseNode");
            point.SetAttributeValue("name", name);
            point.SetAttributeValue("id", Guid.NewGuid().ToString());

            insertAfter.AddAfterSelf(copy);
            return copy;
        }

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

                if (string.IsNullOrEmpty(prefix))
                {
                    if (recipe.RecipeID == builtInBlackID) { prefix = blackPrefix; }
                    else if (recipe.RecipeID == builtInTransparentID) { prefix = transparentPrefix; }
                    else
                    {
                        skipped.Add($"{recipe.DisplayName} (no prefix)");
                        continue;
                    }
                }

                string[] height = Enumerable.Range(1, screwCount).Select(x => GetPointName(prefix, x)).ToArray();
                string[] entry = entryScrews.Select(x => GetEntryPointName(prefix, x)).ToArray();

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
            script.Append("# rcp_height: index 1-21 = screw 1-21 (<Colour>_P01 .. <Colour>_P21).\n");
            script.Append("# rcp_entry : index 1-12 = entry for screw ").Append(string.Join(",", entryScrews)).Append(" (<Colour>_Pxx Entry).\n");
            script.Append("#             Screws without an entry are not listed. Index 0 is a placeholder in both lists.\n");
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
