using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.TrackingStatus;
using HMI_Master_KX7500.Service.TrackingStatus;
using NLog;
using System;
using System.Windows;
using System.Windows.Media;

namespace HMI_Master_KX7500.Interface
{
    public class TrackingStatusInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly TrackingStatusService trackingStatusService = new TrackingStatusService();
        public const string DefaultColor = "DefaultColor";
        private Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region PublicProperties
        private TrackingStatusModel trackingStatusModel = new TrackingStatusModel();
        public TrackingStatusModel TrackingStatusModel
        {
            get { return trackingStatusModel; }
            set
            {
                if (trackingStatusModel != value)
                {
                    trackingStatusModel = value;
                    RaisePropertyChanged(nameof(TrackingStatusModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public TrackingStatusInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            //EditTrackingStatusCommand = new RelayCommand(editTrackingStatusCommand);
        }

        private void initializeMessenger()
        {
            initializeTrackingStatus();
            //Messenger.Default.Register<TrackingStatusInitializeModel>(this, initializeTrackingStatus);
            Messenger.Default.Register<TrackingStatusUpdate>(this, updateTrackingStatus);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeTrackingStatus(/*TrackingStatusInitializeModel trackingStatusInitializeModel*/)
        {
            TrackingStatusModel = TrackingStatusService.DeserializeTrackingStatus();
        }

        private void updateTrackingStatus(TrackingStatusUpdate trackingStatusUpdate)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                switch (trackingStatusUpdate)
                {
                    case TrackingStatusUpdate.Index:
                        TrackingStatusModel.TrackingStatusModuleList.ForEach(x =>
                        {
                            x.TrackingStatusModuleStatusIndicatorList.ForEach(y =>
                            {
                                if (TrackingStatusModel.TrackingStatusModuleStatusList.Exists(
                                        z => z.TrackingStatusModuleStatusIndex ==
                                        y.TrackingStatusModuleStatusIndicatorIndex))
                                {
                                    y.TrackingStatusModuleStatusIndicatorColor =
                                    TrackingStatusModel.TrackingStatusModuleStatusList.Find(
                                        z => z.TrackingStatusModuleStatusIndex ==
                                        y.TrackingStatusModuleStatusIndicatorIndex
                                        ).TrackingStatusModuleStatusColor;
                                }
                                else
                                {
                                    try
                                    {
                                   //     y.TrackingStatusModuleStatusIndicatorColor = GetResource<SolidColorBrush>(DefaultColor).ToString();
                                    }
                                    catch (Exception e)
                                    {

                                        logger.Error(e);
                                    }
                                }
                                if (x.TrackingStatusModuleStatusList.Exists(
                                            z => z.TrackingStatusModuleStatusIndex ==
                                            y.TrackingStatusModuleStatusIndicatorIndex))
                                {
                                    y.TrackingStatusModuleStatusIndicatorColor =
                                    x.TrackingStatusModuleStatusList.Find(
                                        z => z.TrackingStatusModuleStatusIndex ==
                                        y.TrackingStatusModuleStatusIndicatorIndex
                                        ).TrackingStatusModuleStatusColor;
                                }
                            });
                        });
                        break;
                    default:
                        break;
                }
            }));
        }

        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditTrackingStatus = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.TrackingStatusEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            TrackingStatusModel.TrackingStatusModuleList.ForEach(x =>
            {
                x.TrackingStatusModuleName = HmiHelper.GetResource<string>(x.TrackingStatusModuleKey);

                x.TrackingStatusModuleStatusIndicatorList.ForEach(y =>
                {
                    y.TrackingStatusModuleStatusIndicatorName = HmiHelper.GetResource<string>(y.TrackingStatusModuleStatusIndicatorKey);
                });
                x.TrackingStatusModuleStatusList.ForEach(y =>
                {
                    y.TrackingStatusModuleStatusName = HmiHelper.GetResource<string>(y.TrackingStatusModuleStatusKey);
                });
            });

            TrackingStatusModel.TrackingStatusModuleStatusList.ForEach(x =>
            {
                x.TrackingStatusModuleStatusName = HmiHelper.GetResource<string>(x.TrackingStatusModuleStatusKey);
            });

            TrackingStatusModel.TrackingStatusReadOnlyGroupList.ForEach(x =>
            {
                x.TrackingStatusReadOnlyGroupName = HmiHelper.GetResource<string>(x.TrackingStatusReadOnlyGroupKey);
                x.TrackingStatusReadOnlyList.ForEach(z =>
                {
                    z.TrackingStatusReadOnlyName = HmiHelper.GetResource<string>(z.TrackingStatusReadOnlyKey);
                });
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
