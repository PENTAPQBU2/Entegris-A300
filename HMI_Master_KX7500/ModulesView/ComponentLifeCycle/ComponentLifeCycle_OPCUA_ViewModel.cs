using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Services;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.ComponentLifeCycle
{
    public class ComponentLifeCycle_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private readonly ComponentLifeCycleService componentLifeCycleService = new ComponentLifeCycleService();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<string> componentLifeCycleTagList = new List<string>();
        private OPCUA_ opcUa = null;
        #endregion

        #region PublicProperties
        private ComponentLifeCycleInterfaceModel componentLifeCycleInterfaceModel = new ComponentLifeCycleInterfaceModel();
        public ComponentLifeCycleInterfaceModel ComponentLifeCycleInterfaceModel
        {
            get { return componentLifeCycleInterfaceModel; }
            set
            {
                if (componentLifeCycleInterfaceModel != value)
                {
                    componentLifeCycleInterfaceModel = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public ComponentLifeCycle_OPCUA_ViewModel()
        {
            initializeMessenger();
            //Messenger.Default.Send(new ComponentLifeCycleInitializeModel
            //{
            //    ComponentLifeCycleFilePath = FilePath.ComponentLifeCycle
            //});
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            initializeComponentLifeCycleList();
            //Messenger.Default.Register<ComponentLifeCycleInitializeModel>(this, initializeComponentLifeCycleList);
            Messenger.Default.Register<_OPC>(this, siemensOPCUpdate);
        }

        private void initializeopcUa(OPCUA_ opcUa_)
        {
            if (opcUa_.OPC.Module.Contains(OPCModules.ComponentLifeCycle))
            {
                opcUa = opcUa_;
                componentLifeCycleTagList = opcUa.GetAddresses();
            }
            //componentLifeCycleTagList = ComponentLifeCycleService.GetComponentLifeCycleTagList(
            //        ComponentLifeCycleInterfaceModel.ComponentLifeCycleList);

            //if (Global.MachineConfigModel.InitializeOPCUA)
            //{
            //    opcUa.InitializeUAOPC(Global.OPCUA_Url, componentLifeCycleTagList);
            //}
        }
        #endregion

        #region PrivateCommandMethods
        private void resetComponentLifeCycleCountCommand(ComponentLifeCycleModel componentLifeCycleModel)
        {
            if (MessageBox.Show($"Reset {componentLifeCycleModel.ComponentLifeCycleName}'s Count ?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                opcUa.Write(componentLifeCycleModel.ComponentLifeCycleTagModel.ComponentLifeCycleCountResetTag, true);
            }
        }

        private void setComponentLifeCycleLimitCommand(ComponentLifeCycleModel componentLifeCycleModel)
        {
            if (MessageBox.Show($"Set {componentLifeCycleModel.ComponentLifeCycleName}'s Limit ?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                opcUa.Write(componentLifeCycleModel.ComponentLifeCycleTagModel.ComponentLifeCycleLimitTag, componentLifeCycleModel.ComponentLifeCycleLimit);
            }
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeComponentLifeCycleList(/*ComponentLifeCycleInitializeModel componentLifeCycleInitializeModel*/)
        {
            ComponentLifeCycleInterfaceModel.ComponentLifeCycleList = ComponentLifeCycleService.DeserializeComponentLifeCycleopcUaList();
            ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.ForEach(x =>
            {
                x.ResetComponentLifeCycleCountCommand = new RelayCommand<ComponentLifeCycleModel>(
                        resetComponentLifeCycleCountCommand);
                x.SetComponentLifeCycleLimitCommand = new RelayCommand<ComponentLifeCycleModel>(
                    setComponentLifeCycleLimitCommand);
            });
            //initializeopcUa();
            Messenger.Default.Register<OPCUA_>(this, initializeopcUa);
        }

        private void siemensOPCUpdate(_OPC opc_)
        {
            if (opc_.Module.Contains(OPCModules.ComponentLifeCycle)) // <-check this
            {
                opc_.Datachange.ToList().ForEach(x =>
                {
                    Task.Run(() =>
                    {
                        if (ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Exists(
                                y => y.ComponentLifeCycleTagModel.ComponentLifeCycleEnableTag == x.Key))
                        {
                            ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Find(
                                y => y.ComponentLifeCycleTagModel.ComponentLifeCycleEnableTag == x.Key).ComponentLifeCycleEnable = Convert.ToBoolean(x.Value);
                        }
                        else if (ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Exists(
                            y => y.ComponentLifeCycleTagModel.ComponentLifeCycleNameTag == x.Key))
                        {
                            ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Find(
                                y => y.ComponentLifeCycleTagModel.ComponentLifeCycleNameTag == x.Key).ComponentLifeCycleKey = x.Value;
                        }
                        else if (ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Exists(
                            y => y.ComponentLifeCycleTagModel.ComponentLifeCycleCountTag == x.Key))
                        {
                            ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Find(
                                y => y.ComponentLifeCycleTagModel.ComponentLifeCycleCountTag == x.Key).ComponentLifeCycleCount = x.Value;
                        }
                        else if (ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Exists(
                            y => y.ComponentLifeCycleTagModel.ComponentLifeCycleLimitTag == x.Key))
                        {
                            ComponentLifeCycleInterfaceModel.ComponentLifeCycleList.Find(
                                y => y.ComponentLifeCycleTagModel.ComponentLifeCycleLimitTag == x.Key).ComponentLifeCycleLimit = x.Value;
                        }
                    });
                });
            }
        }
        #endregion
    }
}
