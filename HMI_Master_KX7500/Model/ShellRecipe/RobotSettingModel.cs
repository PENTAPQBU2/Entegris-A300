namespace HMI_Master_KX7500.Model.ShellRecipe
{
    /// <summary>
    /// MachineConfiguration\Robot_Settings_File.xml: the UR robot pendant reached over SSH / SFTP by Create Points (MT_001).
    /// Written with these defaults when the file is missing.
    /// </summary>
    public class RobotSettingModel
    {
        public string Host { get; set; } = "192.168.0.1";

        public int Port { get; set; } = 22;

        public string User { get; set; } = "root";

        public string Password { get; set; } = "#penta123";

        /// <summary>Folder on the pendant with the installation in use; the new "Robot Program (..)" folders are made here too.</summary>
        public string ProgramFolder { get; set; } = "/programs";

        /// <summary>Installation in use on the robot = source of Create Points; the new one gets the same name.</summary>
        public string InstallationFileName { get; set; } = "A300_SAM_Robot_Ins_file.installation";

        /// <summary>Name of the new script, followed by the date and time and ".script".</summary>
        public string ScriptBaseName { get; set; } = "A300 Shell MC2";

        public int TimeoutMs { get; set; } = 5000;

        public string RemotePath(string name) => $"{(ProgramFolder ?? string.Empty).TrimEnd('/')}/{name}";
    }
}
