using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Robot;
using System;
using System.IO;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Service.Robot
{
    public static class RobotSettingsService
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(RobotSettingsModel));

        public static string SettingsFile => Path.Combine(FolderPath.MachineSettings, "RobotSettings.xml");

        public static RobotSettingsModel Load()
        {
            try
            {
                if(File.Exists(SettingsFile))
                {
                    using(FileStream stream = File.OpenRead(SettingsFile))
                    {
                        return (RobotSettingsModel)serializer.Deserialize(stream);
                    }
                }
            }
            catch(Exception ex) { LogHelper.Error(nameof(RobotSettingsService), ex); }

            RobotSettingsModel settings = new RobotSettingsModel();
            Save(settings);
            return settings;
        }

        public static void Save(RobotSettingsModel settings)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile));
                using(FileStream stream = File.Create(SettingsFile))
                {
                    serializer.Serialize(stream, settings);
                }
            }
            catch(Exception ex) { LogHelper.Error(nameof(RobotSettingsService), ex); }
        }
    }
}
