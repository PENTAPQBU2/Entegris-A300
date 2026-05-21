using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.TrackingStatus;
using HMI_Master_KX7500.Model.TrackingStatus.Module;
using HMI_Master_KX7500.Model.TrackingStatus.Module.Indicator;
using HMI_Master_KX7500.Model.TrackingStatus.ReadOnly;
using HMI_Master_KX7500.Model.TrackingStatus.Status;
using HMI_Master_KX7500.OPCUA;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.Service.TrackingStatus
{
    public class TrackingStatusService
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private static TrackingStatusInitializeModel trackingStatusInitializeModel = new TrackingStatusInitializeModel();
        private static string IsInDesignMode = "IsInDesignMode";
        #endregion

        #region Constructor
        //public TrackingStatusService()
        //{
        //    initializeMessenger();
        //}
        #endregion

        #region PrivateInitializeMethods
        //private void initializeMessenger()
        //{
        //    Messenger.Default.Register<TrackingStatusInitializeModel>(this, initializeTrackingStatus);
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeTrackingStatus(TrackingStatusInitializeModel _trackingStatusInitializeModel)
        //{
        //    trackingStatusInitializeModel = _trackingStatusInitializeModel;
        //}
        #endregion

        #region PublicMethods
        public static bool SerializeTrackingStatus(TrackingStatusModel trackingStatusModel)
        {
            bool value = false;
            try
            {
                if (trackingStatusModel.TrackingStatusModuleList.Any(
                x => string.IsNullOrEmpty(x.TrackingStatusModuleKey)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleModel.TrackingStatusModuleKey)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusModuleList.Any(
                    x => string.IsNullOrEmpty(x.TrackingStatusModuleImage)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleModel.TrackingStatusModuleImage)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusModuleList.SelectMany(
                    x => x.TrackingStatusModuleStatusIndicatorList).Any(
                    y => string.IsNullOrEmpty(y.TrackingStatusModuleStatusIndicatorKey)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleStatusIndicatorModel.TrackingStatusModuleStatusIndicatorKey)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusModuleList.SelectMany(
                    x => x.TrackingStatusModuleStatusIndicatorList).Any(y => string.IsNullOrEmpty(
                        y.TrackingStatusModuleStatusIndicatorTagModel.TrackingStatusModuleStatusIndicatorTag)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleStatusIndicatorTagModel.TrackingStatusModuleStatusIndicatorTag)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusModuleStatusList.Any(
                    x => string.IsNullOrEmpty(x.TrackingStatusModuleStatusKey)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleStatusModel.TrackingStatusModuleStatusKey)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusModuleStatusList.Any(
                    x => string.IsNullOrEmpty(x.TrackingStatusModuleStatusIndex)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleStatusModel.TrackingStatusModuleStatusIndex)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusModuleStatusList.Any(
                    x => string.IsNullOrEmpty(x.TrackingStatusModuleStatusColor)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusModuleStatusModel.TrackingStatusModuleStatusColor)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusReadOnlyGroupList.Any(x => string.IsNullOrEmpty(x.TrackingStatusReadOnlyGroupKey)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusReadOnlyGroup.TrackingStatusReadOnlyGroupKey)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusReadOnlyGroupList.SelectMany(x => x.TrackingStatusReadOnlyList).Any(x => string.IsNullOrEmpty(x.TrackingStatusReadOnlyKey)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusReadOnlyModel.TrackingStatusReadOnlyKey)} Cannot Be Null/Empty");
                }
                else if (trackingStatusModel.TrackingStatusReadOnlyGroupList.SelectMany(x => x.TrackingStatusReadOnlyList).Any(x => string.IsNullOrEmpty(x.TrackingStatusReadOnlyTagModel.TrackingStatusReadOnlyTag)))
                {
                    MessageBox.Show($"{nameof(TrackingStatusReadOnlyTagModel.TrackingStatusReadOnlyTag)} Cannot Be Null/Empty");
                }
                else
                {
                    bool valid = true;
                    if (valid)
                    {
                        XmlTools.Serialize(FilePath.TrackingStatus, trackingStatusModel);

                        value = true;
                    }

                    //Messenger.Default.Send(trackingStatusInitializeModel);
                }
            }
            catch (Exception ex) { logger.Error(ex); }

            return value;
        }

        public static TrackingStatusModel DeserializeTrackingStatus()
        {
            TrackingStatusModel trackingStatusModel = new TrackingStatusModel();

            string Pallet_Info_Data = "DB_Pallet_Info_Data";
            string Turret = "DB_Turret_Station_Data";
            string Remove_Status = "DB_Remove_Status";
            string Loader_Carrier = "Loader_Carrier";
            string Loader_Gripper = "Loader_Gripper";
            string Insertion_Stage = "Insertion_Stage";
            string Unloader_Gripper = "Unloader_Gripper";
            string Unloader_Carrier = "Unloader_Carrier";
            string Jig_Barcode = "Jig_Barcode";
            string Gripper_Status = "Gripper_Status";
            string Pallet_ID = "Pallet_ID";

            if (!File.Exists(FilePath.TrackingStatus))
            {
                trackingStatusModel = new TrackingStatusModel
                {
                    TrackingStatusModuleList = new List<TrackingStatusModuleModel>()
                    {
                        new TrackingStatusModuleModel()
                        {
                            TrackingStatusModuleImage = FolderPath.Images + "TrackingStatus.png",
                            TrackingStatusModuleKey = "Pallet_Tracking",
                              TrackingStatusModuleStatusList = new List<TrackingStatusModuleStatusModel>
                    {
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "Empty",
                            TrackingStatusModuleStatusIndex = "1",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Gray)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "PalletPresent",
                            TrackingStatusModuleStatusIndex = "5",
                            TrackingStatusModuleStatusColor = nameof(Brushes.LightBlue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "AlignmentPass",
                            TrackingStatusModuleStatusIndex = "55",
                            TrackingStatusModuleStatusColor = nameof(Brushes.LightBlue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "PinInsertPass",
                            TrackingStatusModuleStatusIndex = "56",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Blue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "OverallPass",
                            TrackingStatusModuleStatusIndex = "50",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Green)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "AlignmentFail",
                            TrackingStatusModuleStatusIndex = "95",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "PinInsertFail",
                            TrackingStatusModuleStatusIndex = "96",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "FinalInspectionFail",
                            TrackingStatusModuleStatusIndex = "97",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "Purged",
                            TrackingStatusModuleStatusIndex = "98",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Yellow)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "Disabled",
                            TrackingStatusModuleStatusIndex = "0",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Black)
                        },
                    },
                            TrackingStatusModuleStatusIndicatorList = new List<TrackingStatusModuleStatusIndicatorModel>()
                            {
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Loader_Carrier_Status",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 380,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 420,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Loader_Carrier, "Status"),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Loader_Carrier"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Unloader_Carrier_Status",
                                    TrackingStatusModuleStatusIndicatorRightPosition = -100,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 420,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Unloader_Carrier, "Status"),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Unloader_Carrier"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Loader_Gripper_Status",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 390,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 290,
                                    TrackingStatusModuleStatusIndicatorLineX1 = -400,
                                    TrackingStatusModuleStatusIndicatorLineX2 = -150,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 315,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 315,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Loader_Gripper, "Status"),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Loader_Gripper"),
                                    },
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Unloader_Gripper_Status",
                                    TrackingStatusModuleStatusIndicatorRightPosition = -235,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 290,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = -50,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 315,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 315,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Unloader_Gripper, "Status"),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Unloader_Gripper"),
                                    },
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Insertion_Stage_Status",
                                    TrackingStatusModuleStatusIndicatorRightPosition = -225,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 175,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = -50,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 200,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 200,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Insertion_Stage, "Status"),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Insertion_Stages"),
                                    }
                                },
                            },
                        },
                        //Turret Station
                        new TrackingStatusModuleModel()
                        {
                            TrackingStatusModuleImage = FolderPath.Images + "Turret.jpg",
                            TrackingStatusModuleKey = "Turret_Station_Gripper",
                              TrackingStatusModuleStatusList = new List<TrackingStatusModuleStatusModel>
                    {
                          new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "Empty",
                            TrackingStatusModuleStatusIndex = "1",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Gray)
                        },
                         new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "PinPresent",
                            TrackingStatusModuleStatusIndex = "5",
                            TrackingStatusModuleStatusColor = nameof(Brushes.LightBlue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "VisionPass",
                            TrackingStatusModuleStatusIndex = "55",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Green)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "VisionFail",
                            TrackingStatusModuleStatusIndex = "56",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                         new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "Disabled",
                            TrackingStatusModuleStatusIndex = "0",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Black)
                        },
                              },
                            TrackingStatusModuleStatusIndicatorList = new List<TrackingStatusModuleStatusIndicatorModel>()
                            {
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#1",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 520,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 235,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 1),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_1"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#2",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 460,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 410,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 2),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_2"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#3",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 320,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 480,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 3),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_3"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#4",
                                    TrackingStatusModuleStatusIndicatorRightPosition = -180,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 410,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 4),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_4"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#5",
                                    TrackingStatusModuleStatusIndicatorRightPosition = -220,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 235,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 5),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_5"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#6",
                                    TrackingStatusModuleStatusIndicatorRightPosition = -180,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 60,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 6),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_6"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#7",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 300,
                                    TrackingStatusModuleStatusIndicatorTopPosition = -10,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 7),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_7"),
                                    }
                                },
                                new TrackingStatusModuleStatusIndicatorModel()
                                {
                                    TrackingStatusModuleStatusIndicatorKey = "Turret_Station_Gripper#8",
                                    TrackingStatusModuleStatusIndicatorRightPosition = 460,
                                    TrackingStatusModuleStatusIndicatorTopPosition = 60,
                                    TrackingStatusModuleStatusIndicatorLineX1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineX2 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY1 = 0,
                                    TrackingStatusModuleStatusIndicatorLineY2 = 0,
                                    TrackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel()
                                    {
                                        TrackingStatusModuleStatusIndicatorTag = OPCUAHelper.CombinedNodeArray(Turret, Gripper_Status, 8),
                                        TrackingStatusModuleStatusIndicatorTriggerTag = OPCUAHelper.CombinedNodeString(Remove_Status, "Turret_Gripper_8"),
                                    }
                                },
                            },
                        },
                    },
                    TrackingStatusReadOnlyGroupList = new List<TrackingStatusReadOnlyGroup>
                    {
                        new TrackingStatusReadOnlyGroup
                        {
                            TrackingStatusReadOnlyGroupKey = "Loader_Carrier",
                            TrackingStatusReadOnlyList = new List<TrackingStatusReadOnlyModel>
                            {
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Carrier_Pallet_ID",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Loader_Carrier, Pallet_ID),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Carrier_Jig_Barcode#1",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Loader_Carrier, Jig_Barcode, 0),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Carrier_Jig_Barcode#2",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Loader_Carrier, Jig_Barcode, 1),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Carrier_Jig_Barcode#3",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Loader_Carrier, Jig_Barcode, 2),
                                    }
                                },
                            }
                        },
                        new TrackingStatusReadOnlyGroup
                        {
                            TrackingStatusReadOnlyGroupKey = "Unloader_Carrier",
                            TrackingStatusReadOnlyList = new List<TrackingStatusReadOnlyModel>
                            {
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Carrier_Pallet_ID",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Unloader_Carrier, Pallet_ID),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Carrier_Jig_Barcode#1",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Unloader_Carrier, Jig_Barcode, 0),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Carrier_Jig_Barcode#2",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Unloader_Carrier, Jig_Barcode, 1),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Carrier_Jig_Barcode#3",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Unloader_Carrier, Jig_Barcode, 2),
                                    }
                                },
                            }
                        },
                        new TrackingStatusReadOnlyGroup
                        {
                            TrackingStatusReadOnlyGroupKey = "Loader_Gripper",
                            TrackingStatusReadOnlyList = new List<TrackingStatusReadOnlyModel>
                            {
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Gripper_Pallet_ID",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Loader_Gripper, Pallet_ID),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Gripper_Jig_Barcode#1",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Loader_Gripper, Jig_Barcode, 0),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Gripper_Jig_Barcode#2",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Loader_Gripper, Jig_Barcode, 1),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Loader_Gripper_Jig_Barcode#3",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Loader_Gripper, Jig_Barcode, 2),
                                    }
                                },
                            }
                        },
                        new TrackingStatusReadOnlyGroup
                        {
                            TrackingStatusReadOnlyGroupKey = "Unloader_Gripper",
                            TrackingStatusReadOnlyList = new List<TrackingStatusReadOnlyModel>
                            {
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Gripper_Pallet_ID",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Unloader_Gripper, Pallet_ID),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Gripper_Jig_Barcode#1",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Unloader_Gripper, Jig_Barcode, 0),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Gripper_Jig_Barcode#2",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Unloader_Gripper, Jig_Barcode, 1),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Unloader_Gripper_Jig_Barcode#3",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Unloader_Gripper, Jig_Barcode, 2),
                                    }
                                },
                            }
                        },
                        new TrackingStatusReadOnlyGroup
                        {
                            TrackingStatusReadOnlyGroupKey = "Insertion_Stage",
                            TrackingStatusReadOnlyList = new List<TrackingStatusReadOnlyModel>
                            {
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Insertion_Stage_Pallet_ID",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeString(Pallet_Info_Data, Insertion_Stage, Pallet_ID),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Insertion_Stage_Jig_Barcode#1",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Insertion_Stage, Jig_Barcode, 0),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Insertion_Stage_Jig_Barcode#2",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Insertion_Stage, Jig_Barcode, 1),
                                    }
                                },
                                new TrackingStatusReadOnlyModel
                                {
                                    TrackingStatusReadOnlyKey = "Insertion_Stage_Jig_Barcode#3",
                                    TrackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel
                                    {
                                        TrackingStatusReadOnlyTag = OPCUAHelper.CombinedNodeArray(Pallet_Info_Data, Insertion_Stage, Jig_Barcode, 2),
                                    }
                                },
                            }
                        },
                    },
                    /*
                    TrackingStatusModuleStatusList = new List<TrackingStatusModuleStatusModel>
                    {
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "Empty",
                            TrackingStatusModuleStatusIndex = "1",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Gray)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "PalletPresent",
                            TrackingStatusModuleStatusIndex = "5",
                            TrackingStatusModuleStatusColor = nameof(Brushes.LightBlue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "AlignmentPass",
                            TrackingStatusModuleStatusIndex = "55",
                            TrackingStatusModuleStatusColor = nameof(Brushes.LightBlue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "PinInsertPass",
                            TrackingStatusModuleStatusIndex = "56",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Blue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "OverallPass",
                            TrackingStatusModuleStatusIndex = "50",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Green)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "AlignmentFail",
                            TrackingStatusModuleStatusIndex = "95",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "PinInsertFail",
                            TrackingStatusModuleStatusIndex = "96",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "FinalInspectionFail",
                            TrackingStatusModuleStatusIndex = "97",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "Purged",
                            TrackingStatusModuleStatusIndex = "98",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Yellow)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Pallet_Tracking",
                            TrackingStatusModuleStatusKey = "Disabled",
                            TrackingStatusModuleStatusIndex = "0",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Black)
                        },
                          new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "Empty",
                            TrackingStatusModuleStatusIndex = "1",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Gray)
                        },
                         new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "PinPresent",
                            TrackingStatusModuleStatusIndex = "5",
                            TrackingStatusModuleStatusColor = nameof(Brushes.LightBlue)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "VisionPass",
                            TrackingStatusModuleStatusIndex = "55",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Green)
                        },
                        new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "VisionFail",
                            TrackingStatusModuleStatusIndex = "56",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Red)
                        },
                         new TrackingStatusModuleStatusModel
                        {TrackingStatusModuleKey = "Turret_Station_Gripper",
                            TrackingStatusModuleStatusKey = "Disabled",
                            TrackingStatusModuleStatusIndex = "0",
                            TrackingStatusModuleStatusColor = nameof(Brushes.Black)
                        },
                    }
                    */
                };

                SerializeTrackingStatus(trackingStatusModel);
            }

            trackingStatusModel = XmlTools.Deserialize<TrackingStatusModel>(FilePath.TrackingStatus);

            return trackingStatusModel;
        }

        public static List<string> GetTrackingStatusTagList()
        {
            TrackingStatusModel trackingStatusModel = DeserializeTrackingStatus();
            List<string> trackingStatusTagList = new List<string>();

            trackingStatusTagList = trackingStatusModel.TrackingStatusModuleList.SelectMany(
                    x => x.TrackingStatusModuleStatusIndicatorList).Select(
                    x => x.TrackingStatusModuleStatusIndicatorTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString())).ToList();

            trackingStatusTagList = trackingStatusTagList.Union(trackingStatusModel.TrackingStatusReadOnlyGroupList.SelectMany(
                x => x.TrackingStatusReadOnlyList).Select(x => x.TrackingStatusReadOnlyTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                    y => y.GetValue(x).ToString()))).ToList();

            return trackingStatusTagList;
        }
        #endregion
    }
}
