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
                SL.Open(true);

                token = source.Token;
                Event.UpdateEvent += Update;
                Task.Run(() =>
                {
                    while(!token.IsCancellationRequested)
                    {
                        //Thread.Sleep(100);
                        Event.RaiseUpdateEvent();
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

        #region FOUP CSV (MT_003)
        // One folder + one CSV per shell (FOUP). Format follows customer example (insert machine CSV):
        //   folder : D:\HMI\FOUP CSV\<LotID>_<00001>_InProgress   (while running)
        //            D:\HMI\FOUP CSV\<LotID>_<00001>_Pass / _Fail  (when finished)
        //   file   : <yyyyMMddHHmmss>.csv  (shell start time)
        //   content: FOUPBarcode,<LotID>_<00001>_Pass
        //            LotID,<LotID>
        //            Recipe,<shell colour>
        //            Result,0 (0 = Pass, 1 = Fail)
        //            ShellType,New / Rework / Rework-ConveyorPlate / Rework-SideColumn
        //            (blank line)
        //            ,Result,ScrewDateTime,Torque,TargetTorque,TorqueUnit,Angle,Time,ScrewType,Error
        //            Screw No 1,OK,...
        // <00001> = running number per Lot ID, generated by HMI (rework also gets a new number),
        //           saved in MachineConfiguration\FoupCsvRunningNumber.txt so it survives HMI restart.
        // Shell start = rising edge of New_Shell or Rework_Shell (operator choice, PLC step 1-2).
        // Shell end   = falling edge of New_Shell and Rework_Shell (PLC step 360 or hard reset).
        // Pass        = Overall_Pass seen during the shell (PLC step 340, screw set 4 done), otherwise Fail.
        // Screw results outside a shell: D:\HMI\FOUP CSV\NoShell\<LotID>_NoShell_<yyyyMMdd>.csv
        private readonly object foupCsvLock = new object();
        private bool foupCsvShellActive = false;
        private bool foupCsvShellPassed = false;
        private string foupCsvShellType = string.Empty;
        private string foupCsvLotId = string.Empty;
        private string foupCsvRecipe = string.Empty;
        private string foupCsvUnitName = string.Empty;   // <LotID>_<00001>
        private string foupCsvShellFolder = string.Empty;
        private string foupCsvFile = string.Empty;
        private bool foupCsvLastShellBit = false;
        private const string foupCsvTableHeader = ",Result,ScrewDateTime,Torque,TargetTorque,TorqueUnit,Angle,Time,ScrewType,Error";
#if DEBUG
        private const string foupCsvRootFolder = @"D:\HMI_A300\FOUP CSV\";
        private int debugScrewNum = 0;
#else
        private const string foupCsvRootFolder = @"D:\HMI\FOUP CSV\";
#endif

        private static readonly string[,] foupCsvColourTags =
        {
            { "Black", PLCTag.Shell_Colour_Black },
            { "Transparent", PLCTag.Shell_Colour_Transparent },
            { "Amber", PLCTag.Shell_Colour_Amber },
            { "Green", PLCTag.Shell_Colour_Green },
            { "Red", PLCTag.Shell_Colour_Red },
            { "Blue", PLCTag.Shell_Colour_Blue },
        };

        private static string toSafeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        private static string cleanPlcString(string value)
        {
            return (value ?? string.Empty).Replace("\0", string.Empty).Trim();
        }

        // Next running number for this Lot ID (1, 2, 3 ...). Resets to 1 when Lot ID changes.
        private static int foupCsvNextRunningNumber(string lotName)
        {
            string counterFile = Path.Combine(FolderPath.MachineConfiguration, "FoupCsvRunningNumber.txt");
            int number = 1;
            try
            {
                if (File.Exists(counterFile))
                {
                    string[] parts = File.ReadAllText(counterFile).Trim().Split('|');
                    if (parts.Length == 2 && parts[0] == lotName && int.TryParse(parts[1], out int last))
                    {
                        number = last + 1;
                    }
                }
                File.WriteAllText(counterFile, $"{lotName}|{number}");
            }
            catch (Exception ex) { LogHelper.Error(nameof(FrameView), ex); }
            return number;
        }

        // Called from the Update() loop (PLC polling thread)
        private void foupCsvTrackShell()
        {
            try
            {
                bool newShell = keyenceDll.Read<bool>(PLCTag.New_Shell);
                bool reworkShell = keyenceDll.Read<bool>(PLCTag.Rework_Shell);
                bool shellBit = newShell || reworkShell;

                if (shellBit && !foupCsvLastShellBit)
                {
                    string lotId = cleanPlcString(keyenceDll.Read<string>(PLCTag.Lot_ID, 50));
                    string recipe = "Unknown";
                    for (int i = 0; i < foupCsvColourTags.GetLength(0); i++)
                    {
                        if (keyenceDll.Read<bool>(foupCsvColourTags[i, 1]))
                        {
                            recipe = foupCsvColourTags[i, 0];
                            break;
                        }
                    }
                    foupCsvStartShell(reworkShell ? "Rework" : "New", lotId, recipe);
                }

                if (shellBit)
                {
                    bool overallPass = keyenceDll.Read<bool>(PLCTag.Overall_Pass);
                    bool reworkConveyorPlate = reworkShell && keyenceDll.Read<bool>(PLCTag.Rework_ConveyorPlate);
                    bool reworkSideColumn = reworkShell && keyenceDll.Read<bool>(PLCTag.Rework_SideColumn);
                    lock (foupCsvLock)
                    {
                        if (foupCsvShellActive)
                        {
                            if (overallPass) { foupCsvShellPassed = true; }
                            if (reworkConveyorPlate) { foupCsvShellType = "Rework-ConveyorPlate"; }
                            else if (reworkSideColumn) { foupCsvShellType = "Rework-SideColumn"; }
                        }
                    }
                }

                if (!shellBit && foupCsvLastShellBit)
                {
                    foupCsvEndShell();
                }

                foupCsvLastShellBit = shellBit;
            }
            catch (Exception ex) { LogHelper.Error(GetType().Name, ex); }
        }

        private void foupCsvStartShell(string shellType, string lotId, string recipe)
        {
            lock (foupCsvLock)
            {
                if (foupCsvShellActive)
                {
                    foupCsvFinishFileLocked(); // previous shell was never closed
                }

                string lotName = string.IsNullOrEmpty(lotId) ? "NoLotID" : toSafeFileName(lotId);
                int runningNumber = foupCsvNextRunningNumber(lotName);
                foupCsvLotId = lotId;
                foupCsvRecipe = recipe;
                foupCsvShellType = shellType;
                foupCsvUnitName = $"{lotName}_{runningNumber:00000}";
                foupCsvShellFolder = Path.Combine(foupCsvRootFolder, $"{foupCsvUnitName}_InProgress");
                foupCsvFile = Path.Combine(foupCsvShellFolder, $"{DateTime.Now:yyyyMMddHHmmss}.csv");
                foupCsvShellPassed = false;
                foupCsvShellActive = true;

                if (!Directory.Exists(foupCsvShellFolder))
                {
                    Directory.CreateDirectory(foupCsvShellFolder);
                }
                LogHelper.COMPort(GetType().Name, $"FOUP CSV shell start ({shellType}, {recipe}): {foupCsvFile}");
            }
        }

        private void foupCsvEndShell()
        {
            lock (foupCsvLock)
            {
                if (foupCsvShellActive)
                {
                    foupCsvFinishFileLocked();
                }
            }
        }

        // Add the header block (now that Pass/Fail is known) and rename the folder. Caller must hold foupCsvLock.
        private void foupCsvFinishFileLocked()
        {
            try
            {
                string status = foupCsvShellPassed ? "Pass" : "Fail";
                string finalName = $"{foupCsvUnitName}_{status}";
                if (Directory.Exists(Path.Combine(foupCsvRootFolder, finalName))) // should not happen (running number), keep both
                {
                    finalName = $"{finalName}_{DateTime.Now:yyyyMMddHHmmss}";
                }

                if (File.Exists(foupCsvFile))
                {
                    string rows = File.ReadAllText(foupCsvFile);
                    string header =
                        $"FOUPBarcode,{finalName}{Environment.NewLine}" +
                        $"LotID,{foupCsvLotId}{Environment.NewLine}" +
                        $"Recipe,{foupCsvRecipe}{Environment.NewLine}" +
                        $"Result,{(foupCsvShellPassed ? 0 : 1)}{Environment.NewLine}" +
                        $"ShellType,{foupCsvShellType}{Environment.NewLine}" +
                        Environment.NewLine;
                    File.WriteAllText(foupCsvFile, header + rows);
                }
                else
                {
                    LogHelper.COMPort(GetType().Name, $"FOUP CSV shell end ({status}) without screw data: {foupCsvUnitName}");
                }

                string finalFolder = Path.Combine(foupCsvRootFolder, finalName);
                if (Directory.Exists(foupCsvShellFolder))
                {
                    Directory.Move(foupCsvShellFolder, finalFolder);
                }
                LogHelper.COMPort(GetType().Name, $"FOUP CSV shell end: {finalFolder}");
            }
            catch (Exception ex) { LogHelper.Error(GetType().Name, ex); }
            finally
            {
                foupCsvShellActive = false;
                foupCsvShellPassed = false;
                foupCsvShellFolder = string.Empty;
                foupCsvFile = string.Empty;
            }
        }

        // "14.95/14.07Lbf.in" -> torque 14.95, target 14.07, unit Lbf.in
        private static void splitTorque(string raw, out string torque, out string target, out string unit)
        {
            torque = (raw ?? string.Empty).Trim();
            target = string.Empty;
            unit = string.Empty;
            Match m = Regex.Match(torque, @"^([-\d.]+)/([-\d.]+)\s*(.*)$");
            if (m.Success)
            {
                torque = m.Groups[1].Value;
                target = m.Groups[2].Value;
                unit = m.Groups[3].Value;
            }
        }

        // Called from Info() for every screw result received from the torque driver
        private void foupCsvWriteRow(int screwNum, string screwDateTime, string screwResult, string screwType, string error,
            string torqueVal, string tighteningAngle, string tighteningTime)
        {
            splitTorque(torqueVal, out string torque, out string targetTorque, out string torqueUnit);
            string angle = (tighteningAngle ?? string.Empty).Replace("degrees", string.Empty).Trim();
            string time = (tighteningTime ?? string.Empty).Replace("sec", string.Empty).Trim();
            string row = $"Screw No {screwNum},{(screwResult ?? string.Empty).Trim()},{screwDateTime},{torque},{targetTorque},{torqueUnit},{angle},{time},{(screwType ?? string.Empty).Trim()},{(error ?? string.Empty).Trim()}";

            lock (foupCsvLock)
            {
                string file;
                if (foupCsvShellActive)
                {
                    file = foupCsvFile;
                }
                else
                {
                    // Screw result outside a shell process (e.g. engineering mode / calibration)
#if DEBUG
                    string lotId = "LotTest123";
#else
                    string lotId = cleanPlcString(keyenceDll.Read<string>(PLCTag.Lot_ID, 50));
#endif
                    string lotName = string.IsNullOrEmpty(lotId) ? "NoLotID" : toSafeFileName(lotId);
                    file = Path.Combine(foupCsvRootFolder, "NoShell", $"{lotName}_NoShell_{DateTime.Now:yyyyMMdd}.csv");
                    row = $"{DateTime.Now},{row}";
                }

                string folder = Path.GetDirectoryName(file);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                bool fileExist = File.Exists(file);
                using (FileStream stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        if (!fileExist)
                        {
                            writer.WriteLine(foupCsvShellActive ? foupCsvTableHeader : $"DateTime{foupCsvTableHeader}");
                        }
                        writer.WriteLine(row);
                        writer.Flush();
                    }
                }
            }
        }
        #endregion FOUP CSV (MT_003)

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

                    //foup csv - one folder + one file per shell (see region FOUP CSV (MT_003))
#if DEBUG
                    // Offline test: simulate one shell = screw 1..21, all Pass
                    debugScrewNum = (debugScrewNum % 21) + 1;
                    int screwNum = debugScrewNum;
                    if (screwNum == 1)
                    {
                        foupCsvStartShell("New", "LotTest123", "Black");
                    }
#else
                    int screwNum = keyenceDll.Read<int>(PLCTag.Screw_Number);
#endif
                    foupCsvWriteRow(screwNum, screwDateTime, screwResult, screwType, error, torqueVal, tighteningAngle, tighteningTime);
#if DEBUG
                    if (screwNum == 21)
                    {
                        lock (foupCsvLock) { foupCsvShellPassed = true; }
                        foupCsvEndShell();
                    }
#endif
                    serialData = string.Empty;
                }

            }
            catch (Exception ex) 
            {
                LogHelper.Error(GetType().Name, ex);
            }
        }

        private void Update()
        {
            try
            {
                if (keyenceDll != null)
                {
#if !DEBUG
                    foupCsvTrackShell(); // FOUP CSV: detect shell start / pass / end from PLC
#endif
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
                    
                    //keyenceDll = new KeyenceDll(Global.GetList_GeneralNodeList(mainViewModel.GetTagList()), Global.OPCUA_Url, "8500", DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, modules1);
                    keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, "8500", DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, modules1);

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