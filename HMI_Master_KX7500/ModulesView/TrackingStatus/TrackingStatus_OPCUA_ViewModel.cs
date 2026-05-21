using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Interface;
using HMI_Master_KX7500.Model.TrackingStatus.Module.Indicator;
using HMI_Master_KX7500.Service.TrackingStatus;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.TrackingStatus
{
    public class TrackingStatus_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        //private readonly TrackingStatusService trackingStatusService = new TrackingStatusService();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<string> trackingStatusTagList = new List<string>();
        //private static OPCModules moduleName = OPCModules.Settings;
        private OPCUA_ opcUa = new OPCUA_();
        #endregion

        #region PublicProperties
        private TrackingStatusInterfaceModel trackingStatusInterfaceModel = new TrackingStatusInterfaceModel();
        public TrackingStatusInterfaceModel TrackingStatusInterfaceModel
        {
            get { return trackingStatusInterfaceModel; }
            set
            {
                if (trackingStatusInterfaceModel != value)
                {
                    trackingStatusInterfaceModel = value;
                    RaisePropertyChanged(nameof(TrackingStatusInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public TrackingStatus_OPCUA_ViewModel()
        {
            try
            {
                initializeMessenger();
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            initializeTrackingStatus();
            Messenger.Default.Register<OPCUA_>(this, initializeOpc);
            //Messenger.Default.Register<TrackingStatusInitializeModel>(this, initializeTrackingStatus);
            Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        private void initializeOpc(OPCUA_ opcUa_)
        {
            try
            {
                if (opcUa_.OPC.Module.Contains(OPCModules.TrackingStatus))
                {
                    opcUa = opcUa_;
                    trackingStatusTagList = opcUa.GetAddresses();
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeTrackingStatus()
        {
            try
            {
                TrackingStatusInterfaceModel.TrackingStatusModel = TrackingStatusService.DeserializeTrackingStatus();
                TrackingStatusInterfaceModel.TrackingStatusModel.TrackingStatusModuleList.ForEach(x =>
                {
                    x.TrackingStatusModuleStatusIndicatorList.ForEach(y =>
                    {
                        y.trackingStatusTriggerCommand = new RelayCommand<TrackingStatusModuleStatusIndicatorModel>(setTrackingStatusIndicatorTrigger);
                    });
                });
            }
            catch (Exception ex) { logger.Error(ex); }
            //TrackingStatusInterfaceModel.TrackingStatusModel = TrackingStatusService.DeserializeTrackingStatus();
            ////initializeSiemensOPC();
            //TrackingStatusInterfaceModel.TrackingStatusModel.TrackingStatusModuleList.ForEach(x =>
            //{
            //    x.TrackingStatusModuleStatusIndicatorList.ForEach(y =>
            //    {
            //        y.trackingStatusTriggerCommand = new RelayCommand<TrackingStatusModuleStatusIndicatorModel>(setTrackingStatusIndicatorTrigger);
            //    });
            //});

        }

        private void opcUpdate(_OPC opc)
        {
            try
            {
                if (opc.Module.Contains(OPCModules.TrackingStatus))
                {
                    Task.Run(() =>
                    {
                        opc.Datachange.ToList().ForEach(node =>
                        {
                            if (TrackingStatusInterfaceModel.TrackingStatusModel.TrackingStatusModuleList.SelectMany(
                                x => x.TrackingStatusModuleStatusIndicatorList).ToList().Exists(
                                x => x.TrackingStatusModuleStatusIndicatorTagModel.TrackingStatusModuleStatusIndicatorTag ==
                                node.Key))
                            {
                                TrackingStatusInterfaceModel.TrackingStatusModel.TrackingStatusModuleList.SelectMany(
                                    x => x.TrackingStatusModuleStatusIndicatorList).ToList().Find(
                                    x => x.TrackingStatusModuleStatusIndicatorTagModel.TrackingStatusModuleStatusIndicatorTag ==
                                    node.Key).TrackingStatusModuleStatusIndicatorIndex =
                                    node.Value;
                            }
                            else if (TrackingStatusInterfaceModel.TrackingStatusModel.TrackingStatusReadOnlyGroupList.SelectMany(
                                x => x.TrackingStatusReadOnlyList).ToList().Exists(
                                x => x.TrackingStatusReadOnlyTagModel.TrackingStatusReadOnlyTag ==
                                node.Key))
                            {
                                TrackingStatusInterfaceModel.TrackingStatusModel.TrackingStatusReadOnlyGroupList.SelectMany(
                                x => x.TrackingStatusReadOnlyList).ToList().Find(
                                x => x.TrackingStatusReadOnlyTagModel.TrackingStatusReadOnlyTag ==
                                node.Key).TrackingStatusReadOnlyValue = node.Value;
                            }
                        });
                    });
                }
            } catch (Exception ex) { logger.Error(ex); }
        }

        private void setTrackingStatusIndicatorTrigger(TrackingStatusModuleStatusIndicatorModel trackingStatusModuleStatusIndicatorModel)
        {
            if (MessageBox.Show($"Are you sure you want to remove {trackingStatusModuleStatusIndicatorModel.TrackingStatusModuleStatusIndicatorName} ?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                opcUa.Write(trackingStatusModuleStatusIndicatorModel.TrackingStatusModuleStatusIndicatorTagModel.TrackingStatusModuleStatusIndicatorTriggerTag, true);
            }
            
        }
        #endregion
    }
}
