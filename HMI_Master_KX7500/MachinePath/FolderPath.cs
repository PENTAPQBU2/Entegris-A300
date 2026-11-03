namespace HMI_Master_KX7500.MachinePath
{
    public class FolderPath
    {
        public static string RecipeFolderPath = string.Empty;
        public static string LanguageFolderPath = string.Empty;

        public static string Logs = string.Empty;
        public static string MachineConfiguration = string.Empty;
        public static string MachineSettings = string.Empty;
        public static string OEE = string.Empty;
        public static string Images = string.Empty;
        //public static string DBCLog = string.Empty;
        //public static string DBCLogCopy = string.Empty;

        #region Camline Folder Path
        //public static string Logs = string.Empty;
        public static string Recipe = string.Empty;
        public static string Settings = string.Empty;
        public static string Log_Infomation = string.Empty; // to save all the folder that have log
        //public static string IOLocation = string.Empty;
        public static string CamLineLog = string.Empty;
        public static string RecipeSetting = string.Empty;
        public static string FoupDataFolder = string.Empty;
        #endregion

        #region FOUP CSV (MT_003)
        // FOUP CSV\<LotID>\<LotID>_<00001>_Pass\<yyyyMMddHHmmss>.csv, lot folder created by New Lot
        // Property, not a field: FrameView.initializeFolderPath() sets every public field of this class by
        // reflection (GetFields), a const field there throws and stops the HMI start-up (all pages empty).
#if DEBUG
        public static string FoupCsv { get { return @"D:\HMI_A300\FOUP CSV\"; } }
#else
        public static string FoupCsv { get { return @"D:\HMI\FOUP CSV\"; } }
#endif

        // Lot ID as folder name, empty Lot ID (shell without lot) -> "NoLotID"
        public static string FoupCsvLotName(string lotId)
        {
            string name = (lotId ?? string.Empty).Replace("\0", string.Empty).Trim();
            if (string.IsNullOrEmpty(name))
            {
                return "NoLotID";
            }
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        public static string FoupCsvLotFolder(string lotId)
        {
            return System.IO.Path.Combine(FoupCsv, FoupCsvLotName(lotId));
        }
        #endregion
    }
}