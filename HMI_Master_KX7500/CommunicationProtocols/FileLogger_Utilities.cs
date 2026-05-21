using HMI_Master_KX7500.MachinePath;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMLine
{
    public static class FileLogger_Utilities
    {
        public static void LogInfo_RS232_Left(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\RS232_Logs_Left");
            string fileName = $"RS232_Left_Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        public static void LogInfo_RS232_Right(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\RS232_Logs_Right");
            string fileName = $"RS232_Right_Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        public static void Log_LoadRecipe_TagAddress_And_Value(string desc, string tagAddress, string recipeValue)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\Recipe_Logs");
            string fileName = $"Log_LoadRecipe_TagAddress_And_Value_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} => Desc : {desc}, Tag Address : {tagAddress} || RecipeValue : {recipeValue}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} => Desc : {desc}, Tag Address : {tagAddress} || RecipeValue : {recipeValue}");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        public static void AlarmLog(string AlarmCode, string Type, string Message)
        {
            try
            {
                Writer($@"Alarm\{Type}", Type, new Dictionary<string, string>
                {
                    ["ErrorCode"] = AlarmCode,
                    ["Type"] = Type,
                    ["Message"] = Message,
                    ["DateTime"] = DateTime.Now.ToString("yyyy-MMM-dd_HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Writer(string strPath, string FileName, Dictionary<string, string> Dic_Header_Content)
        {
            try
            {
                strPath = FolderPath.Logs + strPath;
                FileName += $"AlarmLog_{DateTime.Now.ToString("yyyy-MMM-dd")}.csv";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                bool IsNew = false;
                if (!File.Exists(strPath + Path.DirectorySeparatorChar + FileName))
                    IsNew = true;

                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + FileName), IsNew ? FileMode.CreateNew : FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    if (IsNew) writer.WriteLine(string.Join(",", Dic_Header_Content.Keys));
                    writer.WriteLine(string.Join(",", Dic_Header_Content.Values));
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LogInformationGRR(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\LogInformationGRR");
            string fileName = $"GRR_LogInfo_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} || {message}");
                    writer.WriteLine("-----------------------------------------------------------");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} || {message}");
                    writer.WriteLine("-----------------------------------------------------------");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        public static void LogInformationValueData(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\LogInformationValueData");
            string fileName = $"ValueDataLog_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} || {message}");
                    writer.WriteLine("-----------------------------------------------------------");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} || {message}");
                    writer.WriteLine("-----------------------------------------------------------");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        public static void LogInformationMain_Shunt(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\LogInformationMain_Shunt");
            string fileName = $"Main_Shunt_LogInfo_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} || {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} || {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
        }

        public static void BottomScanner_1(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\BottomScanner_1_COM7");
            string fileName = $"BottomScanner_1_Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        public static void BottomScanner_2(string message)
        {
            string strPath = Path.Combine($"{FolderPath.Log_Infomation}\\BottomScanner_2_COM8");
            string fileName = $"BottomScanner_2_Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (!File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else if (File.Exists(strPath + Path.DirectorySeparatorChar + fileName))
            {
                using (FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + fileName),
                       FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now.ToString()} : {message}");
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
        }

        //!!!Foong0307
        public static void TCPIP_Log(string message)
        {
            string strPath = FolderPath.CamLineLog;
            string fileName = $"Communication_Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            string combinedPath = Path.Combine(strPath, fileName);

            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }

            if (File.Exists(combinedPath))
            {
                using (FileStream stream = new FileStream((combinedPath),
                    FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"DateTime,Message");
                    writer.WriteLine($"{DateTime.Now.ToString()} , {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
            else
            {
                using (FileStream stream = new FileStream((combinedPath),
                    FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"DateTime,Message");
                    writer.WriteLine($"{DateTime.Now.ToString()} , {message}");
                    writer.Flush();
                    writer.Close();
                    writer.Close();
                }
            }
        }

    }
}
