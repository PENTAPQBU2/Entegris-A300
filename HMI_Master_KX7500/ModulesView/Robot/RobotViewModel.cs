using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.CommunicationProtocols.UR;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Robot;
using HMI_Master_KX7500.Service.Robot;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.Robot
{
    //Robot page (MT_002) and robot points of the recipe (MT_001)
    public class RobotViewModel : ViewModelBase
    {
        public const string PageName = "Robot";

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUrRobot robot;
        private readonly KeyenceDll keyenceDll;
        private readonly RobotSettingsModel settings;
        private UrRobotState state = new UrRobotState();
        private bool pageOpenWritten;
        private int busy;

        private static readonly string[,] sharedPoints =
        {
            { "P_Home", "Home" },
            { "P_Safe", "Safe" },
            { "P_CalReady", "Calibration ready" },
            { "P_CalStart", "Calibration start" },
            { "P_Cal85Nm", "Calibration 8.5 Nm" },
            { "P_Cal14Nm", "Calibration 14 Nm" }
        };

        public RobotViewModel()
        {
            settings = RobotSettingsService.Load();
            ipAddress = settings.RobotIPAddress;
            jogStepText = settings.JogStepDefault.ToString("F2", CultureInfo.InvariantCulture);

            robot = Global.IsOfflineMode ? (IUrRobot)new UrRobotSimulator() : new UrRobot();
            robot.StateChanged += onStateChanged;

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort,
                DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            ConnectCommand = new RelayCommand(connect, () => !IsConnected);
            DisconnectCommand = new RelayCommand(disconnect, () => IsConnected);
            PowerOnCommand = new RelayCommand(powerOn, () => IsConnected);
            RunCommand = new RelayCommand(() => pulse(PLCTag.Robot_Run, "Run"), () => IsConnected);
            PauseCommand = new RelayCommand(() => pulse(PLCTag.Robot_Pause, "Pause"), () => IsConnected);
            StopCommand = new RelayCommand(() => pulse(PLCTag.Robot_Stop, "Stop"), () => IsConnected);
            ResetCommand = new RelayCommand(reset, () => IsConnected);
            ApplySpeedCommand = new RelayCommand(applySpeed, () => IsConnected);
            TeachModeCommand = new RelayCommand(toggleTeachMode, () => IsConnected && (TeachAllowed || TeachRequested));
            JogCommand = new RelayCommand<string>(jog, x => InTeachMode);
            TeachPointCommand = new RelayCommand(teachPoint, () => InTeachMode && SelectedPoint != null);
            LoadRecipeToRobotCommand = new RelayCommand(loadRecipeToRobot, () => InTeachMode && SelectedRecipe != null);
            SaveRecipeCommand = new RelayCommand(saveRecipe, () => SelectedRecipe != null && RecipeDirty);

            RobotRecipeService.EnsureDefaultRecipes();
            reloadRecipes();

            Event.UpdateEvent += update;
        }

        ~RobotViewModel()
        {
            Event.UpdateEvent -= update;
        }

        #region Commands

        public RelayCommand ConnectCommand { get; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand PowerOnCommand { get; }
        public RelayCommand RunCommand { get; }
        public RelayCommand PauseCommand { get; }
        public RelayCommand StopCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand ApplySpeedCommand { get; }
        public RelayCommand TeachModeCommand { get; }
        public RelayCommand<string> JogCommand { get; }
        public RelayCommand TeachPointCommand { get; }
        public RelayCommand LoadRecipeToRobotCommand { get; }
        public RelayCommand SaveRecipeCommand { get; }

        #endregion Commands

        #region Bindable properties

        private string ipAddress;
        public string IpAddress { get => ipAddress; set => Set(ref ipAddress, value); }

        private bool isConnected;
        public bool IsConnected { get => isConnected; private set => Set(ref isConnected, value); }

        public bool IsOffline => Global.IsOfflineMode;

        private string connectionText = "Not connected";
        public string ConnectionText { get => connectionText; private set => Set(ref connectionText, value); }

        private string robotModeText = "-";
        public string RobotModeText { get => robotModeText; private set => Set(ref robotModeText, value); }

        private string safetyText = "-";
        public string SafetyText { get => safetyText; private set => Set(ref safetyText, value); }

        private bool isProtectiveStop;
        public bool IsProtectiveStop { get => isProtectiveStop; private set => Set(ref isProtectiveStop, value); }

        private string programText = "-";
        public string ProgramText { get => programText; private set => Set(ref programText, value); }

        private string currentPointText = "-";
        public string CurrentPointText { get => currentPointText; private set => Set(ref currentPointText, value); }

        private string robotRecipeText = "-";
        public string RobotRecipeText { get => robotRecipeText; private set => Set(ref robotRecipeText, value); }

        private string actualSpeedText = "-";
        public string ActualSpeedText { get => actualSpeedText; private set => Set(ref actualSpeedText, value); }

        private string[] tcpText = { "-", "-", "-", "-", "-", "-" };
        public string[] TcpText { get => tcpText; private set => Set(ref tcpText, value); }

        private string[] jointText = { "-", "-", "-", "-", "-", "-" };
        public string[] JointText { get => jointText; private set => Set(ref jointText, value); }

        private double speedPercent = 30;
        public double SpeedPercent { get => speedPercent; set => Set(ref speedPercent, Math.Round(value)); }

        private bool teachAllowed;
        public bool TeachAllowed { get => teachAllowed; private set => Set(ref teachAllowed, value); }

        private bool teachRequested;
        public bool TeachRequested { get => teachRequested; private set => Set(ref teachRequested, value); }

        private bool inTeachMode;
        public bool InTeachMode { get => inTeachMode; private set => Set(ref inTeachMode, value); }

        private string teachModeText = "Teach mode OFF";
        public string TeachModeText { get => teachModeText; private set => Set(ref teachModeText, value); }

        private string jogStepText;
        public string JogStepText { get => jogStepText; set => Set(ref jogStepText, value); }

        private bool toolFrame;
        public bool ToolFrame { get => toolFrame; set => Set(ref toolFrame, value); }

        public ObservableCollection<RobotRecipeModel> Recipes { get; } = new ObservableCollection<RobotRecipeModel>();

        private RobotRecipeModel selectedRecipe;
        public RobotRecipeModel SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                if(selectedRecipe == value) { return; }
                if(RecipeDirty && selectedRecipe != null &&
                   MessageBoxHelper.ShowMessageBox($"Recipe {selectedRecipe.RecipeName} has taught points that are not saved. Save now?",
                       "Robot", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    saveRecipe();
                }
                Set(ref selectedRecipe, value);
                RecipeDirty = false;
                buildPointList();
            }
        }

        public ObservableCollection<RobotPointItem> Points { get; } = new ObservableCollection<RobotPointItem>();

        private RobotPointItem selectedPoint;
        public RobotPointItem SelectedPoint { get => selectedPoint; set => Set(ref selectedPoint, value); }

        private bool recipeDirty;
        public bool RecipeDirty { get => recipeDirty; private set => Set(ref recipeDirty, value); }

        private string message = string.Empty;
        public string Message { get => message; private set => Set(ref message, value); }

        #endregion Bindable properties

        #region Connection

        private void connect()
        {
            IPAddress parsed;
            if(!IsOffline && !IPAddress.TryParse(IpAddress?.Trim() ?? string.Empty, out parsed))
            {
                show("Enter the robot IP address, e.g. 192.168.0.20");
                return;
            }

            string ip = IpAddress?.Trim() ?? string.Empty;
            settings.RobotIPAddress = ip;
            RobotSettingsService.Save(settings);

            run("Connect", () =>
            {
                robot.Connect(ip);
                UrRobotState s = robot.State;
                ui(() => SpeedPercent = Math.Round(s.SpeedFraction * 100));
                show(robot.IsRemoteControl ? "Connected." : "Connected. Robot is NOT in Remote control: start/stop/unlock from the HMI will be refused.");
            });
        }

        private void disconnect()
        {
            if(TeachRequested)
            {
                setTeachRequest(false);
            }
            run("Disconnect", () => robot.Disconnect());
        }

        #endregion Connection

        #region Robot control

        private void powerOn()
        {
            run("Power on", () =>
            {
                show(robot.Dashboard("power on"));
                DateTime until = DateTime.Now.AddSeconds(15);
                while(DateTime.Now < until && robot.State.RobotMode < 5)
                {
                    Thread.Sleep(200);
                }
                show(robot.Dashboard("brake release"));
            });
        }

        //Run / Pause / Stop go through the PLC, which drives the robot program registers
        private void pulse(string plcTag, string action)
        {
            keyenceDll.Write<bool>(plcTag, true);
            show($"{action} sent to PLC");
        }

        private void reset()
        {
            run("Reset", () =>
            {
                if(robot.State.IsProtectiveStop)
                {
                    robot.Dashboard("close safety popup");
                    show(robot.Dashboard("unlock protective stop"));
                }
                keyenceDll.Write<bool>(PLCTag.Robot_Reset, true);
            });
        }

        private void applySpeed()
        {
            double fraction = SpeedPercent / 100.0;
            run("Speed", () =>
            {
                robot.SetSpeedSlider(fraction);
                show($"Speed set to {fraction:P0}");
            });
        }

        #endregion Robot control

        #region Teach mode, jog, move

        private void toggleTeachMode()
        {
            setTeachRequest(!TeachRequested);
        }

        private void setTeachRequest(bool on)
        {
            if(!on && robot.InterpreterReady)
            {
                string reply;
                robot.Execute(RobotStatements.EndInterpreter(), out reply);
            }
            keyenceDll.Write<bool>(PLCTag.Robot_Teach_Req, on);
            robot.NotifyTeachRequest(on);
            TeachRequested = on;
            show(on ? "Teach mode requested - waiting for the robot." : "Teach mode ended. Send the robot home before production.");
        }

        private bool tryGetJogStep(out double step)
        {
            string text = (JogStepText ?? string.Empty).Trim();
            bool ok = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out step) ||
                      double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out step);
            if(!ok || step < settings.JogStepMin || step > settings.JogStepMax)
            {
                show($"Jog step must be between {settings.JogStepMin:F2} and {settings.JogStepMax:F2}");
                return false;
            }
            return true;
        }

        //parameter: axis index and direction, e.g. "0+" = X+, "5-" = Rz-
        private void jog(string parameter)
        {
            double step;
            if(string.IsNullOrEmpty(parameter) || parameter.Length != 2 || !tryGetJogStep(out step))
            {
                return;
            }

            int axis = parameter[0] - '0';
            double signedStep = parameter[1] == '-' ? -step : step;
            bool tool = ToolFrame;

            run("Jog", () =>
            {
                string statement = RobotStatements.Jog(robot.State.TcpPose, axis, signedStep, tool, settings);
                execute(statement);
            }, quiet: true);
        }

        //Hold-to-move: called by the view when the Move button is pressed and released
        public void StartMove()
        {
            RobotPointItem point = SelectedPoint;
            if(point == null || !InTeachMode)
            {
                return;
            }
            if(point.Kind != RobotPointKind.Shared && !recipeIsOnRobot())
            {
                return;
            }

            List<string> statements;
            switch(point.Kind)
            {
                case RobotPointKind.Screw: statements = RobotStatements.MoveToScrew(point.Variable, point.EntryVariable, settings); break;
                case RobotPointKind.Entry: statements = RobotStatements.MoveToEntry(point.Variable, settings); break;
                default: statements = RobotStatements.MoveToShared(point.Variable, settings); break;
            }

            show($"Moving to {point.Label} - release the button to stop");
            Task.Run(() =>
            {
                foreach(string statement in statements)
                {
                    if(!execute(statement)) { break; }
                }
            });
        }

        public void StopMove()
        {
            if(!InTeachMode)
            {
                return;
            }
            Task.Run(() =>
            {
                try
                {
                    robot.AbortMotion();
                    show("Motion stopped");
                }
                catch(Exception ex) { logger.Error(ex); show("Stop failed: " + ex.Message); }
            });
        }

        private bool recipeIsOnRobot()
        {
            if(SelectedRecipe == null || state.RecipeIdOut != SelectedRecipe.RecipeID)
            {
                show($"Load recipe {SelectedRecipe?.RecipeName} to the robot first (robot has recipe {state.RecipeIdOut}).");
                return false;
            }
            return true;
        }

        private void teachPoint()
        {
            RobotPointItem point = SelectedPoint;
            if(point == null || (point.Kind != RobotPointKind.Shared && !recipeIsOnRobot()))
            {
                return;
            }

            double[] pose = robot.State.TcpPose;
            string where = point.Kind == RobotPointKind.Shared ? "the robot (all recipes)" : $"recipe {SelectedRecipe.RecipeName}";
            if(MessageBoxHelper.ShowMessageBox($"Teach {point.Label} ({point.Variable}) at the current robot position for {where}?",
                   "Teach point", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
            {
                return;
            }

            run("Teach", () =>
            {
                if(!execute(RobotStatements.Assign(point.Variable, pose)))
                {
                    return;
                }
                if(point.RecipePoint != null)
                {
                    point.RecipePoint.SetPose(pose);
                    point.RecipePoint.Source = "Taught " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ui(() =>
                    {
                        RecipeDirty = true;
                        refreshPoint(point);
                    });
                    show($"{point.Label} taught on the robot. Press Save to keep it in recipe {SelectedRecipe.RecipeName}.");
                }
                else
                {
                    show($"{point.Label} taught and kept on the robot.");
                }
            });
        }

        private void loadRecipeToRobot()
        {
            RobotRecipeModel recipe = SelectedRecipe;
            if(MessageBoxHelper.ShowMessageBox($"Load the {recipe.Points.Count} robot points of recipe {recipe.RecipeName} to the robot?",
                   "Load recipe", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
            {
                return;
            }

            run("Load recipe", () =>
            {
                List<string> statements = RobotStatements.LoadRecipe(recipe);
                for(int i = 0; i < statements.Count; i++)
                {
                    if(!execute(statements[i]))
                    {
                        show($"Load stopped at {i + 1} of {statements.Count}. Robot recipe is incomplete - load again.");
                        return;
                    }
                }
                show($"Recipe {recipe.RecipeName} loaded to the robot ({recipe.Points.Count} points).");
            });
        }

        private void saveRecipe()
        {
            try
            {
                RobotRecipeService.Save(SelectedRecipe);
                RecipeDirty = false;
                show($"Recipe {SelectedRecipe.RecipeName} saved.");
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Save failed: " + ex.Message);
            }
        }

        private bool execute(string statement)
        {
            string reply;
            if(robot.Execute(statement, out reply))
            {
                return true;
            }
            show("Robot refused: " + (reply ?? "no reply"));
            return false;
        }

        #endregion Teach mode, jog, move

        #region Recipes and points

        private void reloadRecipes()
        {
            Recipes.Clear();
            foreach(RobotRecipeModel recipe in RobotRecipeService.LoadAll())
            {
                Recipes.Add(recipe);
            }
            SelectedRecipe = Recipes.FirstOrDefault();
        }

        private void buildPointList()
        {
            Points.Clear();
            if(SelectedRecipe != null)
            {
                foreach(RobotPointModel p in SelectedRecipe.Points)
                {
                    Points.Add(new RobotPointItem
                    {
                        Variable = p.Variable,
                        Label = p.Label,
                        Kind = p.Variable.EndsWith("_E") ? RobotPointKind.Entry : RobotPointKind.Screw,
                        RecipePoint = p
                    });
                }
            }

            for(int i = 0; i < sharedPoints.GetLength(0); i++)
            {
                Points.Add(new RobotPointItem { Variable = sharedPoints[i, 0], Label = sharedPoints[i, 1], Kind = RobotPointKind.Shared });
            }
            SelectedPoint = Points.FirstOrDefault();
        }

        //Re-insert the item so the list shows its new value
        private void refreshPoint(RobotPointItem point)
        {
            int index = Points.IndexOf(point);
            if(index >= 0)
            {
                Points[index] = new RobotPointItem { Variable = point.Variable, Label = point.Label, Kind = point.Kind, RecipePoint = point.RecipePoint };
                SelectedPoint = Points[index];
            }
        }

        #endregion Recipes and points

        #region State updates

        private void update()
        {
            try
            {
                bool active = Global.PageList.ContainsKey(PageName) && Global.PageList[PageName];
                if(active != pageOpenWritten)
                {
                    keyenceDll.Write<bool>(PLCTag.Robot_Page_Open, active);
                    pageOpenWritten = active;
                    if(!active && TeachRequested)
                    {
                        ui(() => setTeachRequest(false));
                    }
                }

                if(!active)
                {
                    return;
                }

                bool allowed = Global.IsOfflineMode || keyenceDll.Read<bool>(PLCTag.Robot_Teach_Allowed);
                if(allowed != TeachAllowed)
                {
                    ui(() => TeachAllowed = allowed);
                }
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void onStateChanged(UrRobotState s)
        {
            if(!(Global.PageList.ContainsKey(PageName) && Global.PageList[PageName]))
            {
                state = s;
                return;
            }

            ui(() =>
            {
                state = s;
                IsConnected = s.Connected;
                ConnectionText = s.Connected ? (IsOffline ? "Connected (simulated robot)" : "Connected " + IpAddress) : "Not connected";
                RobotModeText = s.RobotModeText;
                SafetyText = s.SafetyStatusText;
                IsProtectiveStop = s.IsProtectiveStop;
                ProgramText = s.RuntimeStateText;
                CurrentPointText = s.CurrentPointText;
                ActualSpeedText = $"{s.SpeedFraction:P0}";
                InTeachMode = s.InTeachMode && robot.InterpreterReady;
                TeachModeText = InTeachMode ? "Teach mode ON" : (TeachRequested ? "Teach mode requested" : "Teach mode OFF");

                RobotRecipeModel onRobot = Recipes.FirstOrDefault(x => x.RecipeID == s.RecipeIdOut);
                RobotRecipeText = onRobot != null ? $"{s.RecipeIdOut} - {onRobot.RecipeName}" : s.RecipeIdOut.ToString();

                TcpText = new[]
                {
                    (s.TcpPose[0] * 1000).ToString("F2"), (s.TcpPose[1] * 1000).ToString("F2"), (s.TcpPose[2] * 1000).ToString("F2"),
                    (s.TcpPose[3] * 180 / Math.PI).ToString("F2"), (s.TcpPose[4] * 180 / Math.PI).ToString("F2"), (s.TcpPose[5] * 180 / Math.PI).ToString("F2")
                };
                JointText = s.ActualQ.Select(q => (q * 180 / Math.PI).ToString("F2")).ToArray();
            });
        }

        #endregion State updates

        #region Helpers

        //Runs robot communication off the UI thread, one operation at a time
        private void run(string name, Action action, bool quiet = false)
        {
            if(Interlocked.Exchange(ref busy, 1) == 1)
            {
                if(!quiet) { show("Robot is busy, try again"); }
                return;
            }

            Task.Run(() =>
            {
                try { action(); }
                catch(Exception ex)
                {
                    logger.Error(ex);
                    show($"{name} failed: {ex.Message}");
                }
                finally
                {
                    Interlocked.Exchange(ref busy, 0);
                    ui(CommandManager_Refresh);
                }
            });
        }

        private static void CommandManager_Refresh()
        {
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        private void show(string text)
        {
            LogHelper.KeyValue(GetType().Name, "Robot page", text);
            ui(() => Message = $"{DateTime.Now:HH:mm:ss}  {text}");
        }

        private static void ui(Action action)
        {
            Application app = Application.Current;
            if(app == null) { return; }
            if(app.Dispatcher.CheckAccess()) { action(); }
            else { app.Dispatcher.BeginInvoke(action); }
        }

        #endregion Helpers
    }
}
