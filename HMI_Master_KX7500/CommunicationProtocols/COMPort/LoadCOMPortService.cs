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

namespace HMI_SSDC_PIM.CommunicationProtocols.COMPort
{
    class LoadCOMPortService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region COMPortSetting
        public delegate void UpdateCOMPortSetting(COMPortSettingModel comPortSettingModel);
        public static event UpdateCOMPortSetting OnUpdate_COMPortSetting;
        public static void LoadCOMPortSetting()
        {
            try
            {
                if (File.Exists(FilePath.COMPort_Settings_File))
                {
                    if (OnUpdate_COMPortSetting != null)
                    {
                        COMPortSettingModel comPortSettingModel = new COMPortSettingModel();
                        comPortSettingModel = XmlTools.Deserialize<COMPortSettingModel>(FilePath.COMPort_Settings_File, comPortSettingModel);
                        OnUpdate_COMPortSetting(comPortSettingModel);
                    }
                }
                else
                {
                    if (OnUpdate_COMPortSetting != null)
                    {
                        COMPortSettingModel comPortSettingModel = new COMPortSettingModel
                        {
                            PortName = "COM1",
                            BaudRate = 0,
                            DataBits = 0,
                        };
                        XmlTools.Serialize(FilePath.COMPort_Settings_File, comPortSettingModel);
                        comPortSettingModel = XmlTools.Deserialize<COMPortSettingModel>(FilePath.COMPort_Settings_File, comPortSettingModel);
                        OnUpdate_COMPortSetting(comPortSettingModel);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion
    }
}
