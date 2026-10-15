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
    /// Robot points per recipe (MT_001): copies the Black reference points of a UR installation file
    /// (PolyScope *.installation = gzip XML) under a recipe prefix, e.g. Point_1b -> Blue_Point_1, Point_1bEntry -> Blue_Point_1Entry.
    /// The operator copies the robot installation to USB, creates the points on HMI and loads the file back on the robot.
    /// </summary>
    public class RobotPointService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const int MaxPrefixLength = 5;

        // Black point set = 21 screw points Point_<n>b + 12 approach points Point_<n>bEntry (33 points)
        private static readonly Regex referencePointName = new Regex(@"^Point_(\d+)b(Entry)?$");

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

        // Point_1b -> Blue_Point_1, Point_1bEntry -> Blue_Point_1Entry (the b of the Black set is dropped)
        public static string GetPointName(string prefix, string referenceName)
        {
            Match match = referencePointName.Match(referenceName);
            return $"{NormalizePrefix(prefix)}_Point_{match.Groups[1].Value}{match.Groups[2].Value}";
        }

        /// <summary>
        /// Reads sourceFile, adds a copy of every reference point named prefix + reference name, writes targetFile.
        /// sourceFile and targetFile may be the same file. Returns the number of points created, error = reason when 0.
        /// </summary>
        public static int CreatePoints(string sourceFile, string targetFile, string prefix, out string error)
        {
            error = string.Empty;

            try
            {
                if (!IsValidPrefix(prefix))
                {
                    error = $"Invalid prefix \"{prefix}\".";
                    return 0;
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
                    error = "File is not a robot installation file.";
                    return 0;
                }

                // each point is <GeomPoseNode><GeomPoseNode id=".." name="..">..</GeomPoseNode></GeomPoseNode>
                List<XElement> pointWrappers = features.Elements("GeomPoseNode").ToList();
                List<XElement> references = pointWrappers
                    .Where(x => referencePointName.IsMatch((string)x.Element("GeomPoseNode")?.Attribute("name") ?? string.Empty))
                    .ToList();

                if (references.Count == 0)
                {
                    error = "Reference points (Point_1b ...) not found in the installation file.";
                    return 0;
                }

                HashSet<string> existingNames = new HashSet<string>(
                    features.Descendants().Select(x => (string)x.Attribute("name")).Where(x => x != null),
                    StringComparer.OrdinalIgnoreCase);
                List<string> duplicates = references
                    .Select(x => GetPointName(prefix, (string)x.Element("GeomPoseNode").Attribute("name")))
                    .Where(x => existingNames.Contains(x))
                    .ToList();

                if (duplicates.Count > 0)
                {
                    error = $"Points with prefix \"{prefix}\" already exist (e.g. {duplicates[0]}).";
                    return 0;
                }

                // new points go after the last existing point
                XElement insertAfter = pointWrappers.Last();
                foreach (XElement reference in references)
                {
                    XElement copy = new XElement(reference);
                    XElement point = copy.Element("GeomPoseNode");
                    point.SetAttributeValue("name", GetPointName(prefix, (string)point.Attribute("name")));
                    point.SetAttributeValue("id", Guid.NewGuid().ToString());

                    insertAfter.AddAfterSelf(copy);
                    insertAfter = copy;
                }

                // same layout as PolyScope: no declaration, 2 space indent, LF, UTF-8 without BOM
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    Encoding = new UTF8Encoding(false)
                };

                // build in memory first, sourceFile can be the targetFile
                using (MemoryStream buffer = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(buffer, CompressionMode.Compress, true))
                    using (XmlWriter writer = XmlWriter.Create(gzip, settings))
                    {
                        installation.Save(writer);
                    }
                    File.WriteAllBytes(targetFile, buffer.ToArray());
                }

                logger.Info($"Robot points created: prefix {prefix}, {references.Count} points, {sourceFile} -> {targetFile}");
                return references.Count;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                error = ex.Message;
            }

            return 0;
        }
        #endregion
    }
}
