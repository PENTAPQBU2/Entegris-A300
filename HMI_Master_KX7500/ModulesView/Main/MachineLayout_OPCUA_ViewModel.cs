using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Interface;
using HMI_Master_KX7500.Models.KeyencePLC;
//using HMI_Master_KX7500.Service.Main;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.ModulesView.Main
{
    public class MachineLayout_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private Logger logger = LogManager.GetCurrentClassLogger();
        private List<KeyencePLCTagModel> machineLayoutTagList = new List<KeyencePLCTagModel>();
        private KeyenceDll  keyenceDll = new KeyenceDll();
        #endregion

        #region PublicProperties
        private MachineLayoutInterfaceModel machineLayoutInterfaceModel = new MachineLayoutInterfaceModel();
        public MachineLayoutInterfaceModel MachineLayoutInterfaceModel
        {
            get { return machineLayoutInterfaceModel; }
            set
            {
                if (machineLayoutInterfaceModel != value)
                {
                    machineLayoutInterfaceModel = value;
                    RaisePropertyChanged(nameof(MachineLayoutInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public MachineLayout_OPCUA_ViewModel()
        {
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            //initializeMachineLayout();
            Messenger.Default.Register<KeyenceDll>(this, initializeopcUa);
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        private void initializeMachineLayout()
        {
            //MachineLayoutInterfaceModel.MachineLayoutModel = MachineLayoutService.DeserializeMachineLayout();
            //MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutToggleList.ForEach(x =>
            //{
            //    x.MachineLayoutToggleCommand = new GalaSoft.MvvmLight.Command.RelayCommand<MachineLayoutToggleModel>(machineLayoutToggleCommand);
            //});
        }

        private void initializeopcUa(KeyenceDll  _keyenceDll )
        {
            if (_keyenceDll.Module.Contains(OPCModules.MachineLayout))
            {
                keyenceDll = _keyenceDll;
                //machineLayoutTagList = keyenceDll.GetAddresses();
            }
        }
        #endregion

        #region PrivateMessengerMethods
        //private void opcUpdate(_OPC opc_)
        //{
        //    if (opc_.Module.Contains(OPCModules.MachineLayout))
        //    {
        //        opc_.DataChange.ToList().ForEach(node =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if (node.Value != null)
        //                {
        //                    if (MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutModuleList.SelectMany(
        //                        x => x.MachineLayoutModuleStatusIndicatorList).ToList().Exists(
        //                        x => x.MachineLayoutModuleStatusIndicatorTagModel.MachineLayoutModuleStatusIndicatorTag == node.Key))
        //                    {
        //                        MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutModuleList.SelectMany(
        //                            x => x.MachineLayoutModuleStatusIndicatorList).ToList().Find(
        //                            x => x.MachineLayoutModuleStatusIndicatorTagModel.MachineLayoutModuleStatusIndicatorTag == node.Key).MachineLayoutModuleStatusIndicatorIndex = node.Value;

        //                        logger.Debug($"OPC Update Machine Layout Value: {node.Value}");
        //                        logger.Debug($"OPC Update Machine Layout Key: {node.Key}");
        //                    }


        //                    //else if (MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutToggleList.Exists(
        //                    //   x => x.MachineLayoutToggleTagModel.MachineLayoutToggleTag == node.Key))
        //                    //{
        //                    //    MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutToggleList.Find(
        //                    //   x => x.MachineLayoutToggleTagModel.MachineLayoutToggleTag == node.Key).MachineLayoutToggleValue = node.Value;
        //                    //}
        //                }

        //            });
        //        });
        //    }
        //}
        #endregion

        #region PrivateCommand
        //private void machineLayoutToggleCommand(MachineLayoutToggleModel machineLayoutToggleModel)
        //{
        //    bool val = bool.Parse(machineLayoutToggleModel.MachineLayoutToggleValue);
        //    opcUa.Write(machineLayoutToggleModel.MachineLayoutToggleTagModel.MachineLayoutToggleTag, val);
        //}
        #endregion
    }
}
