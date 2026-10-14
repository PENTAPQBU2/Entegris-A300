using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.Main;
using HMI_Master_KX7500.Service.Main;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace HMI_Master_KX7500.Interface
{
    public class MachineLayoutInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly MachineLayoutService machineLayoutService = new MachineLayoutService();
        public const string DefaultColor = "DefaultColor";
        private Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly MachineLayoutEditorView machineLayoutEditorView = new MachineLayoutEditorView();

        private KeyenceDll keyenceDll = null;
        #endregion

        #region PublicProperties
        private MachineLayoutModel machineLayoutModel = new MachineLayoutModel();
        public MachineLayoutModel MachineLayoutModel
        {
            get { return machineLayoutModel; }
            set
            {
                if (machineLayoutModel != value)
                {
                    machineLayoutModel = value;
                    RaisePropertyChanged(nameof(MachineLayoutModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public MachineLayoutInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            //MethodHelper.Action(GetType().Name, nameof(initializeCommand),
            //    () => EditMachineLayoutCommand = new RelayCommand(editMachineLayoutCommand));
        }

        private void initializeMessenger()
        {
            initializeMachineLayout();
            //Messenger.Default.Register<MachineLayoutUpdate>(this, updateMachineLayout);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeMachineLayout()
        {
            MachineLayoutModel = MachineLayoutService.DeserializeMachineLayout();
        }

        private void Update()
        {
            if (Global.PageList["Main"])
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var x in MachineLayoutModel.MachineLayoutModuleList)
                    {
                        foreach (var y in x.MachineLayoutModuleStatusIndicatorList)
                        {
                            switch (y.MachineLayoutModuleStatusIndicatorKey)
                            {
                                case "Shell_Loading_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM140").ToString();                                    
                                    break;
                                case "Top_Clamp_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM141").ToString();
                                    break;
                                case "Flipping_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM142").ToString();
                                    break;
                                case "Shim_and_O-Ring_Alignment_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM143").ToString();
                                    break;
                                case "Side_Column_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM144").ToString();
                                    break;
                                case "Vision_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM145").ToString();
                                    break;
                                case "UR_Robot":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<short>("EM146").ToString();
                                    break;
                            }

                            foreach (var z in MachineLayoutModel.MachineLayoutModuleStatusList)
                            {
                                if (y.MachineLayoutModuleStatusIndicatorIndex == z.MachineLayoutModuleStatusIndex)
                                {
                                    y.MachineLayoutModuleStatusIndicatorColor = z.MachineLayoutModuleStatusColor;
                                }
                            }
                        }
                    }
                });
            }
        }

        private void updateMachineLayout(MachineLayoutUpdate machineLayoutUpdate)
        {
            if (Global.PageList["Main"])
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var x in MachineLayoutModel.MachineLayoutModuleList)
                    {
                        foreach (var y in x.MachineLayoutModuleStatusIndicatorList)
                        {
                            switch (y.MachineLayoutModuleStatusIndicatorKey)
                            {
                                case "Shell_Loading_Module":
                                    y.MachineLayoutModuleStatusIndicatorIndex = keyenceDll.Read<int>("EM140").ToString();                                  
                                    break;
                            }
                        }
                    }
                });
            }

            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            //    switch (machineLayoutUpdate)
            //    {
            //        case MachineLayoutUpdate.Index:
            //            MachineLayoutModel.MachineLayoutModuleList.ForEach(x =>
            //            {
            //                x.MachineLayoutModuleStatusIndicatorList.ForEach(y =>
            //                {
            //                    if (MachineLayoutModel.MachineLayoutModuleStatusList.Exists(
            //                           z => z.MachineLayoutModuleStatusIndex ==
            //                           y.MachineLayoutModuleStatusIndicatorIndex))
            //                    {
            //                        y.MachineLayoutModuleStatusIndicatorColor =
            //                        MachineLayoutModel.MachineLayoutModuleStatusList.Find(
            //                            z => z.MachineLayoutModuleStatusIndex ==
            //                            y.MachineLayoutModuleStatusIndicatorIndex).MachineLayoutModuleStatusColor;

            //                        logger.Debug($"MachineLayoutModuleStatusColor: {MachineLayoutModel.MachineLayoutModuleStatusList.Find(z => z.MachineLayoutModuleStatusIndex == y.MachineLayoutModuleStatusIndicatorIndex).MachineLayoutModuleStatusColor}");
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                          //  y.MachineLayoutModuleStatusIndicatorColor = GetResource<SolidColorBrush>(DefaultColor).ToString();
            //                        }
            //                        catch (Exception e)
            //                        {
            //                            logger.Error(e);
            //                        }
            //                    }
            //                });
            //            });
            //            break;
            //        default:
            //            break;
            //    }
            //}));
        }

        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditMachineLayout = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.MachineLayoutEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            MachineLayoutModel.MachineLayoutModuleList.ForEach(x =>
            {
                x.MachineLayoutModuleName = HmiHelper.GetResource<string>(x.MachineLayoutModuleKey);

                x.MachineLayoutModuleStatusIndicatorList.ForEach(y =>
                {
                    y.MachineLayoutModuleStatusIndicatorName = HmiHelper.GetResource<string>(y.MachineLayoutModuleStatusIndicatorKey);
                });
            });

            MachineLayoutModel.MachineLayoutModuleStatusList.ForEach(x =>
            {
                x.MachineLayoutModuleStatusName = HmiHelper.GetResource<string>(x.MachineLayoutModuleStatusKey);
            });
        }
        #endregion

        #region PrivateMethod
        private static T GetResource<T>(string key)
        {
            var value = default(T);
            value = (T)Application.Current.TryFindResource(key);
            return value;
        }
        #endregion
    }
}
