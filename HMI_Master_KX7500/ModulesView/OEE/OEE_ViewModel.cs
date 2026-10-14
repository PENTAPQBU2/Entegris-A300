using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMI_Master_KX7500.Model.OEE
{
    public class OEE_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private Dictionary<string, string> oEETagList = new Dictionary<string, string>();
        private KeyenceDll keyenceDll = null;
        #endregion PrivateFields

        #region PublicProperties

        private OEEInterfaceModel oEEInterfaceModel = new OEEInterfaceModel();

        public OEEInterfaceModel OEEInterfaceModel
        {
            get { return oEEInterfaceModel; }
            set
            {
                if(oEEInterfaceModel != value)
                {
                    oEEInterfaceModel = value;
                    RaisePropertyChanged(nameof(OEEInterfaceModel));
                }
            }
        }

        #endregion PublicProperties

        #region Constructor

        public OEE_ViewModel()
        {
            initializeMessenger();

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~OEE_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            if (Global.PageList["OEE"])
            {
                OEEInterfaceModel.OEEModel.OEEDataList.AsParallel().ForAll(x =>
                {
                    if (x.OEEDataKey == "LotOEE")
                    {
                        foreach (var y in x.OEECountList)
                        {
                            switch (y.OEECountKey)
                            {
                                case "TotalQuantityIn":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Lot_OEE_Total_Quantity_In);
                                    break;
                                case "TotalQuantityOut":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Lot_OEE_Total_Quantity_Out);
                                    break;
                                case "TotalPassed":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Lot_OEE_Total_Passed);
                                    break;
                                case "TotalFailed":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Lot_OEE_Total_Failed);
                                    break;
                                case "TotalYield":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Lot_OEE_Total_Yield);
                                    break;
                                case "SoftJam":
                                    y.OEECount = keyenceDll.Read<short>(PLCTag.Lot_OEE_Soft_Jam);
                                    break;
                                case "HardJam":
                                    y.OEECount = keyenceDll.Read<short>(PLCTag.Lot_OEE_Hard_Jam);
                                    break;
                            }
                        }
                        foreach (var y in x.OEETimeList)
                        {
                            switch (y.OEETimeKey)
                            {
                                case "SystemUpTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Lot_OEE_System_Up_Time).ToString();
                                    break;
                                case "OperationTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Lot_OEE_Operation_Time).ToString();
                                    break;
                                case "DownTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Lot_OEE_Down_Time).ToString();
                                    break;
                                case "IdleTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Lot_OEE_Idle_Time).ToString();
                                    break;
                                case "MTBF":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Lot_OEE_MTBF).ToString();
                                    break;
                                case "MTBA":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Lot_OEE_MTBA).ToString();
                                    break;
                            }
                        }
                        foreach (var y in x.OEEReadOnlyList)
                        {
                            switch (y.OEEReadOnlyKey)
                            {
                                case "LotID":
                                    y.OEEReadOnly = keyenceDll.Read<string>(PLCTag.Lot_OEE_New_Lot_ID_String, 50);
                                    break;
                                case "Operator_ID":
                                    y.OEEReadOnly = keyenceDll.Read<string>(PLCTag.Lot_OEE_Operator_ID, 10);
                                    break;
                            }
                        }
                    }
                    else if (x.OEEDataKey == "ShiftOEE")
                    {
                        foreach (var y in x.OEECountList)
                        {
                            switch (y.OEECountKey)
                            {
                                case "TotalQuantityIn":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Shift_OEE_Total_Quantity_In);
                                    break;
                                case "TotalQuantityOut":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Shift_OEE_Total_Quantity_Out);
                                    break;
                                case "TotalPassed":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Shift_OEE_Total_Passed);
                                    break;
                                case "TotalFailed":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Shift_OEE_Total_Failed);
                                    break;
                                case "TotalYield":
                                    y.OEECount = keyenceDll.Read<int>(PLCTag.Shift_OEE_Total_Yield);
                                    break;
                                case "SoftJam":
                                    y.OEECount = keyenceDll.Read<short>(PLCTag.Shift_OEE_Soft_Jam);
                                    break;
                                case "HardJam":
                                    y.OEECount = keyenceDll.Read<short>(PLCTag.Shift_OEE_Hard_Jam);
                                    break;
                            }
                        }
                        foreach (var y in x.OEETimeList)
                        {
                            switch (y.OEETimeKey)
                            {
                                case "SystemUpTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Shift_OEE_System_Up_Time).ToString();
                                    break;
                                case "OperationTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Shift_OEE_Operation_Time).ToString();
                                    break;
                                case "DownTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Shift_OEE_Down_Time).ToString();
                                    break;
                                case "IdleTime":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Shift_OEE_Idle_Time).ToString();
                                    break;
                                case "MTBF":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Shift_OEE_MTBF).ToString();
                                    break;
                                case "MTBA":
                                    y.OEETime = keyenceDll.Read<int>(PLCTag.Shift_OEE_MTBA).ToString();
                                    break;
                            }
                        }
                    }
                });

                OEEInterfaceModel.OEEModel.OEEHourlyUphList.AsParallel().ForAll(x =>
                {
                    switch (x.OEEHourlyUphKey)
                    {
                        case "12am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_0);
                            break;
                        case "1am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_1);
                            break;
                        case "2am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_2);
                            break;
                        case "3am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_3);
                            break;
                        case "4am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_4);
                            break;
                        case "5am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_5);
                            break;
                        case "6am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_6);
                            break;
                        case "7am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_7);
                            break;
                        case "8am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_8);
                            break;
                        case "9am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_9);
                            break;
                        case "10am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_10);
                            break;
                        case "11am":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_11);
                            break;
                        case "12pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_12);
                            break;
                        case "1pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_13);
                            break;
                        case "2pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_14);
                            break;
                        case "3pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_15);
                            break;
                        case "4pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_16);
                            break;
                        case "5pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_17);
                            break;
                        case "6pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_18);
                            break;
                        case "7pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_19);
                            break;
                        case "8pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_20);
                            break;
                        case "9pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_21);
                            break;
                        case "10pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_22);
                            break;
                        case "11pm":
                            x.OEEHourlyUph = keyenceDll.Read<int>(PLCTag.UPH_Hourly_23);
                            break;
                    }
                });

                //=================================================

                OEEInterfaceModel.OEEModel.OEEDataList.AsParallel().ForAll(x =>
                {
                    if (!string.IsNullOrEmpty(x.OEEDataTagModel.OEEGenerateCsvTag))
                    {
                        bool bolOEEGenerateCSV_Value = keyenceDll.Read<bool>(x.OEEDataTagModel.OEEGenerateCsvTag);
                        if (bolOEEGenerateCSV_Value)
                        {
                            OEEService.GenerateOEECsv(OEEInterfaceModel.OEEModel.OEEDataList, x.OEEDataTagModel.OEEGenerateCsvTag);
                            keyenceDll.Write<bool>(x.OEEDataTagModel.OEEGenerateCsvTag, false);
                        }
                    }
                });

            }         
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeMessenger()
        {
            initializeOEE();
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        //private void InitializeKX7500()
        //{
        //    //if (opcUa_.OPC.Module.Contains(OPCModules.OEE))
        //    //{
        //    //    opcUa = opcUa_;
        //    //    oEETagList = opcUa.GetAddresses();
        //    //}
        //    List<OPCModules> module = new List<OPCModules>() { OPCModules.OEE };

        //    //oEETagList = OEEService.GetOEETagList();
        //    oEETagList = OEEService.GetOEETagDictionary();

        //    if(Global.MachineConfigModel.InitializeKX7500)
        //    {
        //        keyenceDll = new KeyenceDll(oEETagList, Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, module);
        //        Messenger.Default.Send(keyenceDll);
        //    }
        //}

        #endregion PrivateInitializeMethods

        #region PrivateMessengerMethods

        private void initializeOEE()
        {
            OEEInterfaceModel.OEEModel = OEEService.DeserializeOEESiemensOPC();
        }


        //-----------------------------------------------------------
        //private void opcUpdate(_OPC opc_)
        //{
        //    if (opc_.Module.Contains(OPCModules.OEE))
        //    //if (oEETagList.SequenceEqual(opc_.TagList))
        //    {
        //        opc_.DataChange.ToList().ForEach(y =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if (OEEInterfaceModel.OEEModel.OEEHourlyUphList.ToList().Exists(
        //                        x => x.OEEHourlyUphTagModel.OEEHourlyUphTag ==
        //                        y.Key))
        //                {
        //                    OEEInterfaceModel.OEEModel.OEEHourlyUphList.ToList().Find(
        //                        x => x.OEEHourlyUphTagModel.OEEHourlyUphTag ==
        //                        y.Key).OEEHourlyUph =
        //                        Convert.ToDouble(y.Value);
        //                }
        //                else if (OEEInterfaceModel.OEEModel.OEEDataList.ToList().Exists(
        //                    x => x.OEEDataTagModel.OEEGenerateCsvTag ==
        //                    y.Key))
        //                {
        //                    if (Convert.ToBoolean(y.Value))
        //                    {
        //                        keyenceDll.Writedevice7KR(
        //                            y.Key, 0);

        //                        OEEService.GenerateOEECsv(OEEInterfaceModel.OEEModel.OEEDataList,
        //                            y.Key);
        //                    }
        //                }
        //                else if (OEEInterfaceModel.OEEModel.OEEDataList.SelectMany(
        //                    x => x.OEEReadOnlyList).ToList().Exists(x => x.OEEReadOnlyTagModel.OEEReadOnlyTag ==
        //                    y.Key))
        //                {
        //                    OEEInterfaceModel.OEEModel.OEEDataList.SelectMany(
        //                        x => x.OEEReadOnlyList).ToList().Find(x => x.OEEReadOnlyTagModel.OEEReadOnlyTag ==
        //                        y.Key).OEEReadOnly =
        //                        y.Value;
        //                }
        //                else if (OEEInterfaceModel.OEEModel.OEEDataList.SelectMany(
        //                    x => x.OEETimeList).ToList().Exists(x => x.OEETimeTagModel.OEETimeTag ==
        //                    y.Key))
        //                {
        //                    OEEInterfaceModel.OEEModel.OEEDataList.SelectMany(
        //                        x => x.OEETimeList).ToList().Find(x => x.OEETimeTagModel.OEETimeTag ==
        //                        y.Key).OEETime =
        //                        y.Value;
        //                }
        //                else if (OEEInterfaceModel.OEEModel.OEEDataList.SelectMany(
        //                    x => x.OEECountList).ToList().Exists(x => x.OEECountTagModel.OEECountTag ==
        //                    y.Key))
        //                {
        //                    OEEInterfaceModel.OEEModel.OEEDataList.SelectMany(
        //                        x => x.OEECountList).ToList().Find(x => x.OEECountTagModel.OEECountTag ==
        //                        y.Key).OEECount =
        //                        Convert.ToDouble(y.Value);
        //                }
        //            });
        //        });
        //    }
        //}

        #endregion PrivateMessengerMethods
    }
}