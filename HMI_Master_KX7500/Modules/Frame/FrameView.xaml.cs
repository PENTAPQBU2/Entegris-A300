using CAMLine;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Alarm;
using HMI_Master_KX7500.Model.DryRun;
//using HMI_Master_KX7500.Model.DryRun;
using HMI_Master_KX7500.Model.Frame;
using HMI_Master_KX7500.Model.IO;
//using HMI_Master_KX7500.Model.IO;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.MachineStatus;
using HMI_Master_KX7500.Model.Motor;
using HMI_Master_KX7500.Model.OEE;
//using HMI_Master_KX7500.Model.Motor;
//using HMI_Master_KX7500.Model.PopUp;
using HMI_Master_KX7500.Modules.Alarm;
using HMI_Master_KX7500.Modules.IO;
using HMI_Master_KX7500.Modules.PopUp;
//using HMI_Master_KX7500.Modules.DryRun;
//using HMI_Master_KX7500.Modules.IO;
//using HMI_Master_KX7500.Modules.IOLocation;
//using HMI_Master_KX7500.Modules.PopUp;
//using HMI_Master_KX7500.Modules.Recipe;
//using HMI_Master_KX7500.Modules.TrackingStatus;
using HMI_Master_KX7500.Modules.UserAccount;
using HMI_Master_KX7500.ModulesView.CycleTime;
using HMI_Master_KX7500.ModulesView.EngMode;
using HMI_Master_KX7500.ModulesView.LotEntry;
//using HMI_Master_KX7500.ModulesView.ComponentLifeCycle;
//using HMI_Master_KX7500.ModulesView.CycleTime;
//using HMI_Master_KX7500.ModulesView.EngMode;
//using HMI_Master_KX7500.ModulesView.LotEntry;
//using HMI_Master_KX7500.ModulesView.Main;
using HMI_Master_KX7500.ModulesView.Settings;
using HMI_SSDC_PIM.CommunicationProtocols;
//using HMI_Master_KX7500.ModulesView.PopUp;
//using HMI_Master_KX7500.ModulesView.Recipe;
//using HMI_Master_KX7500.ModulesView.Settings;
//using HMI_Master_KX7500.ModulesView.TrackingStatus;
//using HMI_Master_KX7500.OPCUA;
using NLog;
using NLog.Config;
using NLog.Targets;
using Pentamaster.HMI.Modules.UserAccount;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HMI_Master_KX7500.Modules.Frame
{
    /// <summary>
    /// Interaction logic for FrameView.xaml
    /// </summary>
    public partial class FrameView : Window
    {
        #region PrivateFields

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<MenuModel_> menuModels = null;
        private UserAccountService userAccountService = new UserAccountService();
        private UserAccountSignInOutService userAccountSignInOutService = new UserAccountSignInOutService();
        private KeyenceDll keyenceDll = null;

        //private KeyenceDll keyenceDll2 = null;
        ////private OPCUA_ opcUA_3 = null;
        //private KeyenceDll keyenceDll_Recipe = null;
        //private KeyenceDll keyenceDll_Motor = null;
        private KeyenceDll keyenceDll_IO = null;

        private string Error => "Error";
        private string Warning => "Warning";
        private string SelectedLanguage = string.Empty;
        private bool chinese = false;

        private SetupSettingModel serverSettingModel = new SetupSettingModel();
        //public Entergris_TcpCommService entergris_TcpCommService = new Entergris_TcpCommService();

        private string sideColumnPromptType = string.Empty;

        private SerialListener SL;
        #endregion PrivateFields

        #region PublicFields

        public static bool CycleTimeAccess = false;
        public static string UserNameCycleTime = string.Empty;

        #endregion PublicFields

        #region FrameViewConfig

        public static MachineStatusModel machineStatusModel = new MachineStatusModel();
        public static MachineModeModel machineModeModel = new MachineModeModel();
        public static MachineSpeedModel machineSpeedModel = new MachineSpeedModel();
        public static List<TaktTime_UPH_Model> taktTimeUPHModel = new List<TaktTime_UPH_Model>();
        public static List<ControlPanelModel> controlPanelModel_ = new List<ControlPanelModel>();
        public static AlarmNodeModel alarmNodeModel = new AlarmNodeModel();

        #endregion FrameViewConfig

        #region MenuView

        // Typical stuff due to unacknowledged of YAGNI
        // Bad design
        //private MainViewModel mainViewModel = null;

        //private MainView.MainView mainView = null;
        private MainView.MainView mainView = null;

        private LotEntryView lotEntryView = null;
        private OEEView oeeView = null;
        private AlarmHistoryView alarmhistoryView = null;

        private DryRun.DryRunView dryRunView = null;
        private MotorView motorView = null;
        private HMI_Master_KX7500.Modules.Robot.RobotView robotView = null;

        private EngModeView engModeView = null;
        //EngModeView engModeView = null;
        //ComponentLifeCycleView clcView = null;
        IOView_ ioView = null;
        //IOLocationView ioLocationView = null;
        private CycleTimeView cycleTimeView = null;

        private UserAccountAccessView userAccountAccessView = null;
        ConfigurationView configView = null;
        SettingsView settingsView = null;
        PopUpView popUpView = null;
        //TrackingStatusView trackingStatusView = null;
        //WelderSetupView welderSetupView = null;
        //RecipeView recipeView = null;
        //RecipeEditorView recipeEditorView = null;
        //DBCLogView dBCLogView = null;
        //WelderLogView welderLogView = null;

        #endregion MenuView

        #region Public Static

        public static Dictionary<Tuple<string, string>, string> ErrorListDictEN = new Dictionary<Tuple<string, string>, string>();
        public static Dictionary<Tuple<string, string>, string> ErrorListDictCN = new Dictionary<Tuple<string, string>, string>();

        //public static Dictionary<Tuple<string, string>, string> ErrorListDict = new Dictionary<Tuple<string, string>, string>();
        public static OleDbConnection gl_conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FolderPath.MachineSettings + @"\HMI\MachineSettings\Alarm.mdb");

        #endregion Public Static

        #region Constructor

        public FrameView()
        {
            InitializeComponent();
            try
            {
                //Messenger.Default.Register<_OPC>(this, opcUpdate);
                Messenger.Default.Register<HMIAction>(this, hmiAction);
                Messenger.Default.Register<LanguageModel>(this, updateLanguage);
                Messenger.Default.Register<UserAccountSignInOutServiceModel>(this, initializeUserAccountSignInOutServiceModel);
                Messenger.Default.Register<UserAccountModel>(this, initializeUserAccountModel);
                initializeNonAsync();

                //COMPort 6
                SL = new SerialListener();
                SL.onDataReceived += LoadCellListener;
                if(!Global.IsOfflineMode)
                {
                    SL.Open(true);
                }

                token = source.Token;
                Event.UpdateEvent += Update;
                Task.Run(() =>
                {
                    while(!token.IsCancellationRequested)
                    {
                        //Thread.Sleep(100);
                        Event.RaiseUpdateEvent();

                        //Real PLC reads pace this loop. The simulated PLC answers instantly, so throttle it.
                        if(Global.IsOfflineMode)
                        {
                            Thread.Sleep(100);
                        }
                    }
                });
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token;
        ~FrameView()
        {
            SL.Close();
            source.Cancel();
            Event.UpdateEvent -= Update;
        }

        #endregion Constructor

        #region PrivateMessengerMethods
        string serialData = string.Empty;
        private void LoadCellListener(object sender, EventArgs e)
        {
            try
            {
                #region Comment area 27/3/2026 KWY data issue
                //string data = sender.ToString();
                //if (data != null)
                //{
                //    if (!string.IsNullOrEmpty(data))
                //    {
                //        LogHelper.COMPort(GetType().Name, $"RawData: {data}");
                //        if (!data.Contains('\n'))
                //        {
                //            serialData += data;
                //            //LogHelper.COMPort(GetType().Name, $"Checking Not Contain new line : {serialData}");
                //        }
                //        else
                //        {
                //            var splitData = data.Split('\n');
                //            if (splitData.Length > 0)
                //            {
                //                serialData += splitData[0];
                //                //LogHelper.COMPort(GetType().Name, $"Checking Contain new line (with splitData[0]): {serialData}");
                //                //Dispatcher.Invoke(() => Info(data)); 
                //                if (!string.IsNullOrWhiteSpace(serialData))
                //                {
                //                    Dispatcher.Invoke(() => Info(serialData));
                //                }
                //                serialData = string.Empty;
                //                if (splitData.Length > 1)
                //                {
                //                    serialData += splitData[1];
                //                    //LogHelper.COMPort(GetType().Name, $"Checking Contain new line (with splitData[1]): {serialData}");
                //                }
                //            }
                //        }
                //    }
                //} 
                #endregion
                string data = sender.ToString();
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        LogHelper.COMPort(GetType().Name, $"RawData: {data}");
                        string result = Regex.Replace(data, @"(\r?\n)+", "");
                        if (data.Contains("SN:") && serialData.Contains("SN:"))
                        {
                            LogHelper.COMPort(GetType().Name, $"Return function: data: {data}, stored serial data: {serialData}");
                            return;
                        }
                        else if (serialData.Contains(data))
                        {
                            LogHelper.COMPort(GetType().Name, $"Return function: data: {data}, stored serial data: {serialData}, stored serial data contains data");
                            return;
                        }

                        if (!string.IsNullOrEmpty(result))
                        {
                            serialData += result;
                            if (serialData.EndsWith("end") || serialData.EndsWith("error"))
                            {
                                Dispatcher.Invoke(() => Info(serialData));
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void Info(string message, bool showTime = true)
        {
            try
            {
                LogHelper.COMPort(GetType().Name, message);

                if (message.Contains('\n'))
                {
                    message.Replace('\n', '_');
                }

                var splitData = message.Split(' ');
                if (splitData.Length > 0 && message.Contains("Torque"))
                {
                    string screwSn = string.Empty;
                    string screwResult = string.Empty;
                    string tighteningTime = string.Empty;
                    string tighteningAngle = string.Empty;
                    string torqueVal = string.Empty;
                    string screwDateTime = string.Empty;
                    string screwType = string.Empty;
                    string error = string.Empty;

                    if (splitData[0].ToString().Contains("SN"))
                    {
                        //string[] delimiter = { "SN:" };
                        //var screwSnSplit = splitData[0].ToString().Split(delimiter, StringSplitOptions.None);
                        //screwSn = screwSnSplit[1];
                        screwResult = splitData[3];
                        if (!splitData[3].Contains("N"))
                        {
                            var screwTorqueSplit = splitData[13].Split(':'); //Torque:
                            var screwTypeSplit = splitData[10].Split(':'); //Screw
                            var screwError = splitData[11].Split(':'); //Errors
                            tighteningTime = splitData[8];
                            tighteningAngle = splitData[9];
                            screwType = " " + screwTypeSplit[1];
                            torqueVal = screwTorqueSplit[1];
                            screwDateTime = splitData[1] + " " + splitData[2];
                            error = " " + screwError[1]; 
                        }
                        else
                        {
                            var screwTorqueSplit = splitData[12].Split(':');
                            var screwTypeSplit = splitData[9].Split(':');
                            var screwError = splitData[10].Split(':');
                            tighteningTime = splitData[7];
                            tighteningAngle = splitData[8];
                            screwType = " " + screwTypeSplit[1];
                            torqueVal = screwTorqueSplit[1];
                            screwDateTime = splitData[1] + " " + splitData[2];
                            error = " " + screwError[1];
                        }
                    }
                    else if (splitData[1].ToString().Contains("SN"))
                    {
                        //string[] delimiter = { "SN:" };
                        //var screwSnSplit = splitData[1].ToString().Split(delimiter, StringSplitOptions.None);
                        screwResult = splitData[4];
                        if (!splitData[4].Contains("N"))
                        {
                            var screwTorqueSplit = splitData[14].Split(':');
                            var screwTypeSplit = splitData[11].Split(':');
                            var screwError = splitData[12].Split(':');
                            //screwSn = screwSnSplit[1];
                            tighteningTime = splitData[9];
                            tighteningAngle = splitData[10];
                            screwType = " " + screwTypeSplit[1];
                            torqueVal = screwTorqueSplit[1];
                            screwDateTime = splitData[2] + " " + splitData[3];
                            error = " " + screwError[1]; 
                        }
                        else
                        {
                            var screwTorqueSplit = splitData[13].Split(':');
                            var screwTypeSplit = splitData[10].Split(':');
                            var screwError = splitData[11].Split(':');
                            //screwSn = screwSnSplit[1];
                            tighteningTime = splitData[8];
                            tighteningAngle = splitData[9];
                            screwType = " " + screwTypeSplit[1];
                            torqueVal = screwTorqueSplit[1];
                            screwDateTime = splitData[2] + " " + splitData[3];
                            error = " " + screwError[1];
                        }
                    }
                    //else if (splitData[2].ToString().Contains("SN"))
                    //{
                    //    //string[] delimiter = { "SN:" };
                    //    //var screwSnSplit = splitData[2].ToString().Split(delimiter, StringSplitOptions.None);
                    //    var screwTorqueSplit = splitData[12].Split(':');
                    //    var screwTypeSplit = splitData[9].Split(':');
                    //    var screwError = splitData[10].Split(':');
                    //    //screwSn = screwSnSplit[1];
                    //    screwResult = splitData[5];
                    //    tighteningTime = splitData[7];
                    //    tighteningAngle = splitData[8];
                    //    screwType = " " + screwTypeSplit[1];
                    //    torqueVal = screwTorqueSplit[1];
                    //    screwDateTime = splitData[3] + " " + splitData[4];
                    //    error = " " + screwError[1];
                    //}

                    //foup csv
#if DEBUG
                    string foupId = "FOUPTESTABC123";
                    string lotId = "LotTest123";
                    string folderPath = getFoupCsvFolder(@"D:\HMI_A300\FOUP CSV\");
                    int screwNum = 1234;
#else
                    string foupId = keyenceDll.Read<string>(PLCTag.Foup_Barcode, 20);
                    string lotId = keyenceDll.Read<string>(PLCTag.Lot_ID);
                    int screwNum = keyenceDll.Read<int>(PLCTag.Screw_Number);
                    string folderPath = getFoupCsvFolder(@"D:\HMI\FOUP CSV\");
#endif
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string dateStr = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    string folderPathWithDate = Path.Combine(folderPath, dateStr);

                    if (!Directory.Exists(folderPathWithDate))
                    {
                        Directory.CreateDirectory(folderPathWithDate);
                    }

                    string filename = Path.Combine(folderPathWithDate, $"{foupId}_{lotId}_{dateStr}.csv");
                    bool fileExist = File.Exists(filename);

                    if (!fileExist)
                    {
                        using (FileStream stream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
                        {
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                //writer.WriteLine("DateTime,FoupId,LotId,ScrewNumber,ScrewType,TorqueValue,TighteningAngle,TighteningTime");
                                ////writer.WriteLine($"{DateTime.Now},{foupId},{lotId},{screwSn},{screwType},{torqueVal},{tighteningAngle},{tighteningTime}");
                                //writer.WriteLine($"{screwDateTime},{foupId},{lotId},{screwSn},{screwType},{torqueVal},{tighteningAngle},{tighteningTime}");
                                //writer.WriteLine("DateTime,FoupId,LotId,ScrewNumber,ScrewDateTime,ScrewType,TorqueValue,TighteningAngle,TighteningTime");
                                //writer.WriteLine($"{DateTime.Now},{foupId},{lotId},{screwSn},{screwDateTime},{screwType},{torqueVal},{tighteningAngle},{tighteningTime}");
                                writer.WriteLine("DateTime,FoupId,LotId,ScrewNumber,ScrewDateTime,Result,ScrewType,Error,TorqueValue,TighteningAngle,TighteningTime");
                                writer.WriteLine($"{DateTime.Now},{foupId},{lotId},{screwNum},{screwDateTime},{screwResult},{screwType},{error},{torqueVal},{tighteningAngle},{tighteningTime}");
                                writer.Flush();
                                writer.Close();
                                stream.Close();
                            }
                        }
                    }
                    else
                    {
                        using (FileStream stream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                        {
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                ////writer.WriteLine($"{DateTime.Now},{foupId},{lotId},{screwSn},{screwType},{torqueVal},{tighteningAngle},{tighteningTime}");
                                //writer.WriteLine($"{screwDateTime},{foupId},{lotId},{screwSn},{screwType},{torqueVal},{tighteningAngle},{tighteningTime}");
                                //writer.WriteLine($"{DateTime.Now},{foupId},{lotId},{screwSn},{screwDateTime},{screwType},{torqueVal},{tighteningAngle},{tighteningTime}");
                                writer.WriteLine($"{DateTime.Now},{foupId},{lotId},{screwNum},{screwDateTime},{screwResult},{screwType},{error},{torqueVal},{tighteningAngle},{tighteningTime}");
                                writer.Flush();
                                writer.Close();
                                stream.Close();
                            }
                        }
                    }
                    serialData = string.Empty;
                }

            }
            catch (Exception ex) 
            {
                LogHelper.Error(GetType().Name, ex);
            }
        }

        //Configured folder, else the legacy D: folder, else "<HMI folder>\FOUP CSV\" when that drive does not exist
        private string getFoupCsvFolder(string legacyFolder)
        {
            string configured = Global.MachineConfigModel?.FoupCsvFolder;
            if(!string.IsNullOrWhiteSpace(configured))
            {
                return configured.Trim().TrimEnd('\\') + @"\";
            }

            if(Directory.Exists(Path.GetPathRoot(legacyFolder)))
            {
                return legacyFolder;
            }

            string hmiFolder = Directory.GetParent(FolderPath.Logs.TrimEnd('\\')).FullName;
            return Path.Combine(hmiFolder, "FOUP CSV") + @"\";
        }

        private void Update()
        {
            try
            {
                if (keyenceDll != null)
                {
                    //var sscsEnable = keyenceDll.Read<bool>("MR1504");
                    //var cscEnable = keyenceDll.Read<bool>("MR1511");
                    //var sscsText = keyenceDll.Read<string>("EM3700", 100);
                    //var cscText = keyenceDll.Read<string>("EM3340", 100);
                    var machineStatus = keyenceDll.Read<short>(PLCTag.MachineStatus).ToString();
                    var machineMode = keyenceDll.Read<short>(PLCTag.MachineMode).ToString();
                    var gaugeMachineUPH = keyenceDll.Read<int>(PLCTag.ProductionUPH);
                    var errorValue = keyenceDll.Read<string>(PLCTag.ErrorString, 256);
                    var warningValue = keyenceDll.Read<string>(PLCTag.WarningString, 256);
                    Dispatcher.Invoke(() =>
                    {
                        //if(sscsEnable || cscEnable)
                        //{
                        //    SideColumnStackPanel.Visibility = Visibility.Visible;
                        //    if (sscsEnable)
                        //    {
                        //        SideColumnTextBox.Text = sscsText;
                        //        sideColumnPromptType = "StartSideColumn";
                        //    }
                        //    else
                        //    {
                        //        SideColumnTextBox.Text = cscText;
                        //        sideColumnPromptType = "ClampSideColumn";
                        //    }
                        //}
                        //else
                        //{
                        //    SideColumnStackPanel.Visibility = Visibility.Collapsed;
                        //}

                        updateMachineStatus(machineStatus);
                        updateMachineMode(machineMode);
                        GaugeMachineUPH.Value = gaugeMachineUPH;

                        TextBlockWorkOrderID.Text = keyenceDll.Read<string>("EM1501", 50);
                        TextBlockRecipe.Text = keyenceDll.Read<int>("EM1559").ToString();
                        //TextBlock12mmScrewAmount.Text = keyenceDll.Read<short>("EM20201").ToString();
                        //TextBlock22mmScrewAmount.Text = keyenceDll.Read<short>("EM20200").ToString();

                        updateAlarm(PLCTag.ErrorString, errorValue);
                        updateAlarm(PLCTag.WarningString, warningValue);
                    }); 
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        //private void opcUpdate(_OPC opcUA)
        //{
        //    try
        //    {
        //        if(opcUA.Module.Contains(OPCModules.Frame))
        //        {
        //            Task.Run(() =>
        //            {
        //                Dispatcher.Invoke(new Action(() =>
        //                {
        //                    opcUA.DataChange.ToList().ForEach(x =>
        //                    {
        //                        if(x.Key == machineStatusModel.MachineStatusNode)
        //                        {
        //                            updateMachineStatus(x.Value);
        //                        }
        //                        else if(x.Key == machineModeModel.MachineModeNode)
        //                        {
        //                            updateMachineMode(x.Value);
        //                        }
        //                        else if(x.Key == machineSpeedModel.MachineSpeedNode)
        //                        {
        //                            GaugeMachineSpeed.Value = Convert.ToDouble(x.Value);
        //                        }
        //                        else if(taktTimeUPHModel.Exists(y => y.Node == x.Key))
        //                        {
        //                            taktTimeUPHModel.Find(y => y.Node == x.Key).NodeValue = x.Value;
        //                        }
        //                        else if(controlPanelModel_.Any(y => y.ToggleModelList.Exists(z => z.ToggleNode == x.Key)))
        //                        {
        //                            controlPanelModel_.Any(y => y.ToggleModelList.Find(z => z.ToggleNode == x.Key).ToggleValue = Convert.ToBoolean(x.Value));
        //                        }
        //                        else if(x.Key == alarmNodeModel.ErrorNodeString || x.Key == alarmNodeModel.WarningNodeString)
        //                        {
        //                            updateAlarm(x.Key, x.Value);
        //                        }
        //                    });
        //                }));
        //            });
        //        }
        //    }
        //    catch(Exception ex) { logger.Error(ex); }
        //}

        private void InitializeKX7500()
        {
            try
            {
                keyenceDll = new KeyenceDll();
                //keyenceDll2 = new KeyenceDll();
                ////opcUA_3 = new OPCUA_();
                //keyenceDll_Recipe = new KeyenceDll();
                //keyenceDll_Motor = new KeyenceDll();
                //keyenceDll_IO = new KeyenceDll();

                List<OPCModules> modules1 = new List<OPCModules>() { OPCModules.Frame, OPCModules.Settings, OPCModules.DryRun,
                    OPCModules.OEE, OPCModules.WelderSetup, OPCModules.ComponentLifeCycle, OPCModules.LotEntry, OPCModules.LFLegSettings,
                    OPCModules.LFLegDrawingPos, OPCModules.MachineLayout,OPCModules.Main , OPCModules.Engineering  };

                List<OPCModules> modules2 = new List<OPCModules>() { OPCModules.VisionSetup, OPCModules.CycleTime, OPCModules.TrackingStatus,
                    OPCModules.PopUp };

                //List<OPCModules> modules3 = new List<OPCModules>() { OPCModules.DBCLog, OPCModules.WelderLog };

                List<OPCModules> recipemodule = new List<OPCModules>() { OPCModules.Recipe, OPCModules.RecipeEditor };

                //if true, initialize
                if(Global.MachineConfigModel.InitializeKX7500)
                {
                    ///initialize the address
                    ///List<OPCModules> modules = new List<OPCModules>() { OPCModules.Frame, OPCModules.Settings, OPCModules.DryRun, OPCModules.PopUp,
                    ///    OPCModules.OEE, OPCModules.Motor, OPCModules.IO, OPCModules.Engineering, OPCModules.CycleTime, OPCModules.ComponentLifeCycle,
                    ///    OPCModules.VisionSetup, OPCModules.LFLegSettings, OPCModules.LFLegDrawingPos, OPCModules.ModuleStatus, OPCModules.TrackingStatus,
                    ///    OPCModules.WelderSetup, OPCModules.Recipe, OPCModules.DBCLog, OPCModules.WelderLog };
                    ///opcUA.InitializeUAOPC(Global.OPCUA_Url,Global.GetList_GeneralNodeList(), modules);
                    
                    //keyenceDll = new KeyenceDll(Global.GetList_GeneralNodeList(mainViewModel.GetTagList()), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, modules1);
                    keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, modules1);

                    //keyenceDll2.InitializeUAOPC(Global.OPCUA_Url, Global.GetList_GeneralNodeList2(), modules2);
                    //opcUA_3.InitializeUAOPC(Global.OPCUA_Url, Global.GetList_GeneralNodeList3(), modules3);
                    //keyenceDll_Recipe.InitializeUAOPC(Global.OPCUA_Url, Global.GetList_RecipeNode(), recipemodule);
                    //keyenceDll_Motor.InitializeUAOPC(Global.OPCUA_Url, Global.GetMotorNodeList(), new List<OPCModules> { OPCModules.Motor });
                    //keyenceDll_IO.InitializeUAOPC(Global.OPCUA_Url, Global.GetIONodeList(), new List<OPCModules> { OPCModules.IO });

                    Messenger.Default.Send(keyenceDll);
                    //Messenger.Default.Send(keyenceDll2);
                    ////Messenger.Default.Send(opcUA_3);
                    //Messenger.Default.Send(keyenceDll_Recipe);
                    //Messenger.Default.Send(keyenceDll_Motor);
                    Messenger.Default.Send(keyenceDll_IO);
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void updateLanguage(LanguageModel obj = null)
        {
            #region Control Panel

            controlPanelModel_.SelectMany(z => z.ButtonModelList).ToList().ForEach(z => z.ButtonName = HmiHelper.GetResource<string>(z.ButtonKey));

            controlPanelModel_.SelectMany(z => z.ToggleModelList).ToList().ForEach(z => z.ToggleName = HmiHelper.GetResource<string>(z.ToggleKey));

            #endregion Control Panel

            #region UPH

            taktTimeUPHModel.ForEach(z => z.TaktUPH_Name = HmiHelper.GetResource<string>(z.TaktUPH_Key));

            #endregion UPH

            //#region ProgramVersion
            //ProgramVersionModels.ForEach(z => z.ProgramName = HmiHelper.GetResource<string>(z.ProgramKey));
            //#endregion

            #region Menu

            try
            {
                if(menuModels != null)
                {
                    menuModels.ForEach(z =>
                    {
                        z.MenuName = HmiHelper.GetResource<string>(z.MenuKey);
                    });
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }

            #endregion Menu

            if(obj != null)
            {
                if(obj.LanguageKey.ToUpper().Contains("CHINESE"))
                {
                    chinese = true;
                }
                else
                {
                    chinese = false;
                }
            }
        }

        #endregion PrivateMessengerMethods

        #region PrivateInitializeFrame

        /*create the config file for control if
         * the file is not exist*/

        private void initializeFrame()
        {
            try
            {
                initializeErrorWarning();
                initializeMachineStatus();
                initializeMachineMode();
                //initializeMachineSpeed();
                //initializeTaktUPH();
                initializeControlPanel();
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void initializeErrorWarning()
        {
            try
            {
                alarmNodeModel = new AlarmNodeModel();

                if(!File.Exists(FilePath.AlarmNode))
                {
                    alarmNodeModel.ErrorNodeString = $"EM201";
                    alarmNodeModel.WarningNodeString = $"EM480";
                    alarmNodeModel.ErrorNodeTypeString = $"STRING[256]";
                    alarmNodeModel.WarningNodeTypeString = $"STRING[256]";

                    XmlTools.Serialize(FilePath.AlarmNode, alarmNodeModel);
                }

                alarmNodeModel = XmlTools.Deserialize<AlarmNodeModel>(FilePath.AlarmNode, alarmNodeModel);
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void initializeMachineStatus()
        {
            try
            {
                machineStatusModel = new MachineStatusModel();

                if(!File.Exists(FilePath.MachineStatus))
                {
                    machineStatusModel.MachineStatusNode = $"EM103";
                    machineStatusModel.MachineStatusType = $"INT";

                    XmlTools.Serialize(FilePath.MachineStatus, machineStatusModel);
                }

                machineStatusModel = XmlTools.Deserialize<MachineStatusModel>(FilePath.MachineStatus, machineStatusModel);
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void initializeMachineMode()
        {
            try
            {
                machineModeModel = new MachineModeModel();

                if(!File.Exists(FilePath.MachineMode))
                {
                    machineModeModel.MachineModeNode = $"EM104";
                    machineModeModel.MachineModeType = $"INT";

                    XmlTools.Serialize(FilePath.MachineMode, machineModeModel);
                }

                machineModeModel = XmlTools.Deserialize<MachineModeModel>(FilePath.MachineMode, machineModeModel);
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        //private void initializeMachineSpeed()
        //{
        //    try
        //    {
        //        machineSpeedModel = new MachineSpeedModel();

        //        if (!File.Exists(FilePath.MachineSpeed))
        //        {
        //            machineSpeedModel.MachineSpeedNode = $"{Global.NodeNameSpace}DB_Machine_System.Speed";

        //            XmlTools.Serialize(FilePath.MachineSpeed, machineSpeedModel);
        //        }

        //        machineSpeedModel = XmlTools.Deserialize<MachineSpeedModel>(FilePath.MachineSpeed, machineSpeedModel);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}

        //private void initializeTaktUPH()
        //{
        //    try
        //    {
        //        taktTimeUPHModel = new List<TaktTime_UPH_Model>();

        //        if(!File.Exists(FilePath.TaktTime_UPH))
        //        {
        //            taktTimeUPHModel.Add(new TaktTime_UPH_Model
        //            {
        //                TaktUPH_Key = "CurrentUPH",
        //                Node = $"EM1254",
        //                NodeType = "DINT"
        //            });

        //            //taktTimeUPHModel.Add(new TaktTime_UPH_Model
        //            //{
        //            //    TaktUPH_Key = "TaktTime",
        //            //    Node = $"{Global.NodeNameSpace}DB_UPH_Hourly.Takt_Time"
        //            //});

        //            taktTimeUPHModel.Add(new TaktTime_UPH_Model
        //            {
        //                TaktUPH_Key = "Yield",
        //                Node = $"EM1260",
        //                NodeType = "REAL"
        //            });

        //            //there's no sprint. gonna put it 1st. just in case
        //            //taktTimeUPHModel.Add(new TaktTime_UPH_Model
        //            //{
        //            //    TaktUPH_Key = "Sprint UPH",
        //            //    Node = ""
        //            //});
        //            XmlTools.Serialize(FilePath.TaktTime_UPH, taktTimeUPHModel);
        //        }

        //        taktTimeUPHModel = XmlTools.Deserialize<List<TaktTime_UPH_Model>>(FilePath.TaktTime_UPH, taktTimeUPHModel);
        //        ItemsControlTaktUPH.ItemsSource = taktTimeUPHModel;
        //    }
        //    catch(Exception ex) { logger.Error(ex); }
        //}

        private void initializeControlPanel()
        {
            try
            {
                if(!File.Exists(FilePath.ControlPanel))
                {
                    controlPanelModel_.Add(new ControlPanelModel
                    {
                        ButtonModelList = new List<ButtonModel>()
                        {
                            new ButtonModel()
                            {
                                ButtonKey ="Start",
                                ButtonNode = $"MR000",
                                ButtonNodeType = "BOOL",
                                ButtonImage = "pack://application:,,,/Pentamaster;component/HMI/Icons/Play.png",
                            },
                            new ButtonModel()
                            {
                                ButtonKey ="Stop",
                                ButtonNode = $"MR001",
                                ButtonNodeType = "BOOL",
                                ButtonImage = "pack://application:,,,/Pentamaster;component/HMI/Icons/Stop.png",
                            },
                            new ButtonModel()
                            {
                                ButtonKey = "Reset",
                                ButtonNode = $"MR002",
                                ButtonNodeType = "BOOL",
                                ButtonImage = "pack://application:,,,/Pentamaster;component/HMI/Icons/Reload.png",
                            },
                             //new ButtonModel()
                             //{
                             //    ButtonKey = "Initialize",
                             //    ButtonNode = $"{Global.NodeNameSpace}DB_HMI_Data_Transfer.HMI_Init",
                             //    ButtonImage = "pack://application:,,,/Pentamaster;component/HMI/Icons/Refresh.png",
                             //}
                        },
                        ToggleModelList = new List<ToggleModel>()
                        {
                            new ToggleModel()
                            {
                                ToggleKey = "Engineering Mode",
                                ToggleNode = $"MR1000",
                                ToggleNodeType = "BOOL",
                                ToggleImage = "pack://application:,,,/Pentamaster;component/HMI/Icons/Wrench.png",
                            }
                        }
                    });

                    XmlTools.Serialize(FilePath.ControlPanel, controlPanelModel_);
                }

                controlPanelModel_ = XmlTools.Deserialize<List<ControlPanelModel>>(FilePath.ControlPanel, controlPanelModel_);
                ItemsControl_ControlPanel.ItemsSource = controlPanelModel_;
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        #endregion PrivateInitializeFrame

        #region PrivateInitializeMethods

        private void initializeNonAsync()
        {
            try
            {
                LoadSettingService.OnUpdate_ServerSetting += new LoadSettingService.UpdateServerSetting(LoadServerSetting);
                LoadSettingService.LoadServerSetting();
                Entergris_TcpCommService entergris_TcpCommService = new Entergris_TcpCommService();

                initializeFolderPath();
                initializeLog();
                initializeFilePath();
                Global.InitializeMachineConfig(); //to create setting file for debug mode and initializeOPC
                initializeFrame();
                initializeMenu();
                initializeLanguage();
                initializeAlarmContent();
                InitializeKX7500();
                TextBlockAssemblyVersion.Text = $"V{Assembly.GetExecutingAssembly().GetName().Version}";
                if(Global.IsOfflineMode)
                {
                    TextBlockAssemblyVersion.Text += " [OFFLINE]";
                }
                //opcUA.Write(Global.HMIVersionNode, TextBlockAssemblyVersion.Text);
                //Messenger.Default.Send(opcUA);
                //opcUA.ReadAllAddresses();
                userAccountService.SetUserAccountFilePath(FilePath.UserAccountSetting);
                userAccountSignInOutService.SignedInAsOperatorLevel();

                //LoadSettingService.OnUpdate_ServerSetting += new LoadSettingService.UpdateServerSetting(LoadServerSetting);
                //LoadSettingService.LoadServerSetting();
                //Entergris_TcpCommService entergris_TcpCommService = new Entergris_TcpCommService();
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void LoadServerSetting(SetupSettingModel _serverSettingModel)
        {
            this.serverSettingModel = _serverSettingModel;
        }

        private void initializeLog()
        {
            LoggingConfiguration loggingConfiguration
                 = new LoggingConfiguration();

            #region FileTargets

            FileTarget fileTargetLog = new FileTarget
            {
                FileName = $"{FolderPath.Logs}" +
                $@"{DateTime.Today.ToString("yyyyMMdd")}\" +
                $"Log_{DateTime.Today.ToString("yyyyMMdd")}_" +
                $"{Global.MachineName}.txt"
            };
            loggingConfiguration.AddTarget("Log", fileTargetLog);

            FileTarget fileTargetError = new FileTarget
            {
                FileName = $"{FolderPath.Logs}" +
                $@"{DateTime.Today.ToString("yyyyMMdd")}\" +
                $"Error_{DateTime.Today.ToString("yyyyMMdd")}_" +
                $"{Global.MachineName}.txt"
            };
            loggingConfiguration.AddTarget("Error", fileTargetError);

            FileTarget fileTargetOPC = new FileTarget
            {
                FileName = $"{FolderPath.Logs}" +
                $@"{DateTime.Today.ToString("yyyyMMdd")}\" +
                $"OPC_{DateTime.Today.ToString("yyyyMMdd")}_" +
                $"{Global.MachineName}.txt"
            };
            loggingConfiguration.AddTarget("OPC", fileTargetOPC);

            //
            FileTarget fileTargetCOMPort = new FileTarget
            {
                FileName = $"{FolderPath.Logs}" +
                $@"{DateTime.Today.ToString("yyyyMMdd")}\" +
                $"COMPort_{DateTime.Today.ToString("yyyyMMdd")}_" +
                $"{Global.MachineName}.txt"
            };
            loggingConfiguration.AddTarget("COMPort", fileTargetCOMPort);

            #endregion FileTargets

            #region LoggingRule

            LoggingRule loggingRuleLog = new LoggingRule("*",
                LogLevel.Debug, LogLevel.Fatal, fileTargetLog);
            loggingConfiguration.LoggingRules.Add(loggingRuleLog);

            LoggingRule loggingRuleError = new LoggingRule("*",
                LogLevel.Error, LogLevel.Error, fileTargetError);
            loggingConfiguration.LoggingRules.Add(loggingRuleError);

            LoggingRule loggingRuleOPC = new LoggingRule(
                LogTools.LoggerOPC, LogLevel.Trace,
                LogLevel.Trace, fileTargetOPC);
            loggingConfiguration.LoggingRules.Add(loggingRuleOPC);

            //
            LoggingRule loggingRuleCOMPort = new LoggingRule("*",
                LogLevel.Info, LogLevel.Info, fileTargetCOMPort);
            loggingConfiguration.LoggingRules.Add(loggingRuleCOMPort);

            #endregion LoggingRule

            LogManager.Configuration = loggingConfiguration;
        }

        private void initializeFolderPath()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                Assembly.GetExecutingAssembly().Location);
            FolderPath folderPath = new FolderPath();
#if DEBUG
            folderPath.GetType().GetFields().ToList().ForEach(x =>
            {
                x.SetValue(folderPath, $@"{directoryInfo.Root}HMI_A300\{x.Name}\");
                if (!Directory.Exists(x.GetValue(folderPath).ToString()))
                {
                    Directory.CreateDirectory(x.GetValue(
                        folderPath).ToString());
                }
            });
#else
            folderPath.GetType().GetFields().ToList().ForEach(x =>
            {
                x.SetValue(folderPath, $@"{directoryInfo.Root}HMI\{x.Name}\");
                if(!Directory.Exists(x.GetValue(folderPath).ToString()))
                {
                    Directory.CreateDirectory(x.GetValue(
                        folderPath).ToString());
                }
            });
#endif
        }

        private void initializeFilePath()
        {
            FilePath filePath = new FilePath();
            filePath.GetType().GetFields().ToList().ForEach(x =>
            {
                switch(x.Name)
                {
                    case nameof(FilePath.UserAccountSetting):
                        x.SetValue(filePath, $"{FolderPath.MachineSettings}" +
                        $"{x.Name}.mdb");
                        break;

                    case nameof(FilePath.MachineConfig):
                        x.SetValue(filePath, $"{FolderPath.MachineSettings}" +
                        $"{x.Name}.xml");
                        break;

                    case nameof(FilePath.Alarm):
                        x.SetValue(filePath, $"{FolderPath.MachineSettings}" +
                        $"{x.Name}.mdb");
                        break;

                    default:
                        x.SetValue(filePath, $"{FolderPath.MachineConfiguration}" +
                       $"{x.Name}.xml");
                        break;
                }
            });
        }

        private void initializeUserAccountModel(UserAccountModel userAccountModel)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TextBlockUserAccountUserLevel.Text = userAccountModel.UserAccountUserLevel.ToString();

                    #region MenuPageAccess

                    bool addDryRunView = true;
                    bool addSettingView = true;
                    bool addMotorView = true;
                    //bool addVisionSetupView = true;
                    //bool addPinToggleHoles = true;
                    //bool addMagazineView = true;
                    //bool addClcView = true;
                    bool addIoView = true;
                    //bool addIoLocationView = true;
                    bool addEngModeView = true;
                    bool addCycleTimeView = true;
                    bool addUserAccessView = true;
                    bool addConfigView = true;
                    //bool addTrackingStatus = true;
                    //bool addRecipe = true;
                    //bool addRecipeEditor = true;

                    #endregion MenuPageAccess

                    switch(userAccountModel.UserAccountUserLevel)
                    {
                        case UserAccountUserLevel.Operator:
                            addDryRunView = false;
                            addSettingView = false;
                            addMotorView = false;
                            //addVisionSetupView = false;
                            //addPinToggleHoles = false;
                            //addMagazineView = false;
                            //addClcView = false;
                            addIoView = false;
                            //addIoLocationView = false;
                            addEngModeView = false;
                            addCycleTimeView = false;
                            addUserAccessView = false;
                            addConfigView = false;
                            addSettingView = false;
                            //addTrackingStatus = false;
                            //addRecipe = false;
                            //addRecipeEditor = false;
                            break;

                        case UserAccountUserLevel.Technician:
                            //addWelderLog = false;
                            break;

                        //case UserAccountUserLevel.Engineer:
                        //    //addWelderLog = false;
                        //    break;

                        case UserAccountUserLevel.Pentamaster:
                            //addWelderLog = false;
                            addConfigView = false;
                            break;

                        default:
                            //addWelderLog = false;
                            break;
                    }

                    bool debugMode = Global.MachineConfigModel.BypassSignIn;
                    CycleTimeAccess = addCycleTimeView;
                    UserNameCycleTime = CycleTimeAccess ? userAccountModel.UserAccountUserLevel.ToString() : string.Empty;

                    #region PageMenu

                    menuModels = new List<MenuModel_>();
                    addMenuModel(mainView, "pack://application:,,,/HMI_SSDC_PIM;component/Icon/Home.png", "Main");
                    addMenuModel(lotEntryView, "pack://application:,,,/Pentamaster;component/HMI/Icons/Task.png", "Lot Entry");
                    addMenuModel(oeeView, "pack://application:,,,/Pentamaster;component/HMI/Icons/TimeFrame.png", "OEE");
                    addMenuModel(alarmhistoryView, "pack://application:,,,/Pentamaster;component/HMI/Icons/Alarm.png", "Alarm History");
                    if (debugMode || addDryRunView) { addMenuModel(dryRunView, "pack://application:,,,/Pentamaster;component/HMI/Icons/Stopwatch.png", "Dry Run"); }
                    if (debugMode || addSettingView) { addMenuModel(settingsView, "pack://application:,,,/Pentamaster;component/HMI/Icons/Gear.png", "Setting"); }
                    if (debugMode || addMotorView) { addMenuModel(motorView, "pack://application:,,,/Pentamaster;component/HMI/Icons/Engineering.png", "Motor"); }
                    //Robot page: same access as Motor (Technician and Pentamaster, not Operator)
                    if (debugMode || addMotorView) { addMenuModel(robotView, "pack://application:,,,/HMI_SSDC_PIM;component/Icon/Wrench.png", "Robot"); }
                    if (debugMode || addIoView) { addMenuModel(ioView, "pack://application:,,,/HMI_SSDC_PIM;component/Icon/AttendanceList.png", "IO"); }
                    if (debugMode || addCycleTimeView) { addMenuModel(cycleTimeView, "pack://application:,,,/Pentamaster;component/HMI/Icons/TimeCycles.png", "Cycle Time"); }
                    if (debugMode || addUserAccessView) { addMenuModel(userAccountAccessView, "pack://application:,,,/HMI_SSDC_PIM;component/Icon/User.png", "User Access"); }
                    if (debugMode || addConfigView) { addMenuModel(configView, "pack://application:,,,/Pentamaster;component/HMI/Icons/Equipment.png", "Configuration"); }
                    if (debugMode || addEngModeView) { addMenuModel(engModeView, "pack://application:,,,/Pentamaster;component/HMI/Icons/ElectricEngineer.png", "Engineering Mode"); }

                    #endregion PageMenu

                    ListBoxMenu.ItemsSource = menuModels;
                    updateLanguage();
                    ListBoxMenu.SelectedIndex = 0; //MainPage
                }));
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void initializeAlarmContent()
        {
            try
            {
                lock(ErrorListDictEN)
                {
                    DataSet ds = new DataSet();
                    ds.Clear();
                    ErrorListDictEN.Clear();
                    if(gl_conn.State.Equals(ConnectionState.Open))
                    {
                        gl_conn.Close();
                    }
                    //gl_conn is created before FolderPath is set, so point it at MachineSettings\Alarm.mdb here
                    gl_conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath.Alarm;
                    gl_conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM AlarmV1Model", gl_conn);
                    OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
                    adapter.Fill(ds, "AlarmList");
                    foreach(DataRow row in ds.Tables["AlarmList"].Rows)
                    {
                        ErrorListDictEN.Add(Tuple.Create(row["AlarmCode"].ToString(), row["AlarmType"].ToString()), row["AlarmMessageEnglish"] + ";" + row["AlarmActionEnglish"] + ";" + row["AlarmModule"]);
                    }
                    ds.Clear();
                    gl_conn.Close();
                }

                lock(ErrorListDictCN)
                {
                    DataSet ds = new DataSet();
                    ds.Clear();
                    ErrorListDictCN.Clear();
                    if(gl_conn.State.Equals(ConnectionState.Open))
                    {
                        gl_conn.Close();
                    }
                    //gl_conn is created before FolderPath is set, so point it at MachineSettings\Alarm.mdb here
                    gl_conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath.Alarm;
                    gl_conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM AlarmV1Model", gl_conn);
                    OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
                    adapter.Fill(ds, "AlarmList");
                    foreach(DataRow row in ds.Tables["AlarmList"].Rows)
                    {
                        ErrorListDictCN.Add(Tuple.Create(row["AlarmCode"].ToString(), row["AlarmType"].ToString()), row["AlarmMessageChinese"] + ";" + row["AlarmActionChinese"] + ";" + row["AlarmModule"]);
                    }
                    ds.Clear();
                    gl_conn.Close();
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void initializeUserAccountSignInOutServiceModel(UserAccountSignInOutServiceModel userAccountSignInOutServiceModel)
        {
            try
            {
                if(userAccountSignInOutServiceModel.IsUserAccountSignIn)
                {
                    UserAccountSignInView userAccountSignInView = new UserAccountSignInView { DataContext = new UserAccountSignInViewModel() };
                    userAccountService.SetUserAccountFilePath(FilePath.UserAccountSetting);
                    GridUserAccountSignInOut.Children.Clear();
                    GridUserAccountSignInOut.Children.Add(userAccountSignInView);
                    setDialogHostPromptIsOpen(true, false);
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void initializeMenu()
        {
            try
            {
                mainView = new MainView.MainView();
                lotEntryView = new LotEntryView();
                oeeView = new OEEView();
                alarmhistoryView = new AlarmHistoryView();
                dryRunView = new DryRun.DryRunView();
                motorView = new MotorView { DataContext = new Motor_OPCUA_ViewModel() };
                robotView = new HMI_Master_KX7500.Modules.Robot.RobotView { DataContext = new HMI_Master_KX7500.ModulesView.Robot.RobotViewModel() };
                ioView = new IOView_ { DataContext = new IO_OPCUA_ViewModel() };
                cycleTimeView = new CycleTimeView { DataContext = new CycleTime_OPCUA_ViewModel() };
                engModeView = new EngModeView { DataContext = new EngMode_OPCUA_ViewModel() };
                userAccountAccessView = new UserAccountAccessView();
                settingsView = new SettingsView { DataContext = new Settings_OPCUA_ViewModel() };
                configView = new ConfigurationView();
                popUpView = new PopUpView();

                Global.PageList.Add("Main", false);
                Global.PageList.Add("Lot Entry", false);
                Global.PageList.Add("OEE", false);
                Global.PageList.Add("Alarm History", false);
                Global.PageList.Add("Dry Run", false);
                Global.PageList.Add("Motor", false);
                Global.PageList.Add(HMI_Master_KX7500.ModulesView.Robot.RobotViewModel.PageName, false);
                Global.PageList.Add("IO", false);
                Global.PageList.Add("Cycle Time", false);
                Global.PageList.Add("Engineering Mode", false);
                //Global.PageList.Add("User Access", false);
                Global.PageList.Add("Setting", false);
                //Global.PageList.Add("Configuration", false);
                //PopUp
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void initializeLanguage()
        {
            Messenger.Default.Send(new LanguageInitializeModel
            {
                LanguageFilePath = FilePath.Language,
                LanguageFolderPath = FolderPath.LanguageFolderPath
            });
        }

        #endregion PrivateInitializeMethods

        #region PrivateEvents

        private void HMI_Application_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Messenger.Default.Send(HMIAction.CloseHMI);
                Application.Current.Shutdown();
                Process.GetCurrentProcess().Kill();
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private string CurPage;

        private void listBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(ListBoxMenu.SelectedItem != null)
                {
                    MenuModel_ menuModel = (MenuModel_)ListBoxMenu.SelectedItem;
                    string ioPage = "MR1300";
                    //string motorPage = "Motor_Page";
                    string EngMode_Tag = "MR1000";

                    keyenceDll.Write<bool>(ioPage, false);

                    if (menuModel.MenuName == "Main")
                    {
                        Global.ActivatePage("Main");
                    }
                    else if (menuModel.MenuName == "Lot Entry")
                    {
                        Global.ActivatePage("Lot Entry");
                    }
                    else if (menuModel.MenuName == "OEE")
                    {                    
                        Global.ActivatePage("OEE");
                    }
                    else if (menuModel.MenuName == "Alarm History")
                    {
                        Global.ActivatePage("Alarm History");
                    }
                    else if (menuModel.MenuName == "Dry Run")
                    {
                        Global.ActivatePage("Dry Run");
                    }
                    else if (menuModel.MenuName == "Motor")
                    {
                        Global.ActivatePage("Motor");
                    }
                    else if (menuModel.MenuKey == "Robot")
                    {
                        Global.ActivatePage(HMI_Master_KX7500.ModulesView.Robot.RobotViewModel.PageName);
                    }
                    else if (menuModel.MenuName == "IO")
                    {
                        Global.ActivatePage("IO");
                        keyenceDll.Write<bool>(ioPage, true);
                    }
                    else if (menuModel.MenuName == "Cycle Time")
                    {
                        Global.ActivatePage("Cycle Time");
                    }
                    else if (menuModel.MenuName == "Engineering Mode")
                    {
                        Global.ActivatePage("Engineering Mode");
                    }
                    //else if (menuModel.MenuName == "User Access")
                    //{
                    //    Global.ActivatePage("User Access");
                    //}
                    else if (menuModel.MenuName == "Setting")
                    {
                        Global.ActivatePage("Setting");
                    }
                    //else if (menuModel.MenuName == "Configuration")
                    //{
                    //    Global.ActivatePage("Configuration");
                    //}
                    //PopUp

                    if (Global.MachineConfigModel.InitializeKX7500 == true)
                    {
                        // if (menuModel.MenuContent == ioView) { keyenceDll.Writedevice7KMR(ioPage, 1); }
                        // else { keyenceDll.Writedevice7KMR(ioPage, 0); }

                        //if (menuModel.MenuContent == motorView) { opcUA.Write(motorPage, true); }
                        //else { opcUA.Write(motorPage, false); }
                    }

                    if(CurPage != HmiHelper.GetResource<string>("Vision Setup") && menuModel?.MenuName == HmiHelper.GetResource<string>("Vision Setup"))
                        BlockPage(CurPage, menuModel?.MenuName, 0, EngMode_Tag, ref CurPage, 1);
                    else if(CurPage != HmiHelper.GetResource<string>("Welder Setup") && menuModel?.MenuName == HmiHelper.GetResource<string>("Welder Setup"))
                        BlockPage(CurPage, menuModel?.MenuName, 0, EngMode_Tag, ref CurPage, 2);
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void BlockPage(string FromPage, string ToPage, int ResetToPageIndex, string CanGoTag, ref string Curpage, int MsgType = 1)
        {
            string strMsg = ""; //1=Vision, 2=Welder

            if(MsgType == 1)
            { strMsg = $"Please enable Engineering Mode first, before go to Vision Setup Page"; }
            else if(MsgType == 2)
            { strMsg = $"Please enable Engineering Mode first, before go to Welder Setup Page"; }

            //bool bResult1 = Function.EnableVCType < 20;
            int bResult1 = keyenceDll.Read<int>(CanGoTag);

            if(bResult1 == 1)
            {
                CurPage = ToPage;
            }
            else
            {
                MessageBox.Show(strMsg, "Warning", MessageBoxButton.OK);

                //SuccessGo = false;
                Curpage = FromPage;
                ListBoxMenu.SelectedIndex = ResetToPageIndex;
            }
        }

        private void buttonControlPanelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //write the control button value to plc
                Button button = sender as Button;

                ButtonModel buttonModel = button.Tag as ButtonModel;

                //Messenger.Default.Send(new _OPC
                //{
                //    Module = new List<OPCModules> { OPCModules.TrackingStatus },
                //    Datachange = new Dictionary<string, string>
                //        { { "DBC_Data.Left_Stage.Unit_Status", z.ToString()} }

                //});
                //z++;

                //keyenceDll.Write(buttonModel.ButtonNode, 1);
                keyenceDll.Write<bool>(buttonModel.ButtonNode, true);
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void toggleControlPanelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //write the control toggle value to plc
                ToggleButton toggle = sender as ToggleButton;

                ToggleModel toggleModel = toggle.Tag as ToggleModel;

                //opcUA.Write(toggleModel.ToggleNode, toggle.IsChecked);
                if(toggle.IsChecked != null)
                {
                    if((bool)toggle.IsChecked)
                    { //keyenceDll.Write(toggleModel.ToggleNode, 1)
                      keyenceDll.Write<bool>(toggleModel.ToggleNode, true); }
                    else
                    { //keyenceDll.Write(toggleModel.ToggleNode, 0)
                      keyenceDll.Write<bool>(toggleModel.ToggleNode, false); }
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        #endregion PrivateEvents

        #region PrivateMethods

        private void addMenuModel(object pageContent, string pageImage, string pageName)
        {
            try
            {
                menuModels.Add(new MenuModel_
                {
                    MenuContent = pageContent,
                    MenuImage = pageImage,
                    MenuKey = pageName
                });
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void setDialogHostPromptIsOpen(bool isOpen, bool trueForPromptActionFalseForUserAccountSignInOut = true)
        {
            try
            {
                Visibility gridUserAccountSignInOut = Visibility.Visible;

                if(trueForPromptActionFalseForUserAccountSignInOut)
                {
                    gridUserAccountSignInOut = Visibility.Collapsed;
                }
                GridUserAccountSignInOut.Visibility = gridUserAccountSignInOut;
                DialogHostPrompt.IsOpen = isOpen;
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void updateMachineStatus(string status)
        {
            try
            {
                Brush brush = Brushes.Red;
                Brush Font = Brushes.Black;
                bool toggleButtonEngineeringModeIsEnabled = true;
                switch(status)
                {
                    case "1":
                        brush = Brushes.Lime;
                        status = "Running";
                        toggleButtonEngineeringModeIsEnabled = false;
                        break;

                    case "2":
                        brush = Brushes.Blue;
                        status = "Idle";
                        Font = Brushes.Snow;
                        break;

                    case "3":
                        brush = Brushes.Blue;
                        status = "Not Init Done";
                        Font = Brushes.Snow;
                        break;
                    case "4":
                        brush = Brushes.Blue;
                        status = "Initializing";
                        break;
                    case "5":
                        brush = Brushes.Red;
                        status = "Stop";
                        break;

                    case "6":
                        brush = Brushes.Red;
                        status = "Error";
                        break;

                    case "7":
                        brush = Brushes.Yellow;
                        status = "Warning";
                        break;

                    case "8":
                        brush = Brushes.Red;
                        status = "EMG";
                        break;

                    default:
                        break;
                }
                GroupBoxMachineStatus.Background = brush;
                TextBlockMachineStatus.Text = status;
                TextBlockMachineStatus.Foreground = Font;
                //ToggleButtonEngineeringMode.IsEnabled = toggleButtonEngineeringModeIsEnabled;
                controlPanelModel_.ForEach(y => y.ToggleModelList.ForEach(x => x.ToggleIsEnabled = toggleButtonEngineeringModeIsEnabled));
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void updateMachineMode(string Mode)
        {
            try
            {
                Brush brush = Brushes.Red;
                Brush font = Brushes.Black;
                string mode = "Unknown";
                switch(Mode)
                {
                    case "0":
                        brush = Brushes.Green;
                        mode = "Production";
                        break;

                    case "1":
                        brush = Brushes.Blue;
                        mode = "Engineering Mode";
                        font = Brushes.Snow;
                        break;

                    case "7":
                        brush = Brushes.Green;
                        mode = "Dry Run";
                        break;

                    default:
                        break;
                }
                GroupBoxMachineMode.Background = brush;
                TextBlockMachineMode.Text = mode;
                TextBlockMachineMode.Foreground = font;
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private async void updateAlarm(string _Key, string _Value)
        {
            try
            {
                List<AlarmMessageModel> TempOneAlarmList = new List<AlarmMessageModel>();
                string TypeCodes = _Key == alarmNodeModel.ErrorNodeString ? Error : Warning;

                logger.Debug($"{nameof(updateAlarm)} | TypeCodes : {TypeCodes} | Value : {_Value}");
                if(!string.IsNullOrEmpty(_Value) && _Value.Contains(','))
                {
                    string[] ErrorCodes = _Value.Split(',');
                    foreach(string ACode in ErrorCodes)
                    {
                        if(ACode.Length == 0)
                            continue;
                        Tuple<string, string> Key = Tuple.Create(ACode, TypeCodes);
                        string TempMsg = "NA";
                        string TempAction = "NA";

                        string TimeNow = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                        if(chinese)
                        {
                            if(ErrorListDictCN.ContainsKey(Key))
                            {
                                string[] Info = ErrorListDictCN[Key].Split(';');
                                TempMsg = Info[0];
                                TempAction = Info[1];
                            }
                        }
                        else
                        {
                            if(ErrorListDictEN.ContainsKey(Key))
                            {
                                string[] Info = ErrorListDictEN[Key].Split(';');
                                TempMsg = Info[0];
                                TempAction = Info[1];
                            }
                        }

                        AlarmMessageModel alarmMsgModel = new AlarmMessageModel
                        {
                            Code = ACode,
                            Action = TempAction,
                            Message = TempMsg,
                            Time = TimeNow,
                            Type = TypeCodes,
                        };
                        TempOneAlarmList.Add(alarmMsgModel);
                    }
                }
                if(TypeCodes == Error)
                {
                    AlarmBox.Define(TempOneAlarmList);
                    AlarmBox.Background = Brushes.Red;

                    #region ErrorCamline
                    List<string> errorMsg = new List<string>();
                    //bool errorDuringTesting = siemensOPC.ReadTag<bool>(LotEntry_Tag.Error_During_Testing);
                    bool errorDuringTesting = keyenceDll.Read<bool>(PLCTag.Error_During_Testing);
                    if (errorDuringTesting == true)
                    {
                        TempOneAlarmList.ForEach(x =>
                        {
                            errorMsg.Add(x.Message);
                        });

                        try
                        {
                            //string batchID = siemensOPC.ReadTag<string>(LotEntry_Tag.Current_LotID_String);
                            //string serialNumber = siemensOPC.ReadTag<string>(LotEntry_Tag.FoupBarcode);
                            //string equipID = serverSettingModel.EquipID;

                            string batchID = keyenceDll.Read<string>(PLCTag.Lot_ID, 50);
                            string serialNumber = keyenceDll.Read<string>(PLCTag.Foup_Barcode, 20);
                            string equipID = serverSettingModel.EquipID;

                            if (errorMsg.Count > 0)
                            {
                                errorMsg.ForEach(x =>
                                {
                                    if (x != "")
                                    {
                                        lotEntryView.entergris_TcpCommService.tcpIpClient.Send($"ERR_MSG|ERR_MSG:{x}|BATCHID:{batchID}|SERIAL_NO:{serialNumber}|EQID:{equipID}");
                                    }
                                });
                            }
                        }
                        catch (Exception ex) { logger.Error(ex); }
                    }
                    #endregion
                }
                else
                {
                    WarningBox.Define(TempOneAlarmList);
                    WarningBox.Background = Brushes.Yellow;

                    #region WarningCamline
                    List<string> warningMsg = new List<string>();
                    //bool warningDuringTesting = siemensOPC.ReadTag<bool>(LotEntry_Tag.Warning_During_Testing);
                    bool warningDuringTesting = keyenceDll.Read<bool>(PLCTag.Error_During_Testing);
                    if (warningDuringTesting == true)
                    {
                        TempOneAlarmList.ForEach(x =>
                        {
                            warningMsg.Add(x.Message);
                        });

                        try
                        {
                            string batchID = keyenceDll.Read<string>(PLCTag.Lot_ID, 50);
                            string serialNumber = keyenceDll.Read<string>(PLCTag.Foup_Barcode, 20);
                            string equipID = serverSettingModel.EquipID;

                            if (warningMsg.Count > 0)
                            {
                                warningMsg.ForEach(x =>
                                {
                                    if (x != "")
                                    {
                                        lotEntryView.entergris_TcpCommService.tcpIpClient.Send($"ERR_MSG|ERR_MSG:{x}|BATCHID:{batchID}|SERIAL_NO:{serialNumber}|EQID:{equipID}");
                                    }
                                });
                            }
                        }
                        catch (Exception ex) { logger.Error(ex); }

                    }
                    #endregion
                }

                try
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(100);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (TempOneAlarmList == null)
                            {
                                logger.Log(LogLevel.Debug, $"AlarmList is NULL");
                            }
                            else if (TempOneAlarmList.Count <= 0)
                            {
                                logger.Log(LogLevel.Debug, $"AlarmList is empty");
                            }
                            else
                            {
                                if (alarmhistoryView == null) { alarmhistoryView = new AlarmHistoryView(); }
                                alarmhistoryView.AddAlarm(TempOneAlarmList);
                            }
                        });
                    });
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Debug, ex.ToString());
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void hmiAction(HMIAction action)
        {
            if(action == HMIAction.CloseHMI && Global.MachineConfigModel.InitializeKX7500)
            {
                try
                {
                    keyenceDll.Disconnect();
                    //keyenceDll2.Disconnect();
                    //opcUA_3.Disconnect();
                    //keyenceDllRecipe.Disconnect();
                    //keyenceDllIO.Disconnect();
                    //keyenceDllMotor.Disconnect();
                }
                catch (Exception ex) { logger.Error(ex); }
            }
        }

        //private void SideColumnStartClick(object sender, RoutedEventArgs e)
        //{
        //    if(sideColumnPromptType == "StartSideColumn")
        //    {
        //        keyenceDll.Write<bool>("MR1606", true);
        //    }
        //    else
        //    {
        //        keyenceDll.Write<bool>("MR1600", true);
        //    }
        //}

        #endregion PrivateMethods

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //Info("SN:2212870 2025/02/04 16:34:29 OK  1.22sec 4430degrees Screw:01/01 Errors:00/02 Prog:05 Torque:14.16/14.07Lb/in Seq:Off Prog end"); //SN:2212870 2026/03/26 10:06:52 OK     1.25sec 04870degrees Screw:01/01 Errors:00/06 Prog:03 Torque:14.07/13.98Lbf.in Seq:Off Prog end
            //Info("SN:2212870 2025/02/04 18:46:16 NOK 2.20sec 6597degrees Screw:00/01 Errors:01/02 Prog:05 Torque:0.00/14.07Lb/in Seq:Off Max time error"); //SN:2212870 2026/03/26 15:28:08 NOK    4.05sec 08601degrees Screw:00/01 Errors:01/02 Prog:04 Torque:0.00/8.58Lbf.in Seq:Off Max angle error
            TestLoadCellListener("\n\nSN:2212870 "); //SN:2212870 2026/03/26 10:06:52 OK     1.25sec 04870degrees Screw:01/01 Errors:00/06 Prog:03 Torque:14.07/13.98Lbf.in Seq:Off Prog end
            TestLoadCellListener("\n\nSN:2212870 ");
            TestLoadCellListener("2026/03/26 10:06:52 OK     1.25sec 04870degrees Screw:01/01 Errors:00/06 Prog:03 Torque:14.07/13.98Lbf.in Seq:Off Prog end"); //SN:2212870 2026/03/26 10:06:52 OK     1.25sec 04870degrees Screw:01/01 Errors:00/06 Prog:03 Torque:14.07/13.98Lbf.in Seq:Off Prog end
            //TestLoadCellListener("SN:2212870 2026/03/26 15:28:08 NOK    4.05sec 08601degrees Screw:00/01 Errors:01/02 Prog:04 Torque:0.00/8.58Lbf.in Seq:Off Max angle error"); //SN:2212870 2026/03/26 15:28:08 NOK    4.05sec 08601degrees Screw:00/01 Errors:01/02 Prog:04 Torque:0.00/8.58Lbf.in Seq:Off Max angle error
        }

        private void TestLoadCellListener(string message)
        {
            try
            {
                string data = message.ToString();
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        LogHelper.COMPort(GetType().Name, $"RawData: {data}");
                        string result = Regex.Replace(data, @"(\r?\n)+", "");
                        //if (!data.Contains('\n'))
                        //{
                        //    serialData += data;
                        //    //LogHelper.COMPort(GetType().Name, $"Checking Not Contain new line : {serialData}");
                        //}
                        //else
                        //{
                        //    var splitData = data.Split('\n');
                        //    if (splitData.Length > 0)
                        //    {
                        //        serialData += splitData[0];
                        //        //LogHelper.COMPort(GetType().Name, $"Checking Contain new line (with splitData[0]): {serialData}");
                        //        //Dispatcher.Invoke(() => Info(data)); 
                        //        if (!string.IsNullOrWhiteSpace(serialData))
                        //        {
                        //            Dispatcher.Invoke(() => Info(serialData));
                        //        }
                        //        serialData = string.Empty;
                        //        if (splitData.Length > 1)
                        //        {
                        //            serialData += splitData[1];
                        //            //LogHelper.COMPort(GetType().Name, $"Checking Contain new line (with splitData[1]): {serialData}");
                        //        }
                        //    }
                        //}
                        if (data.Contains("SN:") && serialData.Contains("SN:"))
                        {
                            return;
                        }
                        else if (serialData.Contains(data))
                        {
                            return;
                        }
                        if (!string.IsNullOrEmpty(result))
                        {
                            serialData += result;
                            if (serialData.EndsWith("end") || serialData.EndsWith("error"))
                            {
                                Dispatcher.Invoke(() => Info(serialData));
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}