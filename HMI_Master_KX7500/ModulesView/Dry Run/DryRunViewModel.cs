using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.DryRun.Action;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMI_Master_KX7500.Model.DryRun
{
    public class DryRunViewModel : ViewModelBase
    {
        #region PrivateFields
        private List<string> dryRunTagList = new List<string>();
        private KeyenceDll keyenceDll = null;

        //private readonly DryRunService dryRunService = new DryRunService();
        //private OPCUA_ opcUa = new OPCUA_();
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
        public DryRunViewModel()
        {
            initializeMessenger();

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, "8500", DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~DryRunViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            int enableDryRunValue = 3908; //4000 - 92
            int dryRunNameValue = 2980; //3000 - 20
            int dryRunStartValue = 3909; //4001 - 92
            int dryRunResetValue = 3910; //4002 - 92
            int dryRun_SoftJamCountValue = 2992; //3012 - 20
            int dryRun_HardJamCountValue = 2993; //3013 - 20
            int dryRun_RunTimeValue = 2994; //3014 - 20
            int dryRun_IdlingTimeValue = 2996; //3016 - 20
            int dryRun_DownTimeValue = 2998; //3018 - 20

            if (Global.PageList["Dry Run"])
            {
                int count = 1;
                foreach (var x in DryRunInterfaceModel.DryRunList)
                {
                    //int count = 1;

                    if (count % 2 == 0)
                    {
                        enableDryRunValue += 8;
                    }
                    else
                    {
                        enableDryRunValue += 92;
                    }
                    string enableDryRunNode = $"MR{enableDryRunValue}";
                    string dryRunName = $"DM{dryRunNameValue + (count * 20)}";
                    if (count % 2 == 0)
                    {
                        dryRunStartValue += 8;
                    }
                    else
                    {
                        dryRunStartValue += 92;
                    }
                    string dryRunStart = $"MR{dryRunStartValue}";
                    if (count % 2 == 0)
                    {
                        dryRunResetValue += 8;
                    }
                    else
                    {
                        dryRunResetValue += 92;
                    }
                    string dryRunReset = $"MR{dryRunResetValue}";
                    string dryRun_SoftJamCount = $"DM{dryRun_SoftJamCountValue + (count * 20)}";
                    string dryRun_HardJamCount = $"DM{dryRun_HardJamCountValue + (count * 20)}";
                    string dryRun_RunTime = $"DM{dryRun_RunTimeValue + (count * 20)}";
                    string dryRun_IdlingTime = $"DM{dryRun_IdlingTimeValue + (count * 20)}";
                    string dryRun_DownTime = $"DM{dryRun_DownTimeValue + (count * 20)}";

                    x.DryRunEnable = keyenceDll.Read<bool>(enableDryRunNode);
                    x.DryRunName = keyenceDll.Read<string>(dryRunName, 20);
                    x.DryRunStart = keyenceDll.Read<bool>(dryRunStart).ToString();
                    x.DryRunKey = keyenceDll.Read<string>(dryRunName, 20);

                    foreach (var y in x.DryRunTimeList)
                    {
                        switch (y.DryRunTimeKey)
                        {
                            case "RunTime":
                                y.DryRunTime = keyenceDll.Read<int>(dryRun_RunTime).ToString();
                                break;
                            case "IdleTime":
                                y.DryRunTime = keyenceDll.Read<int>(dryRun_IdlingTime).ToString();
                                break;
                            case "DownTime":
                                y.DryRunTime = keyenceDll.Read<int>(dryRun_DownTime).ToString();
                                break;
                        }
                    }
                    foreach (var y in x.DryRunCountList)
                    {
                        switch (y.DryRunCountKey)
                        {
                            case "SoftJam":
                                y.DryRunCount = keyenceDll.Read<short>(dryRun_SoftJamCount).ToString();
                                break;
                            case "HardJam":
                                y.DryRunCount = keyenceDll.Read<short>(dryRun_HardJamCount).ToString();
                                break;
                        }
                    }
                    count++;
                }
            }
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
            initializeDryRunList();
            initializeCommand();
        }

        //private void initializeopcUa(OPCUA_ opcUa_)
        //{
        //    if (opcUa_.OPC.Module.Contains(OPCModules.DryRun)){
        //        opcUa = opcUa_;
        //        dryRunTagList = opcUa.GetAddresses();
        //    }

        //    //dryRunTagList = DryRunService.GetDryRunTagList(DryRunInterfaceModel.DryRunList);

        //    //if (Global.MachineConfigModel.InitializeOPCUA)
        //    //{
        //    //    opcUA.InitializeUAOPC(Global.OPCUA_Url, dryRunTagList);
        //    //}
        //}
        #endregion

        #region PrivateCommandMethods
        private void dryRunStartCommand(DryRunModel dryRunModel)
        {
            keyenceDll.Write<bool>(dryRunModel.DryRunTagModel.DryRunStartTag, true);
        }

        private void dryRunStopCommand(DryRunModel dryRunModel)
        {
            keyenceDll.Write<bool>(dryRunModel.DryRunTagModel.DryRunStartTag, false);
        }

        private void dryRunActionCommand(DryRunActionModel dryRunActionModel)
        {
            keyenceDll.Write<bool>(dryRunActionModel.DryRunActionTagModel.DryRunActionTag, true);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeDryRunList(/*DryRunInitializeModel dryRunInitializeModel*/)
        {
            DryRunInterfaceModel.DryRunList = DryRunService.DeserializeDryRunList();
            DryRunInterfaceModel.DryRunList.ForEach(x =>
            {
                x.DryRunActionList.ToList().ForEach(y =>
                {
                    y.DryRunActionCommand = new RelayCommand<DryRunActionModel>(dryRunActionCommand);
                });
            });

            //initializeOPC_UA();

            //Messenger.Default.Register<OPCUA_>(this, initializeopcUa);
        }

        //private void opcUpdate(_OPC opc_)
        //{
        //    if (opc_.Module.Contains(OPCModules.DryRun))
        //    {
        //        opc_.Datachange.ToList().ForEach(y =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if (DryRunInterfaceModel.DryRunList.Exists(x => x.DryRunTagModel.DryRunEnableTag == y.Key))
        //                {
        //                    DryRunInterfaceModel.DryRunList.Find(x => x.DryRunTagModel.DryRunEnableTag == y.Key).DryRunEnable = Convert.ToBoolean(y.Value);
        //                }
        //                else if (DryRunInterfaceModel.DryRunList.Exists(x => x.DryRunTagModel.DryRunNameTag == y.Key))
        //                {
        //                    DryRunInterfaceModel.DryRunList.Find(x => x.DryRunTagModel.DryRunNameTag == y.Key).DryRunName = y.Value;
        //                }
        //                else if (DryRunInterfaceModel.DryRunList.Exists(x => x.DryRunTagModel.DryRunStartTag == y.Key))
        //                {
        //                    DryRunInterfaceModel.DryRunList.Find(x => x.DryRunTagModel.DryRunStartTag == y.Key).DryRunStart = y.Value;
        //                }
        //                else if (DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunTimeList).ToList().Exists(x => x.DryRunTimeTagModel.DryRunTimeTag == y.Key))
        //                {
        //                    DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunTimeList).ToList().Find(x => x.DryRunTimeTagModel.DryRunTimeTag == y.Key).DryRunTime = y.Value;
        //                }
        //                else if (DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunCountList).ToList().Exists(x => x.DryRunCountTagModel.DryRunCountTag == y.Key))
        //                {
        //                    DryRunInterfaceModel.DryRunList.SelectMany(z => z.DryRunCountList).ToList().Find(x => x.DryRunCountTagModel.DryRunCountTag == y.Key).DryRunCount = y.Value;
        //                }
        //            });
        //        });
        //    }
        //}
        #endregion
    }
}
