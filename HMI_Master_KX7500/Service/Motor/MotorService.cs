using CsvHelper;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
//using HMI_Master_KX7500.OPCUA;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.Model
{
    public class MotorService
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private static MotorInitializeModel motorInitializeModel = new MotorInitializeModel();
        #endregion

        #region PublicFields
        public const string MotorAxisTeachingPosition = nameof(MotorAxisTeachingPosition);
        public const string IsInDesignMode = "IsInDesignMode";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public const string ShortDateFormat = "yyyy-MM-dd";
        public const string ResourceDictionarySource = "pack://application:,,,/HMI;component/";
        public const char Comma = ',';
        public const char Underscore = '_';
        #endregion

        #region Constructor
        //public MotorService()
        //{
        //    initializeMessenger();
        //}
        #endregion

        #region PrivateInitializeMethods
        //private void initializeMessenger()
        //{
        //    try
        //    {
        //        Messenger.Default.Register<MotorInitializeModel>(this, initializeMotor);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeMotor(MotorInitializeModel _motorInitializeModel)
        //{
        //    try
        //    {
        //        motorInitializeModel = _motorInitializeModel;
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PublicMethods
        public static bool SerializeMotor(MotorModel motorModel)
        {
            bool value = false;

            try
            {
                if (motorModel.AxisList.Any(x => string.IsNullOrEmpty(x.AxisKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisModel.AxisKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralStatusModel.AxisGeneralStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralStatusTagModel.AxisGeneralStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralStatusTagModel.AxisGeneralStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralNumericList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralNumericKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralNumericModel.AxisGeneralNumericKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralNumericList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralNumericTagModel.AxisGeneralNumericTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralNumericTagModel.AxisGeneralNumericTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralActionKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralActionModel.AxisGeneralActionKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralActionIcon)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralActionModel.AxisGeneralActionIcon));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisGeneralActionTagModel.AxisGeneralActionTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisGeneralActionTagModel.AxisGeneralActionTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisHomingModel.AxisHomingStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisHomingStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisHomingStatusModel.AxisHomingStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisHomingModel.AxisHomingStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisHomingStatusTagModel.AxisHomingStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisHomingStatusTagModel.AxisHomingStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisHomingModel.AxisHomingActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisHomingActionKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisHomingActionModel.AxisHomingActionKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisHomingModel.AxisHomingActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisHomingActionIcon)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisHomingActionModel.AxisHomingActionIcon));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisHomingModel.AxisHomingActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisHomingActionTagModel.AxisHomingActionTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisHomingActionTagModel.AxisHomingActionTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisPositionModel.AxisPositionStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisPositionStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisPositionStatusModel.AxisPositionStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisPositionModel.AxisPositionStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisPositionStatusTagModel.AxisPositionStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisPositionStatusTagModel.AxisPositionStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisPositionModel.AxisPositionNumericStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisPositionNumericStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisPositionNumericStatusModel.AxisPositionNumericStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisPositionModel.AxisPositionNumericStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisPositionNumericStatusTagModel.AxisPositionNumericStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisPositionNumericStatusTagModel.AxisPositionNumericStatusTag));
                }
                else if (motorModel.AxisList.Any(x => string.IsNullOrEmpty(
                    x.AxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogTagModel.AxisManualJogTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisManualJogModel.AxisManualJogIntervalList).Any(
                    x => string.IsNullOrEmpty(x.AxisManualJogIntervalKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogIntervalModel.AxisManualJogIntervalKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisManualJogModel.AxisManualJogIntervalList).Any(
                    x => string.IsNullOrEmpty(x.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogIntervalTagModel.AxisManualJogIntervalTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisManualJogModel.AxisManualJogStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisManualJogStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogStatusModel.AxisManualJogStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisManualJogModel.AxisManualJogStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisManualJogStatusTagModel.AxisManualJogStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogStatusTagModel.AxisManualJogStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisManualJogModel.AxisManualJogNumericList).Any(
                    x => string.IsNullOrEmpty(x.AxisManualJogNumericKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogNumericModel.AxisManualJogNumericKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisManualJogModel.AxisManualJogNumericList).Any(
                    x => string.IsNullOrEmpty(x.AxisManualJogNumericTagModel.AxisManualJogNumericTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisManualJogNumericTagModel.AxisManualJogNumericTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteStatusModel.AxisMoveAbsoluteStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteStatusTagModel.AxisMoveAbsoluteStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteStatusTagModel.AxisMoveAbsoluteStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteNumericKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteNumericModel.AxisMoveAbsoluteNumericKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteActionKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteActionModel.AxisMoveAbsoluteActionKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteActionIcon)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteActionModel.AxisMoveAbsoluteActionIcon));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList).Any(
                    x => string.IsNullOrEmpty(x.AxisMoveAbsoluteActionTagModel.AxisMoveAbsoluteActionTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisMoveAbsoluteActionTagModel.AxisMoveAbsoluteActionTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).Any(
                    x => string.IsNullOrEmpty(x.AxisTeachingPositionKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionModel.AxisTeachingPositionKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionNumericStatusList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionNumericStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionNumericStatusModel.AxisTeachingPositionNumericStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionNumericStatusList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionNumericStatusTagModel.AxisTeachingPositionNumericStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionNumericStatusTagModel.AxisTeachingPositionNumericStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionNumericList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionNumericKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionNumericModel.AxisTeachingPositionNumericKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionNumericList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionStatusList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionStatusKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionStatusModel.AxisTeachingPositionStatusKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionStatusList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionStatusTagModel.AxisTeachingPositionStatusTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionStatusTagModel.AxisTeachingPositionStatusTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionActionList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionActionKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionActionModel.AxisTeachingPositionActionKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionActionList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionActionIcon)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionActionModel.AxisTeachingPositionActionIcon));
                }
                else if (motorModel.AxisList.SelectMany(x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionActionList).Any(x => string.IsNullOrEmpty(
                        x.AxisTeachingPositionActionTagModel.AxisTeachingPositionActionTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorAxisTeachingPositionActionTagModel.AxisTeachingPositionActionTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.MotorIOModel.MotorIOInputList).Any(x => string.IsNullOrEmpty(x.MotorIOInputKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorIOInputModel.MotorIOInputKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.MotorIOModel.MotorIOOutputList).Any(x => string.IsNullOrEmpty(x.MotorIOOutputKey)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorIOOutputModel.MotorIOOutputKey));
                }
                else if (motorModel.AxisList.SelectMany(x => x.MotorIOModel.MotorIOInputList).Any(x => string.IsNullOrEmpty(x.MotorIOInputTagModel.MotorIOInputTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorIOInputTagModel.MotorIOInputTag));
                }
                else if (motorModel.AxisList.SelectMany(x => x.MotorIOModel.MotorIOOutputList).Any(x => string.IsNullOrEmpty(x.MotorIOOutputTagModel.MotorIOOutputTag)))
                {
                    MessageBoxShowCannotBeNull(nameof(MotorIOOutputTagModel.MotorIOOutputTag));
                }
                else
                {
                    XmlTools.Serialize(FilePath.Motor, motorModel);

                    value = true;

                    //Messenger.Default.Send(motorInitializeModel);
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            return value;
        }

        public static MotorModel DeserializeMotor()
        {
            MotorModel motorModel = new MotorModel();

            try
            {
                if (!File.Exists(FilePath.Motor))
                {
                    motorModel = new MotorModel();

                    string axisKey = string.Empty;

                    //i is for motor axis, j is for axis position

                    List<MotorAxisTeachingPositionModel> axisTeachingPositionList = new List<MotorAxisTeachingPositionModel>();

                    string axisPositionKey = string.Empty;

                    //Ax1
                    axisKey = "General";

                    MotorIOModel motorIOModel = new MotorIOModel();
                    motorIOModel.MotorIOInputList = new List<MotorIOInputModel>();
                    motorIOModel.MotorIOOutputList = new List<MotorIOOutputModel>();

                    motorModel.AxisList.Add(new MotorAxisModel
                    {
                        AxisKey = axisKey,
                        AxisGeneralModel = new MotorAxisGeneralModel
                        {
                            AxisGeneralStatusList = new List<MotorAxisGeneralStatusModel>
                                    {
                                        new MotorAxisGeneralStatusModel
                                        {
                                            AxisGeneralStatusKey = "Ready",
                                            AxisGeneralStatusTagModel = new MotorAxisGeneralStatusTagModel
                                            {
                                                AxisGeneralStatusTag = "MR2001"
                                            }
                                        },
                                        //new MotorAxisGeneralStatusModel
                                        //{
                                        //    AxisGeneralStatusKey = "Stop",
                                        //    AxisGeneralStatusTagModel = new MotorAxisGeneralStatusTagModel
                                        //    {
                                        //        AxisGeneralStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "Stop")
                                        //    }
                                        //},
                                        new MotorAxisGeneralStatusModel
                                        {
                                            AxisGeneralStatusKey = "AxisError",
                                            AxisGeneralStatusTagModel = new MotorAxisGeneralStatusTagModel
                                            {
                                                AxisGeneralStatusTag = "MR2007"
                                            }
                                        },
                                        new MotorAxisGeneralStatusModel
                                        {
                                            AxisGeneralStatusKey = "Reset",
                                            AxisGeneralStatusTagModel = new MotorAxisGeneralStatusTagModel
                                            {
                                               AxisGeneralStatusTag = "MR403"
                                            }
                                        },
                                        //new MotorAxisGeneralStatusModel
                                        //{
                                        //    AxisGeneralStatusKey = "Alarm",
                                        //    AxisGeneralStatusTagModel = new MotorAxisGeneralStatusTagModel
                                        //    {
                                        //        AxisGeneralStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MotorAlarmBit")
                                        //    }
                                        //}
                                    },
                            AxisGeneralNumericList = new List<MotorAxisGeneralNumericModel>
                                    {
                                        new MotorAxisGeneralNumericModel
                                        {
                                            AxisGeneralNumericKey = "Speed",
                                            //AxisGeneralNumericMaximum = 100,
                                            //AxisGeneralNumericMinimum = 10,
                                            AxisGeneralNumericTagModel = new MotorAxisGeneralNumericTagModel
                                            {
                                                AxisGeneralNumericTag = "EM4009"
                                            },
                                            AxisGeneralNumericDecimalPlaces = 3,
                                        }
                                    },
                            AxisGeneralActionList = new List<MotorAxisGeneralActionModel>
                            {
                                //new MotorAxisGeneralActionModel
                                //{
                                //    AxisGeneralActionKey = "Stop",
                                //    AxisGeneralActionIcon = GetIconSource("Stop"),
                                //    AxisGeneralActionTagModel = new MotorAxisGeneralActionTagModel
                                //    {
                                //        AxisGeneralActionTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "HMIStopAxis")
                                //    }
                                //},
                                //new MotorAxisGeneralActionModel
                                //{
                                //    AxisGeneralActionKey = "Reset",
                                //    AxisGeneralActionIcon = GetIconSource("Reload"),
                                //    AxisGeneralActionTagModel = new MotorAxisGeneralActionTagModel
                                //    {
                                //        AxisGeneralActionTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "HMIResetAxis")
                                //    }
                                //}
                            }
                        },
                        AxisHomingModel = new MotorAxisHomingModel
                        {
                            AxisHomingStatusList = new List<MotorAxisHomingStatusModel>
                                    {
                                        new MotorAxisHomingStatusModel
                                        {
                                            AxisHomingStatusKey = "HomeDone",
                                            AxisHomingStatusTagModel = new MotorAxisHomingStatusTagModel
                                            {
                                                //AxisHomingStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "HomeDone")
                                                AxisHomingStatusTag = "MR2003"
                                            }
                                        },
                                        //new MotorAxisHomingStatusModel
                                        //{
                                        //    AxisHomingStatusKey = "HomingCommand",
                                        //    AxisHomingStatusTagModel = new MotorAxisHomingStatusTagModel
                                        //    {
                                        //        AxisHomingStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "Axis_HomingCommand")
                                        //    }
                                        //},
                                        //new MotorAxisHomingStatusModel
                                        //{
                                        //    AxisHomingStatusKey = "HomingDone",
                                        //    AxisHomingStatusTagModel = new MotorAxisHomingStatusTagModel
                                        //    {
                                        //        AxisHomingStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "Axis_HomingDone")
                                        //    }
                                        //},
                                        //new MotorAxisHomingStatusModel
                                        //{
                                        //    AxisHomingStatusKey = "HomingInProgress",
                                        //    AxisHomingStatusTagModel = new MotorAxisHomingStatusTagModel
                                        //    {
                                        //        AxisHomingStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "Axis_HomingInProg")
                                        //    }
                                        //},
                                        //new MotorAxisHomingStatusModel
                                        //{
                                        //    AxisHomingStatusKey = "HomeRequestInterlock",
                                        //    AxisHomingStatusTagModel = new MotorAxisHomingStatusTagModel
                                        //    {
                                        //        AxisHomingStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "HomeReq_Interlock")
                                        //    }
                                        //},
                                        //new MotorAxisHomingStatusModel
                                        //{
                                        //    AxisHomingStatusKey = "Homing",
                                        //    AxisHomingStatusTagModel = new MotorAxisHomingStatusTagModel
                                        //    {
                                        //        AxisHomingStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "Homing")
                                        //    }
                                        //}
                                    },
                            AxisHomingActionList = new List<MotorAxisHomingActionModel>
                                    {
                                        new MotorAxisHomingActionModel
                                        {
                                            AxisHomingActionKey = "Home",
                                            AxisHomingActionIcon = GetIconSource("Refresh"),
                                            AxisHomingActionTagModel = new MotorAxisHomingActionTagModel
                                            {
                                                //AxisHomingActionTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "HomeReq")
                                                AxisHomingActionTag = "MR2002"
                                            }
                                        }
                                    }
                        },
                        AxisPositionModel = new MotorAxisPositionModel
                        {
                            AxisPositionStatusList = new List<MotorAxisPositionStatusModel>
                                    {
                                        //new MotorAxisPositionStatusModel
                                        //{
                                        //    AxisPositionStatusKey = "Standstill",
                                        //    AxisPositionStatusTagModel = new MotorAxisPositionStatusTagModel
                                        //    {
                                        //        AxisPositionStatusTag = SiemensOPCHelper.GetBooleanTag(
                                        //            s7Connection, dataBlockNumber, 1.1)
                                        //    }
                                        //},
                                        new MotorAxisPositionStatusModel
                                        {
                                            AxisPositionStatusKey = "InPosition",
                                            AxisPositionStatusTagModel = new MotorAxisPositionStatusTagModel
                                            {
                                                //AxisPositionStatusTag =  OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "InPos")
                                                AxisPositionStatusTag = "R40001"
                                            }
                                        }
                                    },
                            AxisPositionNumericStatusList = new List<MotorAxisPositionNumericStatusModel>
                                    {
                                        new MotorAxisPositionNumericStatusModel
                                        {
                                            AxisPositionNumericStatusKey = "CurrentPosition",
                                            AxisPositionNumericStatusTagModel = new MotorAxisPositionNumericStatusTagModel
                                            {
                                                //AxisPositionNumericStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "Axis_CurrentPosition")
                                                AxisPositionNumericStatusTag = "EM4000"
                                            },
                                            AxisPositionNumericStatusDecimalPlaces = 2,
                                        }
                                    }
                        },
                        AxisManualJogModel = new MotorAxisManualJogModel
                        {
                            AxisManualJogTagModel = new MotorAxisManualJogTagModel
                            {
                                //AxisManualJogTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "ManualJog")
                                AxisManualJogTag = "MR1003"
                            },
                            //AxisManualJogIntervalList = new List<MotorAxisManualJogIntervalModel>
                            //{
                            //    //new MotorAxisManualJogIntervalModel
                            //    //{
                            //    //    AxisManualJogIntervalKey = "0.1mm",
                            //    //    AxisManualJogIntervalTagModel = new MotorAxisManualJogIntervalTagModel
                            //    //    {
                            //    //        AxisManualJogIntervalTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogInterval_0_1mm")
                            //    //    }
                            //    //},
                            //    //new MotorAxisManualJogIntervalModel
                            //    //{
                            //    //    AxisManualJogIntervalKey = "1mm",
                            //    //    AxisManualJogIntervalTagModel = new MotorAxisManualJogIntervalTagModel
                            //    //    {
                            //    //        AxisManualJogIntervalTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogInterval_1mm")
                            //    //    }
                            //    //},
                            //    //new MotorAxisManualJogIntervalModel
                            //    //{
                            //    //    AxisManualJogIntervalKey = "10mm",
                            //    //    AxisManualJogIntervalTagModel = new MotorAxisManualJogIntervalTagModel
                            //    //    {
                            //    //        AxisManualJogIntervalTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogInterval_10mm")
                            //    //    }
                            //    //},
                            //    //new MotorAxisManualJogIntervalModel
                            //    //{
                            //    //    AxisManualJogIntervalKey = "Continuous",
                            //    //    AxisManualJogIntervalTagModel = new MotorAxisManualJogIntervalTagModel
                            //    //    {
                            //    //        AxisManualJogIntervalTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogInterval_Continuous")
                            //    //    }
                            //    //}
                            //},
                            AxisManualJogStatusList = new List<MotorAxisManualJogStatusModel>
                                    {
                                        new MotorAxisManualJogStatusModel
                                        {
                                            AxisManualJogStatusKey = "JogForward",
                                            AxisManualJogStatusTagModel = new MotorAxisManualJogStatusTagModel
                                            {
                                                //AxisManualJogStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogFwd")
                                                AxisManualJogStatusTag = "MR2004"
                                            }
                                        },
                                        //new MotorAxisManualJogStatusModel
                                        //{
                                        //    AxisManualJogStatusKey = "JogForwardInterlock",
                                        //    AxisManualJogStatusTagModel = new MotorAxisManualJogStatusTagModel
                                        //    {
                                        //        AxisManualJogStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogFwd_Interlock")
                                        //    }
                                        //},
                                        new MotorAxisManualJogStatusModel
                                        {
                                            AxisManualJogStatusKey = "JogBackward",
                                            AxisManualJogStatusTagModel = new MotorAxisManualJogStatusTagModel
                                            {
                                                //AxisManualJogStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogBwd")
                                                AxisManualJogStatusTag = "MR2005"
                                            }
                                        },
                                        //new MotorAxisManualJogStatusModel
                                        //{
                                        //    AxisManualJogStatusKey = "JogBackwardInterlock",
                                        //    AxisManualJogStatusTagModel = new MotorAxisManualJogStatusTagModel
                                        //    {
                                        //        AxisManualJogStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogBwd_Interlock")
                                        //    }
                                        //}
                                    },
                            AxisManualJogNumericList = new List<MotorAxisManualJogNumericModel>
                            {
                                //new MotorAxisManualJogNumericModel
                                //{
                                //    AxisManualJogNumericKey = "JogVelocity",
                                //    AxisManualJogNumericTagModel = new MotorAxisManualJogNumericTagModel
                                //    {
                                //        AxisManualJogNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogVelocity")
                                //    },
                                //    AxisManualJogNumericDecimalPlaces = 3,
                                //},
                                //new MotorAxisManualJogNumericModel
                                //{
                                //    AxisManualJogNumericKey = "JogAcceleration",
                                //    AxisManualJogNumericTagModel = new MotorAxisManualJogNumericTagModel
                                //    {
                                //        AxisManualJogNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogAcc")
                                //    },
                                //    AxisManualJogNumericDecimalPlaces = 3,
                                //},
                                //new MotorAxisManualJogNumericModel
                                //{
                                //    AxisManualJogNumericKey = "JogDeceleration",
                                //    AxisManualJogNumericTagModel = new MotorAxisManualJogNumericTagModel
                                //    {
                                //        AxisManualJogNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "JogDcc")
                                //    },
                                //    AxisManualJogNumericDecimalPlaces = 3,
                                //}
                            }
                        },
                        AxisMoveAbsoluteModel = new MotorAxisMoveAbsoluteModel
                        {
                            AxisMoveAbsoluteStatusList = new List<MotorAxisMoveAbsoluteStatusModel>
                            {
                                //new MotorAxisMoveAbsoluteStatusModel
                                //{
                                //    AxisMoveAbsoluteStatusKey = "MoveAbsoluteInProgress",
                                //    AxisMoveAbsoluteStatusTagModel = new MotorAxisMoveAbsoluteStatusTagModel
                                //    {
                                //        AxisMoveAbsoluteStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MoveAbsInProgress")
                                //    }
                                //},
                                //new MotorAxisMoveAbsoluteStatusModel
                                //{
                                //    AxisMoveAbsoluteStatusKey = "MoveAbsoluteInterlock",
                                //    AxisMoveAbsoluteStatusTagModel = new MotorAxisMoveAbsoluteStatusTagModel
                                //    {
                                //        AxisMoveAbsoluteStatusTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MoveAbs_Interlock")
                                //    }
                                //}
                            },
                            AxisMoveAbsoluteNumericList = new List<MotorAxisMoveAbsoluteNumericModel>
                                    {
                                        //new MotorAxisMoveAbsoluteNumericModel
                                        //{
                                        //    AxisMoveAbsoluteNumericKey = "MoveVelocity",
                                        //    AxisMoveAbsoluteNumericTagModel = new MotorAxisMoveAbsoluteNumericTagModel
                                        //    {
                                        //        AxisMoveAbsoluteNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MoveVelocity")
                                        //    },
                                        //    AxisMoveAbsoluteNumericDecimalPlaces = 3,
                                        //},
                                        new MotorAxisMoveAbsoluteNumericModel
                                        {
                                            AxisMoveAbsoluteNumericKey = "MoveAcceleration",
                                            AxisMoveAbsoluteNumericTagModel = new MotorAxisMoveAbsoluteNumericTagModel
                                            {
                                                //AxisMoveAbsoluteNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MoveAcc")
                                                AxisMoveAbsoluteNumericTag = "EM4011"
                                            },
                                            AxisMoveAbsoluteNumericDecimalPlaces = 3,
                                        },
                                        new MotorAxisMoveAbsoluteNumericModel
                                        {
                                            AxisMoveAbsoluteNumericKey = "MoveDeceleration",
                                            AxisMoveAbsoluteNumericTagModel = new MotorAxisMoveAbsoluteNumericTagModel
                                            {
                                                //AxisMoveAbsoluteNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MoveDcc")
                                                AxisMoveAbsoluteNumericTag = "EM4013"
                                            },
                                            AxisMoveAbsoluteNumericDecimalPlaces = 3,
                                        },
                                        //new MotorAxisMoveAbsoluteNumericModel
                                        //{
                                        //    AxisMoveAbsoluteNumericKey = "MovePosition",
                                        //    AxisMoveAbsoluteNumericTagModel = new MotorAxisMoveAbsoluteNumericTagModel
                                        //    {
                                        //        AxisMoveAbsoluteNumericTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MovePosition")
                                        //    },
                                        //    AxisMoveAbsoluteNumericDecimalPlaces = 3,
                                        //}
                                    },
                            AxisMoveAbsoluteActionList = new List<MotorAxisMoveAbsoluteActionModel>
                            {
                                //new MotorAxisMoveAbsoluteActionModel
                                //{
                                //    AxisMoveAbsoluteActionKey = "MoveAbsolute",
                                //    AxisMoveAbsoluteActionIcon = GetIconSource("Flash"),
                                //    AxisMoveAbsoluteActionTagModel = new MotorAxisMoveAbsoluteActionTagModel
                                //    {
                                //        AxisMoveAbsoluteActionTag = OPCUAHelper.CombinedNodeString("DB_Axis_Flag", axisKey, "MoveAbs")
                                //    }
                                //}
                            }
                        },
                        AxisTeachingModel = new MotorAxisTeachingModel
                        {
                            AxisTeachingPositionList = axisTeachingPositionList
                        },
                        MotorIOModel = motorIOModel
                    });

                    SerializeMotor(motorModel);
                }

                motorModel = XmlTools.Deserialize<MotorModel>(FilePath.Motor);
            }
            catch (Exception ex) { logger.Error(ex); }

            return motorModel;
        }

        //public static List<string> GetMotorTagList(MotorModel motorModel)
        //{
        //    List<string> motorTagList = new List<string>();

        //    try
        //    {
        //        motorTagList = motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralStatusList).Select(
        //            x => x.AxisGeneralStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString())).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisGeneralModel.AxisGeneralNumericList).Select(
        //            x => x.AxisGeneralNumericTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisGeneralModel.AxisGeneralActionList).Select(
        //            x => x.AxisGeneralActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisHomingModel.AxisHomingStatusList).Select(
        //            x => x.AxisHomingStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisHomingModel.AxisHomingActionList).Select(
        //            x => x.AxisHomingActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisPositionModel.AxisPositionStatusList).Select(
        //            x => x.AxisPositionStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisPositionModel.AxisPositionNumericStatusList).Select(
        //            x => x.AxisPositionNumericStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.Select(
        //            x => x.AxisManualJogModel.AxisManualJogTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisManualJogModel.AxisManualJogIntervalList).Select(
        //            x => x.AxisManualJogIntervalTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisManualJogModel.AxisManualJogStatusList).Select(
        //            x => x.AxisManualJogStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisManualJogModel.AxisManualJogNumericList).Select(
        //            x => x.AxisManualJogNumericTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList).Select(
        //            x => x.AxisMoveAbsoluteStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList).Select(
        //            x => x.AxisMoveAbsoluteNumericTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList).Select(
        //            x => x.AxisMoveAbsoluteActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
        //                y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //            x => x.AxisTeachingPositionNumericStatusList).Select(
        //            x => x.AxisTeachingPositionNumericStatusTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //            x => x.AxisTeachingPositionNumericList).Select(
        //            x => x.AxisTeachingPositionNumericTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //            x => x.AxisTeachingPositionStatusList).Select(
        //            x => x.AxisTeachingPositionStatusTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //            x => x.AxisTeachingPositionActionList).Select(
        //            x => x.AxisTeachingPositionActionTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.MotorIOModel.MotorIOInputList).Select(
        //            x => x.MotorIOInputTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();

        //        motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
        //            x => x.MotorIOModel.MotorIOOutputList).Select(
        //            x => x.MotorIOOutputTagModel).SelectMany(
        //            x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                y => y.GetValue(x).ToString()))).ToList();
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //    return motorTagList;
        //}

        public static List<string> GetMotorTagList()
        {
            MotorModel motorModel = DeserializeMotor();
            List<string> motorTagList = new List<string>();

            try
            {
                motorTagList = motorModel.AxisList.SelectMany(x => x.AxisGeneralModel.AxisGeneralStatusList).Select(
                    x => x.AxisGeneralStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString())).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisGeneralModel.AxisGeneralNumericList).Select(
                    x => x.AxisGeneralNumericTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisGeneralModel.AxisGeneralActionList).Select(
                    x => x.AxisGeneralActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisHomingModel.AxisHomingStatusList).Select(
                    x => x.AxisHomingStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisHomingModel.AxisHomingActionList).Select(
                    x => x.AxisHomingActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisPositionModel.AxisPositionStatusList).Select(
                    x => x.AxisPositionStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisPositionModel.AxisPositionNumericStatusList).Select(
                    x => x.AxisPositionNumericStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.Select(
                    x => x.AxisManualJogModel.AxisManualJogTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisManualJogModel.AxisManualJogIntervalList).Select(
                    x => x.AxisManualJogIntervalTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisManualJogModel.AxisManualJogStatusList).Select(
                    x => x.AxisManualJogStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisManualJogModel.AxisManualJogNumericList).Select(
                    x => x.AxisManualJogNumericTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList).Select(
                    x => x.AxisMoveAbsoluteStatusTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList).Select(
                    x => x.AxisMoveAbsoluteNumericTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList).Select(
                    x => x.AxisMoveAbsoluteActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                        y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionNumericStatusList).Select(
                    x => x.AxisTeachingPositionNumericStatusTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionNumericList).Select(
                    x => x.AxisTeachingPositionNumericTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionStatusList).Select(
                    x => x.AxisTeachingPositionStatusTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
                    x => x.AxisTeachingPositionActionList).Select(
                    x => x.AxisTeachingPositionActionTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.MotorIOModel.MotorIOInputList).Select(
                    x => x.MotorIOInputTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

                motorTagList = motorTagList.Union(motorModel.AxisList.SelectMany(
                    x => x.MotorIOModel.MotorIOOutputList).Select(
                    x => x.MotorIOOutputTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();
            }
            catch (Exception ex) { logger.Error(ex); }
            return motorTagList;
        }

        //public static bool ImportMotorAxisTeachingPosition(MotorModel motorModel,
        //    out List<OPCUaTagModel> opcUaList)
        //{
        //    bool value = false;
        //    opcUaList = new List<OPCUaTagModel>();

        //    try
        //    {
        //        if (ShowOpenFileDialog(out string filePath) == CommonFileDialogResult.Ok)
        //        {
        //            string[] motorAxisTeachingPositionCsv = File.ReadAllLines(filePath);

        //            if (motorAxisTeachingPositionCsv.Length > 0)
        //            {
        //                string axisKey = string.Empty;
        //                bool axisTeachingPositionHeader = false;
        //                List<MotorAxisTeachingPositionNumericStatusModel> axisTeachingPositionNumericStatusList =
        //                    new List<MotorAxisTeachingPositionNumericStatusModel>();

        //                for (int i = 0; i < motorAxisTeachingPositionCsv.Length; i++)
        //                {
        //                    try
        //                    {
        //                        if (!string.IsNullOrEmpty(motorAxisTeachingPositionCsv[i]))
        //                        {
        //                            string[] split = motorAxisTeachingPositionCsv[i].Split(Comma);

        //                            if (split.Length == 1)
        //                            {
        //                                if (motorModel.AxisList.Exists(y => y.AxisKey == split[0]))
        //                                {
        //                                    axisKey = motorModel.AxisList.Find(
        //                                        y => y.AxisKey == split[0]).AxisKey;
        //                                }
        //                            }
        //                            else if (split.Length == 2)
        //                            {
        //                                if (motorModel.AxisList.Exists(y => y.AxisKey == axisKey))
        //                                {
        //                                    if (motorModel.AxisList.Find(y => y.AxisKey ==
        //                                    axisKey).AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.Exists(
        //                                        y => y.AxisMoveAbsoluteNumericKey == split[0]))
        //                                    {
        //                                        string axisMoveAbsoluteNumericTag = motorModel.AxisList.Find(
        //                                            y => y.AxisKey == axisKey).AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.Find(
        //                                            y => y.AxisMoveAbsoluteNumericKey ==
        //                                            split[0]).AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag;

        //                                        opcUaList.Add(new OPCUaTagModel
        //                                        {
        //                                            OPCUA_Node = axisMoveAbsoluteNumericTag,
        //                                            OPCUA_Value = split[1]
        //                                        });
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (axisTeachingPositionHeader)
        //                                {
        //                                    axisTeachingPositionHeader = false;

        //                                    if (split.Length == axisTeachingPositionNumericStatusList.Count)
        //                                    {
        //                                        for (int j = 0; j < split.Length; j++)
        //                                        {
        //                                            try
        //                                            {
        //                                                axisTeachingPositionNumericStatusList[j].
        //                                                    AxisTeachingPositionNumericStatus =
        //                                                    Convert.ToDouble(split[j]);
        //                                            }
        //                                            catch (Exception ex) { logger.Error(ex); }
        //                                        }

        //                                        for (int j = 0; j < axisTeachingPositionNumericStatusList.Count; j++)
        //                                        {
        //                                            try
        //                                            {
        //                                                opcUaList.Add(new OPCUaTagModel
        //                                                {
        //                                                    OPCUA_Node = axisTeachingPositionNumericStatusList[j].
        //                                                    AxisTeachingPositionNumericStatusTagModel.
        //                                                    AxisTeachingPositionNumericStatusTag,

        //                                                    OPCUA_Value = axisTeachingPositionNumericStatusList[j].
        //                                                    AxisTeachingPositionNumericStatus.ToString()
        //                                                });
        //                                            }
        //                                            catch (Exception ex) { logger.Error(ex); }
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    axisTeachingPositionHeader = true;
        //                                    axisTeachingPositionNumericStatusList = new List<MotorAxisTeachingPositionNumericStatusModel>();

        //                                    for (int j = 0; j < split.Length; j++)
        //                                    {
        //                                        try
        //                                        {
        //                                            if (motorModel.AxisList.Exists(y => y.AxisKey == axisKey))
        //                                            {
        //                                                if (motorModel.AxisList.Find(y => y.AxisKey ==
        //                                                axisKey).AxisTeachingModel.AxisTeachingPositionList.Exists(
        //                                                    y => y.AxisTeachingPositionKey == split[j]))
        //                                                {
        //                                                    string axisTeachingPositionNumericStatusTag =
        //                                                        motorModel.AxisList.Find(y => y.AxisKey ==
        //                                                        axisKey).AxisTeachingModel.AxisTeachingPositionList.Find(
        //                                                            y => y.AxisTeachingPositionKey == split[j]).
        //                                                            AxisTeachingPositionNumericStatusList[0].
        //                                                            AxisTeachingPositionNumericStatusTagModel.
        //                                                            AxisTeachingPositionNumericStatusTag;

        //                                                    axisTeachingPositionNumericStatusList.Add(new MotorAxisTeachingPositionNumericStatusModel
        //                                                    {
        //                                                        AxisTeachingPositionNumericStatusTagModel = new MotorAxisTeachingPositionNumericStatusTagModel
        //                                                        {
        //                                                            AxisTeachingPositionNumericStatusTag =
        //                                                            axisTeachingPositionNumericStatusTag
        //                                                        }
        //                                                    });
        //                                                }
        //                                            }
        //                                        }
        //                                        catch (Exception ex) { logger.Error(ex); }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex) { logger.Error(ex); }
        //                }
        //                value = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //    return value;
        //}

        public static bool ExportMotorAxisTeachingPosition(MotorModel motorModel)
        {
            bool value = false;

            try
            {
                string date = DateTime.Now.ToString(ShortDateFormat);

                string folderPath = $@"{FolderPath.Logs}{date}\{MotorAxisTeachingPosition}\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = $"{folderPath}{MotorAxisTeachingPosition}{Underscore}" +
                $"{DateTime.Now.ToString(DateTimeFormat)}.csv";

                using (FileStream fileStream = new FileStream(filePath,
                    FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    motorModel.AxisList.ForEach(x =>
                    {
                        csvWriter.WriteField(x.AxisKey);
                        csvWriter.NextRecord();

                        for (int i = 0; i < x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.Count - 1; i++)
                        {
                            csvWriter.WriteField(x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList[i].AxisMoveAbsoluteNumericKey);
                            csvWriter.WriteField(x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList[i].AxisMoveAbsoluteNumeric);
                            csvWriter.NextRecord();
                        }

                        x.AxisTeachingModel.AxisTeachingPositionList.ForEach(y =>
                        {
                            csvWriter.WriteField(y.AxisTeachingPositionKey);
                        });
                        csvWriter.NextRecord();

                        x.AxisTeachingModel.AxisTeachingPositionList.ForEach(y =>
                        {
                            csvWriter.WriteField(y.AxisTeachingPositionNumericStatusList[0].AxisTeachingPositionNumericStatus);
                        });
                        csvWriter.NextRecord();

                        csvWriter.WriteField(string.Empty);
                        csvWriter.NextRecord();
                    });

                    value = true;
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            return value;
        }

        public static void MessageBoxShowCannotBeNull(string message)
        {
            try
            {
                MessageBox.Show($"{message} Cannot Be Null Or Empty");
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        public static CommonFileDialogResult ShowOpenFileDialog(out string filePath)
        {
            CommonFileDialogResult commonFileDialogResult = CommonFileDialogResult.None;
            filePath = string.Empty;

            try
            {
                using (CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog())
                {
                    commonFileDialogResult = commonOpenFileDialog.ShowDialog();

                    if (commonFileDialogResult == CommonFileDialogResult.Ok)
                    {
                        filePath = commonOpenFileDialog.FileName;

                        if (string.IsNullOrEmpty(filePath))
                        {
                            commonFileDialogResult = CommonFileDialogResult.None;
                        }
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            return commonFileDialogResult;
        }

        public static string GetIconSource(string icon)
        {
            string iconSource = string.Empty;
            iconSource = $"{ResourceDictionarySource}Icons/{icon}.png";
            return iconSource;
        }
        #endregion
    }
}