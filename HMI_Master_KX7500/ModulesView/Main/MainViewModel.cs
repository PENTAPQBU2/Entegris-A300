using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Interface;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.Main;
using HMI_Master_KX7500.Models.KeyencePLC;
using HMI_Master_KX7500.Service.Main;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.ModulesView.Main
{
    public class MainViewModel : ViewModelBase
    {
        #region PrivateFields
        private Dictionary<string, string> mainTagList = new Dictionary<string, string>();
        private KeyenceDll keyenceDll = null;
        #endregion PrivateFields

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
        #endregion PublicProperties

        #region Constructor

        public MainViewModel()
        {
            initializeMessenger();
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeMessenger()
        {
            //initializeLotInfo();
            //initializeGrippeYield();
            //initializeStationEnablestatus();
            //initializeMaterialInletType();
            initializeMachineLayout();
            //Messenger.Default.Register<KeyenceDll>(this, InitializeKX7500);
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
            //Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }

        //private void initializeLotInfo()
        //{
        //    try
        //    {
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "LOT_ID",
        //            MainTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "New_Lot_ID_String")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "OPERATOR_ID",
        //            MainTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "Operator_ID")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Customer_ID",
        //            MainTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "Customer_ID")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "RECIPE_ID",
        //            MainTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "Recipe_ID")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "RECIPE_NAME",
        //            MainTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "Current_Recipe_Name")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "LOT_QUANTITY",
        //            MainTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "Quantity")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "TotalQuantityIn",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Total_Output")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Loaded_Jig",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Loaded_Jig")

        //        });

        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Pass_Jig",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Pass_Jig")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Fail_Jig",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Fail_Jig")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Pass_Pallet",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Pass_Pallet")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Fail_Pallet",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Fail_Pallet")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Inserted_Pallet",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Inserted_Pallet")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Inserted_Jig",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Inserted_Jig")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Pin_Inserted",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Pin_Inserted")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Pin_Cut",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Pin_Cut")

        //        });
        //        listLotInfo.Add(new MainModel
        //        {
        //            MainKey = "Dieset_Punch_Count",
        //            MainTag = OPCUAHelper.CombinedNodeString(oEENode_NameSpace, "Dieset_Punch_Count")

        //        });

        //        LotInfoList = new ObservableCollection<MainModel>(listLotInfo);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        //private void initializeStationEnablestatus()
        //{
        //    try
        //    {
        //        listStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Pin Insert",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Insertion_Stage_Bypass_Flag.Bypass_Press_Pin")

        //        });
        //        listStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Vision Alignment",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Insertion_Stage_Bypass_Flag.Bypass_Vision_Alignment")

        //        });
        //        listStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Final Inspection",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Insertion_Stage_Bypass_Flag.Bypass_Final_Inspection")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Orientation Check",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Orientation_Check")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Barcode Scanner",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Barcode_Scanner")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Pin Bottom Vision",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Pin_Bottom_Vision")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Pin S-Curve Vision",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Pin_Front_Vision")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Bypass Pin Width Vision",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Pin_Side_Vision")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Stop Dieset Cut Pin",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Dieset_Cut_Pin")

        //        });
        //        listByPassStationEnable.Add(new MainModel
        //        {
        //            MainKey = "Stop Loose Pin Singulator",
        //            MainTag = OPCUAHelper.CombinedNodeString(ByPass_NameSpace, "Bypass_Loose_Pin_Singulator")

        //        });

        //        StationEnableList = new ObservableCollection<MainModel>(listStationEnable);
        //        listByPassStationEnable.ForEach(x => StationEnableList.Add(x));
        //        //StationEnableList = StationEnableList.Union(new ObservableCollection<MainModel>(listByPassStationEnable)).ToList();
        //    }
        //    catch (Exception ex) { logger.Error(ex); }

        //}
        //private void initializeGrippeYield()
        //{
        //    try
        //    {
        //        listGripperYield.Add(new MainModel
        //        {
        //            MainKey = "Gripper_Total_Pass",
        //            MainTag = OPCUAHelper.CombinedNodeString(Turret_NameSpace, "Gripper_Total_Pass")

        //        });
        //        listGripperYield.Add(new MainModel
        //        {
        //            MainKey = "Gripper_Total_Fail",
        //            MainTag = OPCUAHelper.CombinedNodeString(Turret_NameSpace, "Gripper_Total_Fail")

        //        });

        //        GripperYieldList = new ObservableCollection<MainModel>(listGripperYield);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        //private void initializeMaterialInletType()
        //{
        //    try
        //    {
        //        listMaterialInletType.Add(new MainModel
        //        {
        //            MainKey = "Reel Form Mode",
        //            MainTag = OPCUAHelper.CombinedNodeString(Unit_NameSpace, "Reel_Form_Mode")

        //        });
        //        listMaterialInletType.Add(new MainModel
        //        {
        //            MainKey = "Loose Form Mode",
        //            MainTag = OPCUAHelper.CombinedNodeString(Unit_NameSpace, "Loose_Form_Mode")

        //        });

        //        MaterialInletTypeList = new ObservableCollection<MainModel>(listMaterialInletType);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        private void initializeMachineLayout()
        {
            MachineLayoutInterfaceModel.MachineLayoutModel = MachineLayoutService.DeserializeMachineLayout();
            //MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutToggleList.ForEach(x =>
            //{
            //    x.MachineLayoutToggleCommand = new GalaSoft.MvvmLight.Command.RelayCommand<MachineLayoutToggleModel>(machineLayoutToggleCommand);
            //});
        }

        public Dictionary<string, string> GetTagList()
        {
            // For Main Page Tag List
            Dictionary<string, string> TagList = new Dictionary<string, string>();

            //TagList.AddRange(GripperYieldList.Select (x => x.MainTag, ));
            //TagList.AddRange(StationEnableList.Select(x => x.MainTag).ToList());
            //TagList.AddRange(MaterialInletTypeList.Select(x => x.MainTag).ToList());
            return TagList;
        }

        private void InitializeKX7500(KeyenceDll _keyenceDll)
        {
            //if(_keyenceDll.Module.Contains(OPCModules.MachineLayout))
            //{
            //    keyenceDll = _keyenceDll;
            //    machineLayoutTagList = keyenceDll.GetAddresses();
            //}
        }

        #endregion PrivateInitializeMethods

        #region PrivateMessengerMethods

        //private void opcUpdate(_OPC opc_)
        //{
        //    if(opc_.Module.Contains(OPCModules.LotEntry) || opc_.Module.Contains(OPCModules.OEE) || opc_.Module.Contains(OPCModules.Main) || opc_.Module.Contains(OPCModules.MachineLayout) || opc_.Module.Contains(OPCModules.Settings))
        //    {
        //        opc_.DataChange.ToList().ForEach(node =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if(node.Value != null)
        //                {
        //                    if(LotInfoList.ToList().Exists(y => y.MainTag == node.Key))
        //                    {
        //                        LotInfoList.ToList().Find(y => y.MainTag ==
        //                        node.Key).MainValue = Convert.ToString(node.Value);
        //                    }
        //                    //if (GripperYieldList.ToList().Exists(y => y.MainTag == node.Key))
        //                    //{
        //                    //    GripperYieldList.ToList().Find(y => y.MainTag ==
        //                    //    node.Key).MainValue = Convert.ToString(node.Value);
        //                    //}
        //                    //if (MaterialInletTypeList.ToList().Exists(y => y.MainTag == node.Key))
        //                    //{
        //                    //    if (node.Key == OPCUAHelper.CombinedNodeString(Unit_NameSpace, "Reel_Form_Mode"))
        //                    //    {
        //                    //        if (Convert.ToBoolean(node.Value) == true) { StrMaterialInletType = "Reel Pin"; }
        //                    //    }
        //                    //    if (node.Key == OPCUAHelper.CombinedNodeString(Unit_NameSpace, "Loose_Form_Mode"))
        //                    //    {
        //                    //        if (Convert.ToBoolean(node.Value) == true) { StrMaterialInletType = "Loose Pin"; }
        //                    //    }
        //                    //}
        //                    //if (MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutModuleList.SelectMany(
        //                    //    x => x.MachineLayoutModuleStatusIndicatorList).ToList().Exists(
        //                    //    x => x.MachineLayoutModuleStatusIndicatorTagModel.MachineLayoutModuleStatusIndicatorTag == node.Key))
        //                    //{
        //                    //    MachineLayoutInterfaceModel.MachineLayoutModel.MachineLayoutModuleList.SelectMany(
        //                    //        x => x.MachineLayoutModuleStatusIndicatorList).ToList().Find(
        //                    //        x => x.MachineLayoutModuleStatusIndicatorTagModel.MachineLayoutModuleStatusIndicatorTag == node.Key).MachineLayoutModuleStatusIndicatorIndex = node.Value;

        //                    //    logger.Debug($"OPC Update Machine Layout Value: {node.Value}");
        //                    //    logger.Debug($"OPC Update Machine Layout Key: {node.Key}");
        //                    //}
        //                }
        //            });
        //        });
        //    }
        //}

        private void updateLanguage(LanguageModel obj = null)
        {
            //LotInfoList.ToList().ForEach(z => z.MainName = HmiHelper.GetResource<string>(z.MainKey));
            //GripperYieldList.ToList().ForEach(z => z.MainName = HmiHelper.GetResource<string>(z.MainKey));
            //StationEnableList.ToList().ForEach(z => z.MainName = HmiHelper.GetResource<string>(z.MainKey));
            //MaterialInletTypeList.ToList().ForEach(z => z.MainName = HmiHelper.GetResource<string>(z.MainKey));
        }

        #endregion PrivateMessengerMethods

        #region PrivateCommand

        //private void machineLayoutToggleCommand(MachineLayoutToggleModel machineLayoutToggleModel)
        //{
        //    bool val = bool.Parse(machineLayoutToggleModel.MachineLayoutToggleValue);
        //    opcUa.Write(machineLayoutToggleModel.MachineLayoutToggleTagModel.MachineLayoutToggleTag, val);
        //}

        #endregion PrivateCommand
    }
}