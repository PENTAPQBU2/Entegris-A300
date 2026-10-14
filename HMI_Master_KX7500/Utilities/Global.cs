using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Alarm;

//using HMI_Master_KX7500.Model.DryRun;
using HMI_Master_KX7500.Model.Frame;

//using HMI_Master_KX7500.Model.LotEntry;
using HMI_Master_KX7500.Model.MachineConfig;
using HMI_Master_KX7500.Model.MachineStatus;
using HMI_Master_KX7500.Services;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//using HMI_Master_KX7500.Service.PopUp;
//using HMI_Master_KX7500.Interface;
//using HMI_Master_KX7500.Service.TrackingStatus;
//using HMI_Master_KX7500.Service.Recipe;
//using HMI_Master_KX7500.Service.Main;
//using HMI_Master_KX7500.Service.LotEntry;

namespace HMI_Master_KX7500
{
    internal class Global
    {
        #region PrivateFields

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion PrivateFields

        #region PublicFields

        public static MachineConfigModel MachineConfigModel = new MachineConfigModel();
        public static string MachineName = "A300";

        //public static string OPCUA_Url = "opc.tcp://192.168.3.11:4840";
        public static string OPCUA_Url = "192.168.0.10";
        public static string PLCPort = "8500";

        //Set once MachineConfig.xml has been read (see InitializeMachineConfig)
        public static bool MachineConfigLoaded = false;

        public static bool IsOfflineMode => MachineConfigModel != null && MachineConfigModel.OfflineMode;

        public static string NodeNameSpace = string.Empty; //because last time we test on with plc, with out declare like original nodenamespace, we can still read write, old => NodeNameSpace = "BYD_Welding.PLC";
        public static string HMIVersionNode = $"EM930";
        public static string HMIVersionNodeType = $"STRING[26]";


        public static Dictionary<string, bool> PageList = new Dictionary<string, bool>();
        #endregion PublicFields

        #region Prop

        //public static ComponentLifeCycleInterfaceModel ComponentLifeCycleInterfaceModel { get; set; }

        #endregion Prop
        public static void ActivatePage(string pageName)
        {
            foreach (string key in PageList.Keys.ToList())
            {
                PageList[key] = false;
            }

            PageList[pageName] = true;
        }

        public static void InitializeMachineConfig()
        {
            //try
            //{
            try
            {
                MachineConfigModel = new MachineConfigModel();

                if(!File.Exists(FilePath.MachineConfig))
                {
                    XmlTools.Serialize(FilePath.MachineConfig, MachineConfigModel);
                }

                MachineConfigModel = XmlTools.Deserialize<MachineConfigModel>(FilePath.MachineConfig, MachineConfigModel);

                if(!string.IsNullOrWhiteSpace(MachineConfigModel.PLCIPAddress))
                {
                    OPCUA_Url = MachineConfigModel.PLCIPAddress.Trim();
                }

                if(!string.IsNullOrWhiteSpace(MachineConfigModel.PLCPort))
                {
                    PLCPort = MachineConfigModel.PLCPort.Trim();
                }

                if(MachineConfigModel.OfflineMode)
                {
                    logger.Warn("OfflineMode is ON. PLC, COM port and TCP server are simulated/disabled.");
                }
            }
            catch(Exception ex) { logger.Error(ex); }
            finally
            {
                MachineConfigLoaded = true;
            }

            try
            {
                //Write back so settings added in newer versions appear in an existing MachineConfig.xml
                XmlTools.Serialize(FilePath.MachineConfig, MachineConfigModel);
            }
            catch(Exception ex) { logger.Error(ex); }
            //initializeMachineConfigModel();
            //}
            //catch (Exception ex) { logger.Error(ex); }
        }

        public static Dictionary<string, string> GetList_GeneralNodeList(Dictionary<string, string> MainTagList)
        {
            Dictionary<string, string> CombinedTagList = new Dictionary<string, string>();

            try
            {
                #region Frame

                MachineStatusModel machineStatusModel = new MachineStatusModel();
                if(File.Exists(FilePath.MachineStatus))
                { machineStatusModel = XmlTools.Deserialize<MachineStatusModel>(FilePath.MachineStatus, machineStatusModel); }

                MachineModeModel machineModeModel = new MachineModeModel();
                if(File.Exists(FilePath.MachineMode))
                { machineModeModel = XmlTools.Deserialize<MachineModeModel>(FilePath.MachineMode, machineModeModel); }

                MachineSpeedModel machineSpeedModel = new MachineSpeedModel();
                if(File.Exists(FilePath.MachineSpeed))
                { machineSpeedModel = XmlTools.Deserialize<MachineSpeedModel>(FilePath.MachineSpeed, machineSpeedModel); }

                List<TaktTime_UPH_Model> taktTimeUPHModel = new List<TaktTime_UPH_Model>();
                if(File.Exists(FilePath.TaktTime_UPH))
                { taktTimeUPHModel = XmlTools.Deserialize<List<TaktTime_UPH_Model>>(FilePath.TaktTime_UPH, taktTimeUPHModel); }

                List<ControlPanelModel> controlPanelModel_ = new List<ControlPanelModel>();
                if(File.Exists(FilePath.ControlPanel))
                { controlPanelModel_ = XmlTools.Deserialize<List<ControlPanelModel>>(FilePath.ControlPanel, controlPanelModel_); }

                AlarmNodeModel alarmNodeModel = new AlarmNodeModel();
                if(File.Exists(FilePath.AlarmNode))
                { alarmNodeModel = XmlTools.Deserialize<AlarmNodeModel>(FilePath.AlarmNode, alarmNodeModel); }

                Dictionary<string, string> FrameNodeList = new Dictionary<string, string>();
                FrameNodeList.Add(machineStatusModel.MachineStatusNode, machineStatusModel.MachineStatusType);
                FrameNodeList.Add(machineModeModel.MachineModeNode, machineModeModel.MachineModeType);
                FrameNodeList.Add(machineSpeedModel.MachineSpeedNode, machineSpeedModel.MachineSpeedType);
                taktTimeUPHModel.ForEach(x => FrameNodeList.Add(x.Node, x.NodeType));
                controlPanelModel_.ForEach(x => x.ButtonModelList.ForEach(y => FrameNodeList.Add(y.ButtonNode, y.ButtonNodeType)));
                controlPanelModel_.ForEach(x => x.ToggleModelList.ForEach(y => FrameNodeList.Add(y.ToggleNode, y.ToggleNodeType)));
                FrameNodeList.Add(alarmNodeModel.ErrorNodeString, alarmNodeModel.ErrorNodeTypeString);
                FrameNodeList.Add(alarmNodeModel.WarningNodeString, alarmNodeModel.WarningNodeTypeString);
                FrameNodeList.Add(HMIVersionNode, HMIVersionNodeType);
                foreach(KeyValuePair<string, string> keyValue in MainTagList)
                { FrameNodeList.Add(keyValue.Key, keyValue.Value); }

                #endregion Frame

                #region LotEntry

                //LotEntryModel lotEntryModel = new LotEntryModel();
                //if (File.Exists(FilePath.LotEntry)) { lotEntryModel = XmlTools.Deserialize<LotEntryModel>(FilePath.LotEntry, lotEntryModel); }

                //List<string> lotEntryNodeList = new List<string>();

                //lotEntryNodeList.Add(lotEntryModel.NewLotBitNode);
                //lotEntryNodeList.Add(lotEntryModel.EndLotBitNode);
                //lotEntryNodeList.Add(lotEntryModel.LotIDNode);
                //lotEntryNodeList.Add(lotEntryModel.OperatorIDNode);
                //lotEntryNodeList.Add(lotEntryModel.LotQuantityNode);
                //lotEntryNodeList.Add(lotEntryModel.LotCreatedNode);
                //lotEntryNodeList.Add(lotEntryModel.RecipeNode);

                #endregion LotEntry

                #region ModuleStatus

                //ModuleStatusModel moduleStatusModel = new ModuleStatusModel();
                //if (File.Exists(FilePath.ModuleStatus)) { moduleStatusModel = XmlTools.Deserialize<ModuleStatusModel>(FilePath.ModuleStatus, moduleStatusModel); }
                //List<string> moduleStatusNodeList = new List<string>();
                //moduleStatusNodeList.Add(moduleStatusModel.ModuleStatus_TopVisionStationNode);
                //moduleStatusNodeList.Add(moduleStatusModel.ModuleStatus_WelderStationNode);
                //moduleStatusNodeList.Add(moduleStatusModel.ModuleStatus_LeftStageNode);
                //moduleStatusNodeList.Add(moduleStatusModel.ModuleStatus_RightStageNode);

                #endregion ModuleStatus

                //List<string> clcNodeList = ComponentLifeCycleService.GetComponentLifeCycleTagList();
                //List<string> dryRunNodeList = DryRunService.GetDryRunTagList();
                ////List<string> welderSetupList = WelderSetupService.GetWelderSetupTagList();
                List<string> settingsList = SettingsGeneralService.GetSettingsGeneralTagList();
                //List<string> oeeNodeList = OEEService.GetOEETagList();
                //List<string> machineLayoutNodeList = MachineLayoutService.GetMachineLayoutTagList();
                //List<string> lotEntryNodeList = LotEntryService.GetLotEntryTagList();

                //CombinedTagList.Add("Motor_Page");
                CombinedTagList.Add("MR1300", "BOOL");
                //CombinedTagList.AddRange(FrameNodeList);
                foreach(string key in settingsList)
                {
                    CombinedTagList.Add(key, "BOOL");
                }
                foreach(KeyValuePair<string, string> keyValue in FrameNodeList)
                { CombinedTagList.Add(keyValue.Key, keyValue.Value); }
                //CombinedTagList.AddRange(lotEntryNodeList);
                //CombinedTagList.AddRange(clcNodeList);
                //CombinedTagList.AddRange(dryRunNodeList);
                //CombinedTagList.AddRange(welderSetupList);
                //CombinedTagList.AddRange(settingsList);
                //CombinedTagList.AddRange(oeeNodeList);
                //CombinedTagList.AddRange(machineLayoutNodeList);
                //CombinedTagList.AddRange(lotEntryNodeList);
            }
            catch(Exception ex) { logger.Error(ex); }

            return CombinedTagList;
        }

        public static List<string> GetList_GeneralNodeList2()
        {
            List<string> CombinedTagList2 = new List<string>();

            try
            {
                //List<string> popUpList = PopUpService.GetPopUpTagList();
                //List<string> trackingStatusList = TrackingStatusService.GetTrackingStatusTagList();
                //List<string> cycleTimeNodeList = CycleTimeService.GetCycleTimeTagList();

                //CombinedTagList2.AddRange(popUpList);
                //CombinedTagList2.AddRange(trackingStatusList);
                ////CombinedTagList2.AddRange(recipeList);
                //CombinedTagList2.AddRange(cycleTimeNodeList);
            }
            catch(Exception ex) { logger.Error(ex); }

            return CombinedTagList2;
        }

        public static List<string> GetList_GeneralNodeList3()
        {
            List<string> CombinedTagList3 = new List<string>();

            try
            {
                //List<string> dBCLogList = DBCLogService.GetDBCLogTagList();
                //List<string> welderLogList = WelderLogService.GetWelderLogTag();
                //List<string> popUpList = PopUpService.GetPopUpTagList();

                //CombinedTagList3.AddRange(dBCLogList);
                //CombinedTagList3.AddRange(welderLogList);
                //CombinedTagList3.AddRange(popUpList);
            }
            catch(Exception ex) { logger.Error(ex); }

            return CombinedTagList3;
        }

        public static List<string> GetList_RecipeNode()
        {
            List<string> RecipeTagList = new List<string>();

            try
            {
                //RecipeTagList = RecipeService.GetRecipeTagList();
                //RecipeTagList = RecipeTagList.Union(RecipeService.GetRecipeParameterTagList()).ToList();
            }
            catch(Exception ex) { logger.Error(ex); }

            return RecipeTagList;
        }

        public static List<string> GetIONodeList()
        {
            List<string> ioNodeList = new List<string>();

            try
            {
                //ioNodeList = IOService.GetIOTagList();
            }
            catch(Exception ex) { logger.Error(ex); }

            return ioNodeList;
        }

        public static List<string> GetMotorNodeList()
        {
            List<string> motorNodeList = new List<string>();

            try
            {
                //motorNodeList = MotorService.GetMotorTagList();
            }
            catch(Exception ex) { logger.Error(ex); }

            return motorNodeList;
        }

        private static void initializeMachineConfigModel()
        {
            //consists model for machine config. for debugmode and initialize opc mode
            try
            {
                MachineConfigModel = new MachineConfigModel();

                if(!File.Exists(FilePath.MachineConfig))
                {
                    XmlTools.Serialize(FilePath.MachineConfig, MachineConfigModel);
                }

                MachineConfigModel = XmlTools.Deserialize<MachineConfigModel>(FilePath.MachineConfig, MachineConfigModel);
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        public static bool ConvertBool(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return true;
            }
            else if(value.ToLower().Contains("true"))
            {
                return true;
            }
            else if(bool.TryParse(value, out bool result))
            {
                return result;
            }
            return false;
        }
    }

    //class OpcNameSpace
    //{
    //    public static string GeneralNameSpace = "2:BYD_Welding/2:PLC";
    //    public static string HMIDataTransfer = "2:BYD_Welding/2:PLC/2:DB_HMI_Data_Transfer";
    //    public static string MachineSystem = "2:BYD_Welding/2:PLC/2:DB_Machine_System";
    //    public static string LotOee = "2:BYD_Welding/2:PLC/2:Lot_OEE";
    //    public static string PopUpMsg = "2:BYD_Welding/2:PLC/2:HMI_Pop_Up_Msg/2:PromptMessage";
    //    public static string HMIBypass = "2:BYD_Welding/2:PLC/2:DB_Bypass";
    //    public static string UPHHourly = "2:BYD_Welding/2:PLC/2:DB_UPH_Hourly";
    //}
}