using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.DryRun.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Model.DryRun
{
    public class DryRun_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private readonly DryRunService dryRunService = new DryRunService();
        private List<string> dryRunTagList = new List<string>();
        private OPCUA_ opcUa = new OPCUA_();
        #endregion

        #region PublicProperties
        private DryRunInterfaceModel dryRunInterfaceModel = new DryRunInterfaceModel();
        public DryRunInterfaceModel DryRunInterfaceModel
        {
            get { return dryRunInterfaceModel; }
            set
            {
                if (dryRunInterfaceModel != value)
                {
                    dryRunInterfaceModel = value;
                    RaisePropertyChanged(nameof(DryRunInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public DryRun_OPCUA_ViewModel()
        {
            initializeMessenger();
            //Messenger.Default.Send(new DryRunInitializeModel
            //{
            //    DryRunFilePath = FilePath.DryRun
            //});
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            DryRunInterfaceModel.DryRunStartCommand = new RelayCommand<DryRunModel>(dryRunStartCommand);
            DryRunInterfaceModel.DryRunStopCommand = new RelayCommand<DryRunModel>(dryRunStopCommand);
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<DryRunInitializeModel>(this, initializeDryRunList);
            initializeDryRunList();
            initializeCommand();
            Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        private void initializeopcUa(OPCUA_ opcUa_)
        {
            if (opcUa_.OPC.Module.Contains(OPCModules.DryRun)){
                opcUa = opcUa_;
                dryRunTagList = opcUa.GetAddresses();
            }
            
            //dryRunTagList = DryRunService.GetDryRunTagList(DryRunInterfaceModel.DryRunList);

            //if (Global.MachineConfigModel.InitializeOPCUA)
            //{
            //    opcUA.InitializeUAOPC(Global.OPCUA_Url, dryRunTagList);
            //}
        }
        #endregion

        #region PrivateCommandMethods
        private void dryRunStartCommand(DryRunModel dryRunModel)
        {
            opcUa.Write(dryRunModel.DryRunTagModel.DryRunStartTag, true);
        }

        private void dryRunStopCommand(DryRunModel dryRunModel)
        {
            opcUa.Write(dryRunModel.DryRunTagModel.DryRunStartTag, false);
        }

        private void dryRunActionCommand(DryRunActionModel dryRunActionModel)
        {
            opcUa.Write(dryRunActionModel.DryRunActionTagModel.DryRunActionTag, true);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeDryRunList(/*DryRunInitializeModel dryRunInitializeModel*/)
        {
            DryRunInterfaceModel.DryRunList = DryRunService.DeserializeDryRunSiemensOPCList();
            DryRunInterfaceModel.DryRunList.ForEach(x =>
            {
                x.DryRunActionList.ToList().ForEach(y =>
                {
                    y.DryRunActionCommand = new RelayCommand<DryRunActionModel>(dryRunActionCommand);
                });
            });
            //initializeOPC_UA();
            Messenger.Default.Register<OPCUA_>(this, initializeopcUa);
        }

        private void opcUpdate(_OPC opc_)
        {
            if (opc_.Module.Contains(OPCModules.DryRun))
            {
                opc_.Datachange.ToList().ForEach(y =>
                {
                    Task.Run(() =>
                    {
                        if (DryRunInterfaceModel.DryRunList.Exists(x => x.DryRunTagModel.DryRunEnableTag == y.Key))
                        {
                            DryRunInterfaceModel.DryRunList.Find(x => x.DryRunTagModel.DryRunEnableTag == y.Key).DryRunEnable = Convert.ToBoolean(y.Value);
                        }
                        else if (DryRunInterfaceModel.DryRunList.Exists(x => x.DryRunTagModel.DryRunNameTag == y.Key))
                        {
                            DryRunInterfaceModel.DryRunList.Find(x => x.DryRunTagModel.DryRunNameTag == y.Key).DryRunName = y.Value;
                        }
                        else if (DryRunInterfaceModel.DryRunList.Exists(x => x.DryRunTagModel.DryRunStartTag == y.Key))
                        {
                            DryRunInterfaceModel.DryRunList.Find(x => x.DryRunTagModel.DryRunStartTag == y.Key).DryRunStart = y.Value;
                        }
                        else if (DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunTimeList).ToList().Exists(x => x.DryRunTimeTagModel.DryRunTimeTag == y.Key))
                        {
                            DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunTimeList).ToList().Find(x => x.DryRunTimeTagModel.DryRunTimeTag == y.Key).DryRunTime = y.Value;
                        }
                        else if (DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunCountList).ToList().Exists(x => x.DryRunCountTagModel.DryRunCountTag == y.Key))
                        {
                            DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunCountList).ToList().Find(x => x.DryRunCountTagModel.DryRunCountTag == y.Key).DryRunCount = y.Value;
                        }
                    });
                });
            }
        }
        #endregion
    }
}
