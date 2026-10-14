using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Interface;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.PopUp;
using HMI_Master_KX7500.Model.PopUp.Action;
using HMI_Master_KX7500.Service.PopUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace HMI_Master_KX7500.ModulesView.PopUp
{
    public class PopUp_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private Window window = null;
        private List<string> popUpTagList = new List<string>();
        private static OPCModules moduleName = OPCModules.PopUp;

        //private OPCUA_ opcUa = new OPCUA_();

        private KeyenceDll keyenceDll = null;
        #endregion

        #region PublicCommandProperties
        public ICommand PopUpViewLoadedCommand { get; set; }

        public ICommand PopUpViewClosingCommand { get; set; }
        #endregion

        #region PublicProperties
        private PopUpInterfaceModel popUpInterfaceModel = new PopUpInterfaceModel();
        public PopUpInterfaceModel PopUpInterfaceModel
        {
            get { return popUpInterfaceModel; }
            set
            {
                if (popUpInterfaceModel != value)
                {
                    popUpInterfaceModel = value;
                    RaisePropertyChanged(nameof(PopUpInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public PopUp_OPCUA_ViewModel(Window _window)
        {
            initializeCommand();
            initializeMessenger();
            window = _window;

            Messenger.Default.Send(new PopUpInitializeModel
            {
                PopUpFilePath = FilePath.PopUp
            });

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~PopUp_OPCUA_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            foreach (var x in PopUpInterfaceModel.PopUpList)
            {
                if (keyenceDll.Read<bool>(x.PopUpTagModel.PopUpTag))
                {
                    x.PopUp = true;
                    x.PopUpMessage = keyenceDll.Read<String>(x.PopUpValueTag, x.PopUpValueTagLength);

                    //Band aid solution
                    //if (x.PopUpTagModel.PopUpTag == "MR1509" || x.PopUpTagModel.PopUpTag == "MR1510")
                    //{
                    //    foreach (var y in x.PopUpActionList)
                    //    {
                    //        y.PopUpEnableInput = true;
                    //    }
                    //}

                    if (x.PopUpEnableAutoClose)
                    {
                        //window.ShowDialog();
                        //window.Activate();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            window.Show();
                            window.Activate();
                        });
                    }
                    else
                    {
                        keyenceDll.Write<bool>(x.PopUpTagModel.PopUpTag, false);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            window.Show();
                            window.Activate();
                        });
                    }                   
                }
                else
                {
                    if (x.PopUpEnableAutoClose)
                    {
                        if (x.PopUp == true)
                        {
                            x.PopUp = false;
                            //window.Hide();
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                window.Hide();
                            });
                        }
                    }
                }
            }
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            PopUpViewLoadedCommand = new RelayCommand(popUpViewLoadedCommand);
            //popUpViewLoadedCommand();
            PopUpViewClosingCommand = new RelayCommand<CancelEventArgs>(popUpViewClosingCommand);
        }

        private void initializeMessenger()
        {
            //initializePopUpList();
            Messenger.Default.Register<PopUpInitializeModel>(this, initializePopUpList);
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        //private void initializeopcUa(OPCUA_ opcUa_)
        //{
        //    if (opcUa_.OPC.Module.Contains(OPCModules.PopUp))
        //    {
        //        opcUa = opcUa_;
        //        popUpTagList = opcUa.GetAddresses();
        //    }
            
        //    //popUpTagList = PopUpService.GetPopUpTagList(PopUpInterfaceModel.PopUpList);

        //    //if (Global.MachineConfigModel.InitializeOPCUA)
        //    //{
        //    //    opcUa.InitializeUAOPC(Global.OPCUA_Url, popUpTagList);
        //    //}
        //}
        #endregion

        #region PrivateCommandMethods
        private void popUpViewLoadedCommand()
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void popUpViewClosingCommand(CancelEventArgs cancelEventArgs)
        {
            //keyenceDll.Write<bool>(x.PopUpTagModel.PopUpTag, false);
            cancelEventArgs.Cancel = true;
        }

        private void popUpActionCommand(PopUpActionModel popUpActionModel)
        {
            keyenceDll.Write<bool>(popUpActionModel.PopUpActionTagModel.PopUpActionTag, true);

            //Band aid solution
            if (keyenceDll.Read<bool>("MR1509"))
            {
                short val = keyenceDll.Read<short>("EM20200");
                val += popUpActionModel.PopUpInputQuantity;
                keyenceDll.Write<short>("EM20200", val);
                keyenceDll.Write<bool>("MR1509", false);
            }
            else if (keyenceDll.Read<bool>("MR1510"))
            {
                short val = keyenceDll.Read<short>("EM20201");
                val += popUpActionModel.PopUpInputQuantity;
                keyenceDll.Write<short>("EM20201", val);
                keyenceDll.Write<bool>("MR1510", false);
            }

            if (PopUpInterfaceModel.OffPopUp(popUpActionModel.PopUpActionTagModel.PopUpActionTag))
            {
                //keyenceDll.Write<bool>(x.PopUpTagModel.PopUpTag, true);
                window.Dispatcher.Invoke(new Action(() => window.Hide()));
            }          
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializePopUpList(PopUpInitializeModel popUpInitializeModel)
        {
            PopUpInterfaceModel.PopUpList = PopUpService.DeserializePopUpList();
            PopUpInterfaceModel.PopUpList.ToList().ForEach(x =>
            {
                x.PopUpActionList.ToList().ForEach(y =>
                {
                    y.PopUpActionCommand = new RelayCommand<PopUpActionModel>(popUpActionCommand);
                });
            });
            //initializeopcUa();
            //Messenger.Default.Register<OPCUA_>(this, initializeopcUa);
        }

        //private void opcUpdate(_OPC opc_)
        //{
        //    if (opc_.Module.Contains(moduleName))
        //    {
        //        //    if (popUpTagList.SequenceEqual(opc_.TagList))
        //        //{
        //        opc_.Datachange.ToList().ForEach(y =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if (PopUpInterfaceModel.PopUpList.ToList().Exists(
        //                    x => x.PopUpTagModel.PopUpTag == y.Key))
        //                {
        //                    window.Dispatcher.Invoke(new Action(() =>
        //                    {
        //                        PopUpModel popUpModel = PopUpInterfaceModel.PopUpList.ToList().Find(
        //                            x => x.PopUpTagModel.PopUpTag == y.Key);

        //                        //if (!string.IsNullOrEmpty(popUpModel.PopUpValueTag))
        //                        //{
        //                        //    popUpModel.PopUpValue = opcUa.Read<string>(popUpModel.PopUpValueTag);
        //                        //}
        //                        //else
        //                        //{
        //                        //    popUpModel.PopUpValue = "";
        //                        //}

        //                        if (Convert.ToBoolean(y.Value))
        //                        {
        //                            PopUpInterfaceModel.PopUpList.ToList().Find(
        //                                x => x.PopUpTagModel.PopUpTag ==
        //                                y.Key).PopUp = true;

        //                            if (popUpModel.PopUpEnableAutoClose)
        //                            {
        //                                window.ShowDialog();
        //                                window.Activate();
        //                            }
        //                            else
        //                            {
        //                                opcUa.Write(
        //                                    y.Key, false);
        //                                window.Show();
        //                                window.Activate();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (popUpModel.PopUpEnableAutoClose)
        //                            {
        //                                if (PopUpInterfaceModel.PopUpList.ToList().Exists(
        //                                    x => x.PopUp == true && x.PopUpTagModel.PopUpTag ==
        //                                    y.Key))
        //                                {
        //                                    PopUpInterfaceModel.PopUpList.ToList().Find(
        //                                        x => x.PopUp == true && x.PopUpTagModel.PopUpTag ==
        //                                        y.Key).PopUp = false;

        //                                    if (!PopUpInterfaceModel.PopUpList.ToList().Exists(
        //                                        x => x.PopUp == true))
        //                                    {
        //                                        window.Hide();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }));
        //                }
        //            });
        //        });
        //    }
        //}
        #endregion
    }
}
