using HMI_Master_KX7500.MachinePath;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CAMLine
{
    class LoadSettingService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region ServerSetting
        public delegate void UpdateServerSetting(SetupSettingModel serverSettingModel);
        public static event UpdateServerSetting OnUpdate_ServerSetting;
        public static void LoadServerSetting()
        {
            try
            {
                if (File.Exists(FilePath.Setup_Settings_File))
                {
                    if (OnUpdate_ServerSetting != null)
                    {
                        SetupSettingModel serverSettingModel = new SetupSettingModel();
                        serverSettingModel = XmlTools.Deserialize<SetupSettingModel>(FilePath.Setup_Settings_File, serverSettingModel);
                        OnUpdate_ServerSetting(serverSettingModel);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion
    }
}
