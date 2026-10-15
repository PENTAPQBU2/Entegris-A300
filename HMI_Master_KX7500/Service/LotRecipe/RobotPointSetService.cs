using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.LotRecipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Service.LotRecipe
{
    //Robot point sets (MT_001). Creates the 33 point features of a new set in a copy of the UR installation file and
    //writes recipe_points.script, which the robot program uses to build its screw lists:
    //  BeforeStart: Script node (File) recipe_points.script
    //  Main_PLC:    screwHeightList := rcp_height(shell_int), screwEntryList := rcp_entry(shell_int)
    //See Docs/UR_Robot/UR_Program_Change_Spec.md.
    public static class RobotPointSetService
    {
        public const int PointCount = 33;
        public const string ScriptFileName = "recipe_points.script";

        //Screws with their own entry point, in P22..P33 order. Other screws enter at the screw point.
        public static readonly int[] EntryScrews = { 1, 2, 3, 7, 8, 10, 16, 17, 18, 19, 20, 21 };

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(RobotPointSetListModel));

        public static string SettingsFile => Path.Combine(FolderPath.MachineSettings, "RobotPointSets.xml");

        #region Point set list
        public static List<RobotPointSetModel> LoadAll()
        {
            try
            {
                if(File.Exists(SettingsFile))
                {
                    using(FileStream stream = File.OpenRead(SettingsFile))
                    {
                        RobotPointSetListModel list = (RobotPointSetListModel)serializer.Deserialize(stream);
                        if(list.PointSets.Count > 0)
                        {
                            return list.PointSets.OrderBy(x => x.SetNo).ToList();
                        }
                    }
                }
            }
            catch(Exception ex) { LogHelper.Error(nameof(RobotPointSetService), ex); }

            List<RobotPointSetModel> defaults = DefaultSets();
            SaveAll(defaults);
            return defaults;
        }

        public static void SaveAll(List<RobotPointSetModel> sets)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile));
            string temp = SettingsFile + ".tmp";
            using(FileStream stream = File.Create(temp))
            {
                serializer.Serialize(stream, new RobotPointSetListModel { PointSets = sets.OrderBy(x => x.SetNo).ToList() });
            }
            if(File.Exists(SettingsFile))
            {
                File.Replace(temp, SettingsFile, SettingsFile + ".bak");
            }
            else
            {
                File.Move(temp, SettingsFile);
            }
        }

        //The two point sets of the original robot program (A300 Shell 05092022, Switch shell_int Case 0 and Case 1)
        public static List<RobotPointSetModel> DefaultSets()
        {
            RobotPointSetModel black = new RobotPointSetModel { SetNo = 0, Name = "Black (EBM)" };
            for(int screw = 1; screw <= 21; screw++)
            {
                black.Points.Add($"Point_{screw}b");
            }
            foreach(int screw in EntryScrews)
            {
                black.Points.Add($"Point_{screw}bEntry");
            }

            //The transparent set uses differently named entry points, and the black entry point for screw 10
            Dictionary<int, string> transparentEntry = new Dictionary<int, string>
            {
                { 1, "Point_t1e" }, { 2, "Point_t2e" }, { 3, "Point_t3e" },
                { 7, "Point_t7Entry" }, { 8, "Point_t8Entry" }, { 10, "Point_10bEntry" }
            };
            RobotPointSetModel transparent = new RobotPointSetModel { SetNo = 1, Name = "Transparent (PC Clear)" };
            for(int screw = 1; screw <= 21; screw++)
            {
                transparent.Points.Add($"Point_t{screw}");
            }
            foreach(int screw in EntryScrews)
            {
                transparent.Points.Add(transparentEntry.ContainsKey(screw) ? transparentEntry[screw] : $"Point_t{screw}Entry");
            }

            return new List<RobotPointSetModel> { black, transparent };
        }

        public static List<string> PrefixPoints(string prefix)
        {
            List<string> points = new List<string>();
            for(int p = 1; p <= PointCount; p++)
            {
                points.Add($"{prefix}_P{p:00}");
            }
            return points;
        }

        //Names in robot list order: index 1..21 = screw 1..21 (index 0 is unused by the robot program)
        public static List<string> HeightList(RobotPointSetModel set)
        {
            return set.Points.Take(21).ToList();
        }

        public static List<string> EntryList(RobotPointSetModel set)
        {
            List<string> entry = new List<string>();
            for(int screw = 1; screw <= 21; screw++)
            {
                int index = Array.IndexOf(EntryScrews, screw);
                entry.Add(index >= 0 ? set.Points[21 + index] : set.Points[screw - 1]);
            }
            return entry;
        }
        #endregion

        #region Installation file
        private class InstallationText
        {
            public string Text;
            public bool Gzip;
        }

        //PolyScope saves the installation gzip-compressed; a plain XML copy is read as well
        private static InstallationText readInstallation(string file)
        {
            byte[] data = File.ReadAllBytes(file);
            bool gzip = data.Length > 2 && data[0] == 0x1f && data[1] == 0x8b;
            if(!gzip)
            {
                return new InstallationText { Text = Encoding.UTF8.GetString(data), Gzip = false };
            }
            using(MemoryStream input = new MemoryStream(data))
            using(GZipStream unzip = new GZipStream(input, CompressionMode.Decompress))
            using(StreamReader reader = new StreamReader(unzip, Encoding.UTF8))
            {
                return new InstallationText { Text = reader.ReadToEnd(), Gzip = true };
            }
        }

        private static void writeInstallation(string file, string text, bool gzip)
        {
            byte[] data = new UTF8Encoding(false).GetBytes(text);
            string temp = file + ".tmp";
            using(FileStream output = File.Create(temp))
            {
                if(gzip)
                {
                    using(GZipStream zip = new GZipStream(output, CompressionMode.Compress))
                    {
                        zip.Write(data, 0, data.Length);
                    }
                }
                else
                {
                    output.Write(data, 0, data.Length);
                }
            }
            File.Copy(temp, file, true);
            File.Delete(temp);
        }

        private static XElement features(XDocument doc)
        {
            XElement section = doc.Root?.Element("Features");
            if(section == null)
            {
                throw new InvalidDataException("This file has no Features section. Select a UR installation file (.installation).");
            }
            return section;
        }

        //All named point features of the installation, including points inside lines and planes
        public static HashSet<string> FeatureNames(string installationFile)
        {
            XDocument doc = XDocument.Parse(readInstallation(installationFile).Text);
            return new HashSet<string>(features(doc).Descendants("GeomPoseNode")
                .Select(x => (string)x.Attribute("name")).Where(x => !string.IsNullOrEmpty(x)));
        }

        //Copies the installation to <HMI folder>\RobotBackup\ before it is changed
        public static string Backup(string installationFile)
        {
            Directory.CreateDirectory(FolderPath.RobotBackup);
            string backup = Path.Combine(FolderPath.RobotBackup, $"{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(installationFile)}");
            File.Copy(installationFile, backup, false);
            return backup;
        }

        //Returns null when the new set can be created, otherwise the reason
        public static string Validate(List<RobotPointSetModel> sets, RobotPointSetModel source, int setNo, string name, string prefix)
        {
            if(source == null)
            {
                return "Select the point set to copy from.";
            }
            if(setNo < 0 || setNo > LotRecipeService.MaxRobotPointSet)
            {
                return $"Point set number must be 0 to {LotRecipeService.MaxRobotPointSet}.";
            }
            if(sets.Any(x => x.SetNo == setNo))
            {
                return $"Point set {setNo} already exists.";
            }
            if(string.IsNullOrWhiteSpace(name))
            {
                return "Enter a name for the point set, e.g. PC Amber.";
            }
            if(prefix == null || !Regex.IsMatch(prefix, "^[A-Za-z][A-Za-z0-9]{0,7}$"))
            {
                return "Prefix must start with a letter and have 1 to 8 letters or digits, e.g. Amb.";
            }
            if(sets.Any(x => string.Equals(x.Prefix, prefix, StringComparison.OrdinalIgnoreCase)))
            {
                return $"Prefix {prefix} is already used.";
            }
            return null;
        }

        //Adds the 33 points of the new set (copies of the source set's values) to the installation file, in one block
        //after the last point feature, and adds the set to RobotPointSets.xml. The file is changed in place; a copy is
        //kept in RobotBackup first. Returns the backup file name.
        public static string CreatePointSet(string installationFile, RobotPointSetModel source, int setNo, string name, string prefix)
        {
            List<RobotPointSetModel> sets = LoadAll();
            string problem = Validate(sets, source, setNo, name, prefix);
            if(problem != null)
            {
                throw new InvalidOperationException(problem);
            }

            InstallationText installation = readInstallation(installationFile);
            XDocument doc = XDocument.Parse(installation.Text);
            XElement featureSection = features(doc);
            Dictionary<string, XElement> existing = featureSection.Descendants("GeomPoseNode")
                .Where(x => !string.IsNullOrEmpty((string)x.Attribute("name")))
                .GroupBy(x => (string)x.Attribute("name")).ToDictionary(x => x.Key, x => x.First());

            List<string> missing = source.Points.Where(x => !existing.ContainsKey(x)).ToList();
            if(missing.Count > 0)
            {
                throw new InvalidOperationException($"The installation has no point {missing[0]} of set {source.Name} ({missing.Count} missing). Check the file is the robot's installation.");
            }
            List<string> newPoints = PrefixPoints(prefix);
            string clash = newPoints.FirstOrDefault(x => existing.ContainsKey(x));
            if(clash != null)
            {
                throw new InvalidOperationException($"The installation already has a point named {clash}.");
            }

            //Point features are wrapped: <GeomPoseNode><GeomPoseNode name=...>...</GeomPoseNode></GeomPoseNode>
            XElement lastWrapper = featureSection.Elements("GeomPoseNode").LastOrDefault(x => x.Element("GeomPoseNode")?.Attribute("name") != null);
            if(lastWrapper == null)
            {
                throw new InvalidDataException("No point feature found in the installation.");
            }
            string lastName = (string)lastWrapper.Element("GeomPoseNode").Attribute("name");

            string text = installation.Text;
            string newline = text.Contains("\r\n") ? "\r\n" : "\n";
            int at = text.IndexOf($"name=\"{lastName}\"", text.IndexOf("<Features>", StringComparison.Ordinal), StringComparison.Ordinal);
            int innerEnd = at < 0 ? -1 : text.IndexOf("</GeomPoseNode>", at, StringComparison.Ordinal);
            int outerEnd = innerEnd < 0 ? -1 : text.IndexOf("</GeomPoseNode>", innerEnd + 1, StringComparison.Ordinal);
            if(outerEnd < 0)
            {
                throw new InvalidDataException("Cannot find where to add the points in the installation file.");
            }
            int insertAt = outerEnd + "</GeomPoseNode>".Length;

            StringBuilder block = new StringBuilder();
            for(int i = 0; i < PointCount; i++)
            {
                XElement src = existing[source.Points[i]];
                string joints = (string)src.Element("jointPositionVector")?.Attribute("value") ?? "NaN, NaN, NaN, NaN, NaN, NaN";
                string position = (string)src.Element("toolPosition")?.Attribute("value");
                string rotation = (string)src.Element("toolAxisAngle")?.Attribute("value");
                string tcp = (string)src.Element("tcpOffset")?.Attribute("value") ?? "0.0, 0.0, 0.0, 0.0, 0.0, 0.0";
                block.Append(newline).Append("    <GeomPoseNode>")
                     .Append(newline).Append($"      <GeomPoseNode showAxes=\"true\" joggable=\"true\" isVariable=\"true\" id=\"{Guid.NewGuid()}\" name=\"{newPoints[i]}\">")
                     .Append(newline).Append($"        <jointPositionVector value=\"{joints}\"/>")
                     .Append(newline).Append($"        <toolPosition value=\"{position}\"/>")
                     .Append(newline).Append($"        <toolAxisAngle value=\"{rotation}\"/>")
                     .Append(newline).Append($"        <tcpOffset value=\"{tcp}\"/>")
                     .Append(newline).Append("      </GeomPoseNode>")
                     .Append(newline).Append("    </GeomPoseNode>");
            }
            string changed = text.Substring(0, insertAt) + block + text.Substring(insertAt);

            //Check the result before writing: still valid XML and exactly 33 more points
            int before = existing.Count;
            int after = features(XDocument.Parse(changed)).Descendants("GeomPoseNode").Count(x => !string.IsNullOrEmpty((string)x.Attribute("name")));
            if(after != before + PointCount)
            {
                throw new InvalidDataException("The changed installation did not check out; nothing was written.");
            }

            string backup = Backup(installationFile);
            writeInstallation(installationFile, changed, installation.Gzip);

            sets.Add(new RobotPointSetModel { SetNo = setNo, Name = name.Trim(), Prefix = prefix, Points = newPoints });
            SaveAll(sets);
            LogHelper.KeyValue(nameof(RobotPointSetService), "Point set created", $"{setNo} {name} {prefix} in {installationFile}, backup {backup}");
            return backup;
        }
        #endregion

        #region recipe_points.script
        //Writes recipe_points.script next to the installation file. Only sets whose 33 points are all in the
        //installation are included (a missing name would stop the robot program from compiling).
        //Returns the names of the sets that were left out.
        public static List<string> WriteScript(string installationFile)
        {
            HashSet<string> names = FeatureNames(installationFile);
            List<RobotPointSetModel> included = new List<RobotPointSetModel>();
            List<string> skipped = new List<string>();
            foreach(RobotPointSetModel set in LoadAll())
            {
                if(set.Points.Count == PointCount && set.Points.All(names.Contains))
                {
                    included.Add(set);
                }
                else
                {
                    skipped.Add($"{set.SetNo} {set.Name}");
                }
            }

            string file = Path.Combine(Path.GetDirectoryName(installationFile), ScriptFileName);
            File.WriteAllText(file, BuildScript(included), new UTF8Encoding(false));
            return skipped;
        }

        public static string BuildScript(List<RobotPointSetModel> sets)
        {
            StringBuilder s = new StringBuilder();
            s.Append("# Robot point sets for the A300 recipes. Generated by the HMI (Recipe page) on ")
             .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).Append(". Do not edit by hand.\n");
            s.Append("# shell_int (W0102, from HMI_Robot_PointSet EM4250) selects the set.\n");
            s.Append("# P1-P21 = screw 1-21, P22-P33 = entry above screw 1,2,3,7,8,10,16-21 (other screws enter at the screw point).\n\n");
            appendFunction(s, "rcp_height", sets, HeightList);
            appendFunction(s, "rcp_entry", sets, EntryList);
            return s.ToString();
        }

        private static void appendFunction(StringBuilder s, string function, List<RobotPointSetModel> sets, Func<RobotPointSetModel, List<string>> list)
        {
            s.Append($"def {function}(n):\n");
            for(int i = 0; i < sets.Count; i++)
            {
                s.Append($"  {(i == 0 ? "if" : "elif")} n == {sets[i].SetNo}:   # {sets[i].Name}\n");
                s.Append("    return [p[0,0,0,0,0,0], ").Append(string.Join(", ", list(sets[i]))).Append("]\n");
            }
            if(sets.Count > 0)
            {
                s.Append("  end\n");
            }
            s.Append("  popup(str_cat(\"Unknown robot point set \", n), \"Recipe\", error=True, blocking=True)\n");
            s.Append("  halt\n");
            s.Append("end\n\n");
        }
        #endregion
    }
}
