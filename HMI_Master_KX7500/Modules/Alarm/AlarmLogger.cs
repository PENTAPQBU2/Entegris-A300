using HMI_Master_KX7500.MachinePath;
using Microsoft.VisualBasic.FileIO;
using NLog;
using Opc.Ua;
using Opc.Ua.Schema.Binary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMI_Master_KX7500.Modules.Alarm
{
    internal class AlarmLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void AlarmLog(string AlarmCode, string Type, string Message)
        {
            try
            {
                Writer($@"AlarmLog\{Type}", Type, new Dictionary<string, string>
                {
                    ["ErrorCode"] = AlarmCode,
                    ["Type"] = Type,
                    ["Message"] = Message,
                    ["DateTime"] = DateTime.Now.ToString("yyyy-MMM-dd_HH:mm:ss")
                });
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        public static async Task<List<string>> ReadAlarmLog(string Type, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                List<string> allAlarmLogs = new List<string>();
                DateTime dateInProcess = StartDate;
                while (dateInProcess <= EndDate)
                {
                    var alarmLogs = await Reader(Type, dateInProcess);
                    if (alarmLogs.Count > 0)
                    {
                        allAlarmLogs.AddRange(alarmLogs);
                    }
                    else
                    {
                        logger.Log(LogLevel.Debug, $"{dateInProcess} no logs");
                    }
                    dateInProcess = dateInProcess.AddDays(1);
                    await Task.Delay(10);
                }
                logger.Log(LogLevel.Debug, $"AllAlarmLogs Count: {allAlarmLogs.Count}");
                return allAlarmLogs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void Writer(string strPath, string FileName, Dictionary<string, string> Dic_Header_Content)
        {
            try
            {
                strPath = FolderPath.Logs + strPath;
                FileName += $"AlarmLog_{DateTime.Now.ToString("yyyy-MMM-dd")}.csv";
                if(!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                logger.Log(LogLevel.Debug, $"AlarmLog strPath: {strPath}");
                logger.Log(LogLevel.Debug, $"AlarmLog FileName: {FileName}");

                bool IsNew = false;
                if(!File.Exists(strPath + Path.DirectorySeparatorChar + FileName))
                    IsNew = true;

                using(FileStream stream = new FileStream((strPath + Path.DirectorySeparatorChar + FileName), IsNew ? FileMode.CreateNew : FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    if(IsNew)
                        writer.WriteLine(string.Join(",", Dic_Header_Content.Keys));
                    writer.WriteLine(string.Join(",", Dic_Header_Content.Values));
                    writer.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private static async Task<List<string>> Reader(string Type, DateTime dateToRead)
        {
            try
            {
                string strPath = FolderPath.Logs + $@"AlarmLog\{Type}";
                string FileName = $"{Type}AlarmLog_{dateToRead.ToString("yyyy-MMM-dd")}.csv";
                string path = Path.Combine(strPath, FileName);
                logger.Log(LogLevel.Debug, $"AlarmLogFile: {path}");
                if (!File.Exists(path))
                    return new List<string>();

                List<string> lines = new List<string>();
                using (TextFieldParser parser = new TextFieldParser(path))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields[0] != "ErrorCode")
                        {
                            string line = $"{fields[0]}, {fields[1]}, {fields[2]}, {fields[3]}";
                            lines.Add(line);
                        }
                        await Task.Delay(10);
                    }
                }

                return lines;
            }
            catch
            {
                throw;
            }
        }
    }
}