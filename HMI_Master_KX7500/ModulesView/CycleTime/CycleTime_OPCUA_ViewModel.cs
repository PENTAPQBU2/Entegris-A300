using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Models;
using HMI_Master_KX7500.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.ModulesView.CycleTime
{
    public class CycleTime_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<string> cycleTimeTagList = new List<string>();
        //private OPCUA_ opcUa = new OPCUA_();

        private KeyenceDll keyenceDll = null;
        #endregion

        #region PublicProperties
        private CycleTimeInterfaceModel cycleTimeInterfaceModel = new CycleTimeInterfaceModel();
        public CycleTimeInterfaceModel CycleTimeInterfaceModel
        {
            get { return cycleTimeInterfaceModel; }
            set
            {
                if (cycleTimeInterfaceModel != value)
                {
                    cycleTimeInterfaceModel = value;
                    RaisePropertyChanged(nameof(CycleTimeInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public CycleTime_OPCUA_ViewModel()
        {
            initializeMessenger();

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, "8500", DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~CycleTime_OPCUA_ViewModel()
        {

        }

        private void Update()
        {
            if (Global.PageList["Cycle Time"])
            {
                foreach (var x in CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList)
                {
                    x.CycleTimeTagModel.CycleTimeEnableTag = keyenceDll.Read<bool>("MR3500").ToString();
                    x.CycleTimeTagModel.CycleTimeNameTag = keyenceDll.Read<string>("DM3548", 20);
                    x.CycleTimeTagModel.CycleTimeCaptureTag = keyenceDll.Read<bool>("MR3501").ToString();
                    x.CycleTimeTagModel.CycleTimeStartTag = keyenceDll.Read<bool>("MR3502").ToString();

                    foreach (var y in x.CycleTimeNumericStatusList)
                    {
                        switch (y.CycleTimeNumericStatusKey)
                        {
                            case "CurrentTimer":
                                y.CycleTimeNumericStatus = keyenceDll.Read<float>("DM3500");
                                break;
                            case "LastTimer":
                                y.CycleTimeNumericStatus = keyenceDll.Read<float>("DM3502");
                                break;
                        }
                    }

                    foreach (var y in x.CycleTimeHistoryModel.CycleTimeHistoryTypeList)
                    {
                        foreach (var z in y.CycleTimeHistoryTypeTimerList)
                        {
                            z.CycleTimeHistoryTypeTimer = keyenceDll.Read<float>(z.CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag);
                            //$"DM{cycleTimeValue + (i * 2)}"
                        }
                    }
                }
            }           
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            initializeCycleTime();
            //Messenger.Default.Register<CycleTimeInitializeModel>(this, initializeCycleTime);
            //Messenger.Default.Register<OPCUA_>(this, initializeOpcUa);
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        //private void initializeOpcUa(OPCUA_ opcUa_)
        //{
        //    if (opcUa_.OPC.Module.Contains(OPCModules.CycleTime))
        //    {
        //        opcUa = opcUa_;
        //        cycleTimeTagList = opcUa.GetAddresses();
        //    }

        //    //tagList = CycleTimeService.GetCycleTimeTagList(CycleTimeInterfaceModel.CycleTimeMainModel);

        //    //if(Global.MachineConfigModel.InitializeOPCUA)
        //    //{
        //    //    opcUa.InitializeUAOPC(Global.OPCUA_Url, tagList, new List<OPCModules> { OPCModules.CycleTime});
        //    //}
        //}
        #endregion

        #region PrivateMessengerMethods
        private void initializeCycleTime(/*CycleTimeInitializeModel cycleTimeInitializeModel*/)
        {
            CycleTimeInterfaceModel.CycleTimeMainModel = CycleTimeService.DeserializeCycleTime();
            //initializeOpcUa();
        }

        //private void opcUpdate(_OPC opc_)
        //{
        //    if (opc_.Module.Contains(OPCModules.CycleTime))
        //    {
        //        opc_.Datachange.ToList().ForEach(x =>
        //        {
        //            Task.Run(() =>
        //            {
        //                try
        //                {
        //                    if (CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.Exists(
        //                        y => y.CycleTimeTagModel.CycleTimeEnableTag == x.Key))
        //                    {
        //                        CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.Find(
        //                            y => y.CycleTimeTagModel.CycleTimeEnableTag == x.Key).CycleTimeEnable = Convert.ToBoolean(x.Value);
        //                    }
        //                    else if (CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.Exists(
        //                        y => y.CycleTimeTagModel.CycleTimeNameTag == x.Key))
        //                    {
        //                        CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.Find(
        //                            y => y.CycleTimeTagModel.CycleTimeNameTag == x.Key).CycleTimeKey = x.Value;
        //                    }
        //                    else if (CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.Exists(
        //                        y => y.CycleTimeTagModel.CycleTimeCaptureTag == x.Key))
        //                    {
        //                        if (Convert.ToBoolean(x.Value))
        //                        {
        //                            opcUa.Write(x.Key, false);

        //                            CycleTimeService.GenerateCycleTimeCsv(CycleTimeInterfaceModel.CycleTimeMainModel, x.Key);
        //                        }
        //                    }
        //                    else if (CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.SelectMany(
        //                        y => y.CycleTimeNumericStatusList).ToList().Exists(
        //                        y => y.CycleTimeNumericStatusTagModel.CycleTimeNumericStatusTag == x.Key))
        //                    {
        //                        CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.SelectMany(
        //                            y => y.CycleTimeNumericStatusList).ToList().Find(
        //                            y => y.CycleTimeNumericStatusTagModel.CycleTimeNumericStatusTag == x.Key).CycleTimeNumericStatus = Convert.ToDouble(x.Value);
        //                    }
        //                    else if (CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.SelectMany(
        //                        y => y.CycleTimeHistoryModel.CycleTimeHistoryTypeList).SelectMany(
        //                        y => y.CycleTimeHistoryTypeTimerList).ToList().Exists(
        //                        y => y.CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag == x.Key))
        //                    {
        //                        CycleTimeInterfaceModel.CycleTimeMainModel.CycleTimeList.SelectMany(
        //                            y => y.CycleTimeHistoryModel.CycleTimeHistoryTypeList).SelectMany(
        //                            y => y.CycleTimeHistoryTypeTimerList).ToList().Find(
        //                            y => y.CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag == x.Key).CycleTimeHistoryTypeTimer = Convert.ToDouble(x.Value);
        //                    }
        //                }
        //                catch (FormatException)
        //                {
        //                    logger.Error(nameof(FormatException));
        //                    logger.Error($"CycleTime_opcUpdate, {x.Key}, {x.Value}");
        //                }
        //            });
        //        });
        //    }
                
        //}
        #endregion
    }
}
