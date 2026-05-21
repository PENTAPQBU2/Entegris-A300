using HMI_Master_KX7500.Interfaces;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace HMI_Master_KX7500.Helpers
{
    public class LogHelper
    {
        #region PrivateFields

        private static readonly ILog iLog = new NLogService();

        #endregion PrivateFields

        #region PublicMethods
        public static void COMPort(string className, string message)
        {
            iLog.COMPort(className, message);
        }

        //---------------------------
        public static void CreateDone(string className, string message)
        {
            iLog.Debug(className, $"Create {message} done.");
        }

        public static void Error(string className, Exception exception)
        {
            iLog.Error(className, exception);
        }

        public static void Error(string className, string message)
        {
            iLog.Error(className, message);
        }

        public static void Event(string className, string message)
        {
            iLog.Event(className, message);
        }

        public static void KeyValue(string className, string key, object value)
        {
            iLog.Debug(className, $"{MessageHelper.KeyValue(key, value)}.");
        }

        public static void OPCTrace(string className, string message)
        {
            iLog.OPCTrace(className, message);
        }

        public static void StartTo(string className, string name)
        {
            iLog.Debug(className, $"Start to {name}.");
        }

        public static void Done(string className, string name)
        {
            iLog.Debug(className, MessageHelper.Done(name));
        }

        public static void FailTo(string className, string name)
        {
            iLog.Debug(className, MessageHelper.FailTo(name));
        }

        public static void StartToExecute(string className, string name)
        {
            iLog.Debug(className, $"Start to execute {name}.");
        }

        public static void ExecuteDone(string className, string name)
        {
            iLog.Debug(className, $"Execute {name} done.");
        }

        public static void FailToExecute(string className, string name)
        {
            iLog.Debug(className, $"Fail to execute {name}.");
        }

        public static void StartToGet(string className, string name)
        {
            iLog.Debug(className, $"Start to get {name}.");
        }

        public static void GetDone(string className, string name)
        {
            iLog.Debug(className, $"Get {name} done.");
        }

        public static void FailToGet(string className, string name)
        {
            iLog.Debug(className, $"Fail to get {name}.");
        }

        public static void StartToInitialize(string className, string name)
        {
            iLog.Debug(className, $"Start to initialize {name}.");
        }

        public static void InitializeDone(string className, string name)
        {
            iLog.Debug(className, $"Initialize {name} done.");
        }

        public static void FailToInitialize(string className, string name)
        {
            iLog.Debug(className, $"Fail to initialize {name}.");
        }

        public static void Startup(string className, string name)
        {
            iLog.Debug(className, $"{name} startup.");
        }

        public static void Exit(string className, string name)
        {
            iLog.Debug(className, $"{name} exit.");
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
            catch(Exception ex)
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
                if(!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion PublicMethods
    }
}