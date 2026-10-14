using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.MachinePath;
using MahApps.Metro.Controls;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HMI_Master_KX7500.Model.Motor
{
    public class Motor_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<string> tagList = new List<string>();
        //private OPCUA_ opcUA = new OPCUA_();

        private KeyenceDll keyenceDll = null;
        #endregion

        #region PublicProperties
        private MotorInterfaceModel motorInterfaceModel = new MotorInterfaceModel();
        public MotorInterfaceModel MotorInterfaceModel
        {
            get { return motorInterfaceModel; }
            set
            {
                if (motorInterfaceModel != value)
                {
                    motorInterfaceModel = value;
                    RaisePropertyChanged(nameof(MotorInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public Motor_OPCUA_ViewModel()
        {
            try
            {
                initializeMessenger();
                //Messenger.Default.Send(new MotorInitializeModel
                //{
                //    MotorFilePath = FilePath.Motor
                //});

                keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());
            }
            catch (Exception ex) { logger.Error(ex); }

            Event.UpdateEvent += Update;
        }

        ~Motor_OPCUA_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            if (Global.PageList["Motor"])
            {
                MotorInterfaceModel.MotorModel.AxisList.AsParallel().ForAll(x =>
                {
                    foreach (var y in x.AxisGeneralModel.AxisGeneralStatusList)
                    {
                        y.AxisGeneralStatus = keyenceDll.Read<bool>(y.AxisGeneralStatusTagModel.AxisGeneralStatusTag).ToString();
                    }
                    foreach (var y in x.AxisGeneralModel.AxisGeneralNumericList)
                    {
                        y.AxisGeneralNumeric = keyenceDll.Read<double>(y.AxisGeneralNumericTagModel.AxisGeneralNumericTag);
                    }
                    foreach (var y in x.AxisHomingModel.AxisHomingStatusList)
                    {
                        y.AxisHomingStatus = keyenceDll.Read<bool>(y.AxisHomingStatusTagModel.AxisHomingStatusTag).ToString();
                    }
                    foreach (var y in x.AxisPositionModel.AxisPositionStatusList)
                    {
                        y.AxisPositionStatus = keyenceDll.Read<bool>(y.AxisPositionStatusTagModel.AxisPositionStatusTag).ToString();
                    }
                    foreach (var y in x.AxisPositionModel.AxisPositionNumericStatusList)
                    {
                        y.AxisPositionNumericStatus = keyenceDll.Read<double>(y.AxisPositionNumericStatusTagModel.AxisPositionNumericStatusTag);
                    }
                    x.AxisManualJogModel.AxisManualJog = keyenceDll.Read<bool>(x.AxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag);
                    ///--------------------------------------------------------------
                    //foreach(var y in x.AxisManualJogModel.AxisManualJogIntervalList)
                    //{
                    //    y.AxisManualJogInterval = keyenceDll.Read<bool>(y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag);


                    //    foreach(var z in y.)
                    //    x.AxisManualJogModel.AxisManualJogIntervalList
                    //}
                    ///
                    foreach (var y in x.AxisManualJogModel.AxisManualJogStatusList)
                    {
                        y.AxisManualJogStatus = keyenceDll.Read<bool>(y.AxisManualJogStatusTagModel.AxisManualJogStatusTag).ToString();
                    }
                    foreach (var y in x.AxisManualJogModel.AxisManualJogNumericList)
                    {
                        y.AxisManualJogNumeric = keyenceDll.Read<double>(y.AxisManualJogNumericTagModel.AxisManualJogNumericTag);
                    }
                    foreach (var y in x.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList)
                    {
                        y.AxisMoveAbsoluteStatus = keyenceDll.Read<bool>(y.AxisMoveAbsoluteStatusTagModel.AxisMoveAbsoluteStatusTag).ToString();
                    }
                    foreach (var y in x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList)
                    {
                        y.AxisMoveAbsoluteNumeric = keyenceDll.Read<double>(y.AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag);
                    }
                    foreach (var y in x.AxisTeachingModel.AxisTeachingPositionList)
                    {
                        foreach (var z in y.AxisTeachingPositionNumericStatusList)
                        {
                            z.AxisTeachingPositionNumericStatus = keyenceDll.Read<double>(z.AxisTeachingPositionNumericStatusTagModel.AxisTeachingPositionNumericStatusTag);
                        }
                    }
                    foreach (var y in x.AxisTeachingModel.AxisTeachingPositionList)
                    {
                        foreach (var z in y.AxisTeachingPositionNumericList)
                        {
                            z.AxisTeachingPositionNumeric = keyenceDll.Read<double>(z.AxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag);
                        }
                    }
                    foreach (var y in x.AxisTeachingModel.AxisTeachingPositionList)
                    {
                        foreach (var z in y.AxisTeachingPositionStatusList)
                        {
                            z.AxisTeachingPositionStatus = keyenceDll.Read<bool>(z.AxisTeachingPositionStatusTagModel.AxisTeachingPositionStatusTag).ToString();
                        }
                    }
                    foreach (var y in x.MotorIOModel.MotorIOInputList)
                    {
                        y.MotorIOInputValue = keyenceDll.Read<bool>(y.MotorIOInputTagModel.MotorIOInputTag).ToString();
                    }
                    foreach (var y in x.MotorIOModel.MotorIOOutputList)
                    {
                        y.MotorIOOutputValue = keyenceDll.Read<bool>(y.MotorIOOutputTagModel.MotorIOOutputViewTag).ToString();
                    }
                    ///--------------------------------------------------------------                  
                    //else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
                    //            y => y.AxisManualJogModel.AxisManualJogIntervalList).ToList().Exists(
                    //            y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key))
                    //{
                    //    MotorInterfaceModel.MotorModel.AxisList.SelectMany(
                    //        y => y.AxisManualJogModel.AxisManualJogIntervalList).ToList().Find(
                    //        y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key).AxisManualJogInterval = Convert.ToBoolean(x.Value);

                    //    MotorInterfaceModel.MotorModel.AxisList.Select(
                    //        y => y.AxisManualJogModel).ToList().Find(
                    //        y => y.AxisManualJogIntervalList.Exists(
                    //            z => z.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key)).AxisManualJogIntervalSelectedIndex =
                    //            MotorInterfaceModel.MotorModel.AxisList.Select(
                    //                z => z.AxisManualJogModel).ToList().Find(
                    //                z => z.AxisManualJogIntervalList.Exists(
                    //                    y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key)).AxisManualJogIntervalList.Exists(
                    //                y => y.AxisManualJogInterval == true) ?
                    //                MotorInterfaceModel.MotorModel.AxisList.Select(
                    //                    z => z.AxisManualJogModel).ToList().Find(
                    //                    z => z.AxisManualJogIntervalList.Exists(
                    //                        y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key)).AxisManualJogIntervalList.FindIndex(
                    //                    y => y.AxisManualJogInterval == true) : -1;
                    //}
                    ///--------------------------------------------------------------                 
                });
            }            
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            try
            {
                initializeMotor();
                //Messenger.Default.Register<MotorInitializeModel>(this, initializeMotor);
                //Messenger.Default.Register<MotorUpdate>(this, updateMotor);
                //Messenger.Default.Register<_OPC>(this, opcUpdate);
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        //private void initializeOpcUa(OPCUA_ opcUa_)
        //{
        //    try
        //    {
        //        if (opcUa_.OPC.Module.Contains(OPCModules.Motor))
        //        {
        //            //opcUA = opcUa_;
        //            //tagList = opcUA.GetAddresses();
        //        }

        //        //tagList = MotorService.GetMotorTagList(MotorInterfaceModel.MotorModel);

        //        //if (Global.MachineConfigModel.InitializeOPCUA)
        //        //{
        //        //    opcUA.InitializeUAOPC(Global.OPCUA_Url, tagList, new List<OPCModules> {OPCModules.Motor});
        //        //}
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateCommandMethods
        private void axisSelectionChangedCommand(ComboBox comboBox)
        {
            try
            {
                if (comboBox.SelectedItem != null)
                {
                    MotorAxisModel motorAxisModel = comboBox.SelectedItem as MotorAxisModel;

                    MotorInterfaceModel.MotorModel.AxisList.ForEach(x =>
                    {
                        //opcUA.Write(x.AxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag, false);
                        keyenceDll.Write(x.AxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag, false);
                    });
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void setAxisGeneralNumericCommand(NumericUpDown numericUpDown)
        {
            try
            {
                MotorAxisGeneralNumericModel motorAxisGeneralNumericModel =
                       numericUpDown.Tag as MotorAxisGeneralNumericModel;

                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                    x => x.AxisGeneralModel.AxisGeneralNumericList.Exists(
                        y => y == motorAxisGeneralNumericModel)))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisGeneralModel.AxisGeneralNumericList.Exists(
                            y => y == motorAxisGeneralNumericModel));

                    if (MessageBox.Show($"Are You Sure You Want To set {motorAxisModel.AxisName}'s", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisGeneralNumericModel.AxisGeneralNumericTagModel.AxisGeneralNumericTag, numericUpDown.Value);
                        keyenceDll.Write(motorAxisGeneralNumericModel.AxisGeneralNumericTagModel.AxisGeneralNumericTag, numericUpDown.Value);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void axisGeneralActionCommand(MotorAxisGeneralActionModel motorAxisGeneralActionModel)
        {
            try
            {
                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                            x => x.AxisGeneralModel.AxisGeneralActionList.Exists(
                                y => y == motorAxisGeneralActionModel)))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisGeneralModel.AxisGeneralActionList.Exists(
                            y => y == motorAxisGeneralActionModel));

                    string motorAxisName = $"{motorAxisGeneralActionModel.AxisGeneralActionName}{motorAxisModel.AxisName}";

                    if (MessageBox.Show($"Are You Sure You Want To set {motorAxisName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisGeneralActionModel.AxisGeneralActionTagModel.AxisGeneralActionTag, true);
                        keyenceDll.Write(motorAxisGeneralActionModel.AxisGeneralActionTagModel.AxisGeneralActionTag, true);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void axisHomingActionCommand(MotorAxisHomingActionModel motorAxisHomingActionModel)
        {
            try
            {
                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                           x => x.AxisHomingModel.AxisHomingActionList.Exists(
                               y => y == motorAxisHomingActionModel)))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisHomingModel.AxisHomingActionList.Exists(
                            y => y == motorAxisHomingActionModel));

                    string homingName = $"{motorAxisHomingActionModel.AxisHomingActionName} {motorAxisModel.AxisName}";

                    if (MessageBox.Show($"Are You Sure You Want To set {homingName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisHomingActionModel.AxisHomingActionTagModel.AxisHomingActionTag, true);
                        keyenceDll.Write(motorAxisHomingActionModel.AxisHomingActionTagModel.AxisHomingActionTag, true);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void axisManualJogOffCommand(MotorAxisManualJogModel motorAxisManualJogModel)
        {
            try
            {
                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                            x => x.AxisManualJogModel == motorAxisManualJogModel))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisManualJogModel == motorAxisManualJogModel);

                    if (MessageBox.Show($"Are You Sure You Want To Off {motorAxisModel.AxisName}'s Manual Jog", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag, false);
                        keyenceDll.Write(motorAxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag, false);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void axisManualJogOnCommand(MotorAxisManualJogModel motorAxisManualJogModel)
        {
            try
            {
                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                            x => x.AxisManualJogModel == motorAxisManualJogModel))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisManualJogModel == motorAxisManualJogModel);

                    if (MessageBox.Show($"Are You Sure You Want To On {motorAxisModel.AxisName}'s Manual Jog", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag, true);
                        keyenceDll.Write(motorAxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag, true);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }


        //private void axisManualJogStatusCommand(MotorAxisManualJogStatusModel motorAxisManualJogStatusModel)
        //{
        //    try
        //    {
        //        keyenceDll.Write<bool>(motorAxisManualJogModel.AxisManualJogStatusList, Global.ConvertBool(motorAxisManualJogStatusModel.AxisManualJogStatus));
        //        keyenceDll.Write<bool>(iOOutputModel.IOOutputTagModel.IOOutputTag, Global.ConvertBool(iOOutputModel.IO_OutputValue));
        //        //opcUa.Write(iOOutputModel.IOOutputTagModel.IOOutputTag, Global.ConvertBool(iOOutputModel.IO_OutputValue));
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}


        private void setAxisManualJogIntervalCommand(ComboBox comboBox)
        {
            try
            {
                MotorAxisManualJogModel motorAxisManualJogModel = comboBox.Tag as MotorAxisManualJogModel;

                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                    x => x.AxisManualJogModel == motorAxisManualJogModel))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisManualJogModel == motorAxisManualJogModel);

                    if (MessageBox.Show($"Are You Sure You Want To Set {motorAxisModel.AxisName}'s Manual Jog Interval", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        motorAxisManualJogModel.AxisManualJogIntervalList.ForEach(x =>
                        {
                            //opcUA.Write(x.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag, x == comboBox.SelectedItem ? true : false);
                            keyenceDll.Write(x.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag, x == comboBox.SelectedItem ? true : false);
                        });
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void setAxisManualJogNumericCommand(NumericUpDown numericUpDown)
        {
            try
            {
                MotorAxisManualJogNumericModel motorAxisManualJogNumericModel =
                     numericUpDown.Tag as MotorAxisManualJogNumericModel;

                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                    x => x.AxisManualJogModel.AxisManualJogNumericList.Exists(
                        y => y == motorAxisManualJogNumericModel)))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisManualJogModel.AxisManualJogNumericList.Exists(
                            y => y == motorAxisManualJogNumericModel));

                    string axisName = $"{motorAxisModel.AxisName}'s {motorAxisManualJogNumericModel.AxisManualJogNumericName}";

                    if (MessageBox.Show($"Are You Sure You Want To Set {axisName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisManualJogNumericModel.AxisManualJogNumericTagModel.AxisManualJogNumericTag, numericUpDown.Value);
                        keyenceDll.Write(motorAxisManualJogNumericModel.AxisManualJogNumericTagModel.AxisManualJogNumericTag, numericUpDown.Value);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void setAxisMoveAbsoluteNumericCommand(NumericUpDown numericUpDown)
        {
            try
            {
                MotorAxisMoveAbsoluteNumericModel motorAxisMoveAbsoluteNumericModel =
                        numericUpDown.Tag as MotorAxisMoveAbsoluteNumericModel;

                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                    x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.Exists(
                        y => y == motorAxisMoveAbsoluteNumericModel)))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.Exists(
                            y => y == motorAxisMoveAbsoluteNumericModel));

                    string axisName = $"{motorAxisModel.AxisName}'s {motorAxisMoveAbsoluteNumericModel.AxisMoveAbsoluteNumericName}";

                    if (MessageBox.Show($"Are You Sure You Want To Set {axisName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisMoveAbsoluteNumericModel.AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag, numericUpDown.Value);
                        keyenceDll.Write(motorAxisMoveAbsoluteNumericModel.AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag, numericUpDown.Value);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void axisMoveAbsoluteActionCommand(MotorAxisMoveAbsoluteActionModel motorAxisMoveAbsoluteActionModel)
        {
            try
            {
                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                          x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList.Exists(
                              y => y == motorAxisMoveAbsoluteActionModel)))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList.Exists(
                            y => y == motorAxisMoveAbsoluteActionModel));

                    string absoluteName = $"{motorAxisMoveAbsoluteActionModel.AxisMoveAbsoluteActionName} {motorAxisModel.AxisName}";

                    if (MessageBox.Show($"Are You Sure You Want To Set {absoluteName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisMoveAbsoluteActionModel.AxisMoveAbsoluteActionTagModel.AxisMoveAbsoluteActionTag, true);
                        keyenceDll.Write(motorAxisMoveAbsoluteActionModel.AxisMoveAbsoluteActionTagModel.AxisMoveAbsoluteActionTag, true);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void setAxisTeachingPositionNumericCommand(NumericUpDown numericUpDown)
        {
            try
            {
                MotorAxisTeachingPositionNumericModel motorAxisTeachingPositionNumericModel =
                      numericUpDown.Tag as MotorAxisTeachingPositionNumericModel;

                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                    x => x.AxisTeachingModel.AxisTeachingPositionList.Exists(
                        y => y.AxisTeachingPositionNumericList.Exists(
                            z => z == motorAxisTeachingPositionNumericModel))))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisTeachingModel.AxisTeachingPositionList.Exists(
                            y => y.AxisTeachingPositionNumericList.Exists(
                                z => z == motorAxisTeachingPositionNumericModel)));

                    string positionName = $"{motorAxisModel.AxisName}'s {motorAxisTeachingPositionNumericModel.AxisTeachingPositionNumericName}";

                    if (MessageBox.Show($"Are You Sure You Want To Set {positionName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisTeachingPositionNumericModel.AxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag, numericUpDown.Value);
                        keyenceDll.Write(motorAxisTeachingPositionNumericModel.AxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag, numericUpDown.Value);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void axisTeachingPositionActionCommand(MotorAxisTeachingPositionActionModel motorAxisTeachingPositionActionModel)
        {
            try
            {
                if (MotorInterfaceModel.MotorModel.AxisList.Exists(
                            x => x.AxisTeachingModel.AxisTeachingPositionList.Exists(
                                y => y.AxisTeachingPositionActionList.Exists(
                                    z => z == motorAxisTeachingPositionActionModel))))
                {
                    MotorAxisModel motorAxisModel = MotorInterfaceModel.MotorModel.AxisList.Find(
                        x => x.AxisTeachingModel.AxisTeachingPositionList.Exists(
                            y => y.AxisTeachingPositionActionList.Exists(
                                z => z == motorAxisTeachingPositionActionModel)));

                    string teachingName = $"{motorAxisTeachingPositionActionModel.AxisTeachingPositionActionName} {motorAxisModel.AxisName}";

                    if (MessageBox.Show($"Are You Sure You Want To Set {teachingName}", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                    {
                        //opcUA.Write(motorAxisTeachingPositionActionModel.AxisTeachingPositionActionTagModel.AxisTeachingPositionActionTag, true);
                        keyenceDll.Write(motorAxisTeachingPositionActionModel.AxisTeachingPositionActionTagModel.AxisTeachingPositionActionTag, true);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void motorIOOutputCommand(MotorIOOutputModel motorIOOutputModel)
        {
            try
            {
                if (MessageBox.Show($"Are You Sure You Want To Set {motorIOOutputModel.MotorIOOutputName}", "Confirmation",
                           MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                {
                    //opcUA.Write(motorIOOutputModel.MotorIOOutputTagModel.MotorIOOutputTag, Global.ConvertBool(motorIOOutputModel.MotorIOOutputValue));
                    //opcUA.Write(motorIOOutputModel.MotorIOOutputTagModel.MotorIOOutputTag, true);
                    keyenceDll.Write(motorIOOutputModel.MotorIOOutputTagModel.MotorIOOutputTag, true);
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeMotor(/*MotorInitializeModel motorInitializeModel*/)
        {
            try
            {
                MotorInterfaceModel.MotorModel = MotorService.DeserializeMotor();
                MotorInterfaceModel.MotorModel.AxisSelectionChangedCommand = new RelayCommand<ComboBox>(
                    axisSelectionChangedCommand);
                MotorInterfaceModel.MotorModel.AxisList.ForEach(x =>
                {
                    x.AxisGeneralModel.AxisGeneralNumericList.ForEach(y =>
                    {
                        y.SetAxisGeneralNumericCommand = new RelayCommand<NumericUpDown>(
                                setAxisGeneralNumericCommand);
                    });

                    x.AxisGeneralModel.AxisGeneralActionList.ForEach(y =>
                    {
                        y.AxisGeneralActionCommand = new RelayCommand<MotorAxisGeneralActionModel>(
                                axisGeneralActionCommand);
                    });

                    x.AxisHomingModel.AxisHomingActionList.ForEach(y =>
                    {
                        y.AxisHomingActionCommand = new RelayCommand<MotorAxisHomingActionModel>(
                                axisHomingActionCommand);
                    });

                    x.AxisManualJogModel.AxisManualJogOffCommand = new RelayCommand<MotorAxisManualJogModel>(
                        axisManualJogOffCommand);

                    x.AxisManualJogModel.AxisManualJogOnCommand = new RelayCommand<MotorAxisManualJogModel>(
                        axisManualJogOnCommand);

                    //
                    //x.AxisManualJogStatusModel.AxisManualJogStatusCommand = new RelayCommand<MotorAxisManualJogModel>(
                    //    axisManualJogStatusCommand);
                    //

                    x.AxisManualJogModel.SetAxisManualJogIntervalCommand = new RelayCommand<ComboBox>(
                        setAxisManualJogIntervalCommand);

                    x.AxisManualJogModel.AxisManualJogNumericList.ForEach(y =>
                    {
                        y.SetAxisManualJogNumericCommand = new RelayCommand<NumericUpDown>(
                                setAxisManualJogNumericCommand);
                    });

                    x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.ForEach(y =>
                    {
                        y.SetAxisMoveAbsoluteNumericCommand = new RelayCommand<NumericUpDown>(
                                setAxisMoveAbsoluteNumericCommand);
                    });

                    x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList.ForEach(y =>
                    {
                        y.AxisMoveAbsoluteActionCommand = new RelayCommand<MotorAxisMoveAbsoluteActionModel>(
                                axisMoveAbsoluteActionCommand);
                    });

                    x.AxisTeachingModel.AxisTeachingPositionList.ForEach(y =>
                    {
                        y.AxisTeachingPositionNumericList.ForEach(z =>
                        {
                            z.SetAxisTeachingPositionNumericCommand = new RelayCommand<NumericUpDown>(
                                    setAxisTeachingPositionNumericCommand);
                        });

                        y.AxisTeachingPositionActionList.ForEach(z =>
                        {
                            z.AxisTeachingPositionActionCommand = new RelayCommand<MotorAxisTeachingPositionActionModel>(
                                    axisTeachingPositionActionCommand);
                        });
                    });

                    x.MotorIOModel.MotorIOOutputList.ForEach(y =>
                    {
                        y.MotorIOOutputCommand = new RelayCommand<MotorIOOutputModel>(motorIOOutputCommand);
                    });
                });
                //Messenger.Default.Register<OPCUA_>(this, initializeOpcUa);
                //initializeOPC_UA();
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        //private void updateMotor(MotorUpdate motorUpdate)
        //{
        //    try
        //    {
        //        switch (motorUpdate)
        //        {
        //            case MotorUpdate.ImportMotorAxisTeachingPosition:
        //                if (MotorService.ImportMotorAxisTeachingPosition(MotorInterfaceModel.MotorModel,
        //                    out List<OPCUaTagModel> opcUaNodeList))
        //                {
        //                    opcUaNodeList.ForEach(x =>
        //                    {
        //                        opcUA.Write(x.OPCUA_Node, x.OPCUA_Value);

        //                        Thread.Sleep(2);
        //                    });

        //                    MessageBox.Show("Import motor axis teaching position");
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Fail import motor axis teaching position");
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}

        //private void opcUpdate(_OPC _oPC)
        //{
        //    try
        //    {
        //        if (_oPC.Module.Contains(OPCModules.Motor))
        //        {
        //            Task.Run(() =>
        //            {
        //                _oPC.Datachange.ToList().ForEach(x =>
        //                {
        //                    if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisGeneralModel.AxisGeneralStatusList).ToList().Exists(
        //                                y => y.AxisGeneralStatusTagModel.AxisGeneralStatusTag ==
        //                                x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisGeneralModel.AxisGeneralStatusList).ToList().Find(
        //                            y => y.AxisGeneralStatusTagModel.AxisGeneralStatusTag == x.Key).AxisGeneralStatus = x.Value;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisGeneralModel.AxisGeneralNumericList).ToList().Exists(
        //                                y => y.AxisGeneralNumericTagModel.AxisGeneralNumericTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisGeneralModel.AxisGeneralNumericList).ToList().Find(
        //                            y => y.AxisGeneralNumericTagModel.AxisGeneralNumericTag == x.Key).AxisGeneralNumeric = Convert.ToDouble(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisHomingModel.AxisHomingStatusList).ToList().Exists(
        //                                y => y.AxisHomingStatusTagModel.AxisHomingStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisHomingModel.AxisHomingStatusList).ToList().Find(
        //                            y => y.AxisHomingStatusTagModel.AxisHomingStatusTag == x.Key).AxisHomingStatus = x.Value;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisPositionModel.AxisPositionStatusList).ToList().Exists(
        //                                y => y.AxisPositionStatusTagModel.AxisPositionStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisPositionModel.AxisPositionStatusList).ToList().Find(
        //                            y => y.AxisPositionStatusTagModel.AxisPositionStatusTag == x.Key).AxisPositionStatus = x.Value;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisPositionModel.AxisPositionNumericStatusList).ToList().Exists(
        //                                y => y.AxisPositionNumericStatusTagModel.AxisPositionNumericStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisPositionModel.AxisPositionNumericStatusList).ToList().Find(
        //                            y => y.AxisPositionNumericStatusTagModel.AxisPositionNumericStatusTag == x.Key).AxisPositionNumericStatus = Convert.ToDouble(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.Exists(
        //                                y => y.AxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.Find(
        //                            y => y.AxisManualJogModel.AxisManualJogTagModel.AxisManualJogTag == x.Key).AxisManualJogModel.AxisManualJog = Convert.ToBoolean(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisManualJogModel.AxisManualJogIntervalList).ToList().Exists(
        //                                y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisManualJogModel.AxisManualJogIntervalList).ToList().Find(
        //                            y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key).AxisManualJogInterval = Convert.ToBoolean(x.Value);

        //                        MotorInterfaceModel.MotorModel.AxisList.Select(
        //                            y => y.AxisManualJogModel).ToList().Find(
        //                            y => y.AxisManualJogIntervalList.Exists(
        //                                z => z.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key)).AxisManualJogIntervalSelectedIndex =
        //                                MotorInterfaceModel.MotorModel.AxisList.Select(
        //                                    z => z.AxisManualJogModel).ToList().Find(
        //                                    z => z.AxisManualJogIntervalList.Exists(
        //                                        y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key)).AxisManualJogIntervalList.Exists(
        //                                    y => y.AxisManualJogInterval == true) ?
        //                                    MotorInterfaceModel.MotorModel.AxisList.Select(
        //                                        z => z.AxisManualJogModel).ToList().Find(
        //                                        z => z.AxisManualJogIntervalList.Exists(
        //                                            y => y.AxisManualJogIntervalTagModel.AxisManualJogIntervalTag == x.Key)).AxisManualJogIntervalList.FindIndex(
        //                                        y => y.AxisManualJogInterval == true) : -1;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisManualJogModel.AxisManualJogStatusList).ToList().Exists(
        //                                y => y.AxisManualJogStatusTagModel.AxisManualJogStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisManualJogModel.AxisManualJogStatusList).ToList().Find(
        //                            y => y.AxisManualJogStatusTagModel.AxisManualJogStatusTag == x.Key).AxisManualJogStatus = x.Value;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisManualJogModel.AxisManualJogNumericList).ToList().Exists(
        //                                y => y.AxisManualJogNumericTagModel.AxisManualJogNumericTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisManualJogModel.AxisManualJogNumericList).ToList().Find(
        //                            y => y.AxisManualJogNumericTagModel.AxisManualJogNumericTag == x.Key).AxisManualJogNumeric = Convert.ToDouble(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList).ToList().Exists(
        //                                y => y.AxisMoveAbsoluteStatusTagModel.AxisMoveAbsoluteStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList).ToList().Find(
        //                            y => y.AxisMoveAbsoluteStatusTagModel.AxisMoveAbsoluteStatusTag == x.Key).AxisMoveAbsoluteStatus = x.Value;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList).ToList().Exists(
        //                                y => y.AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList).ToList().Find(
        //                            y => y.AxisMoveAbsoluteNumericTagModel.AxisMoveAbsoluteNumericTag == x.Key).AxisMoveAbsoluteNumeric = Convert.ToDouble(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //                                y => y.AxisTeachingPositionNumericStatusList).ToList().Exists(
        //                                y => y.AxisTeachingPositionNumericStatusTagModel.AxisTeachingPositionNumericStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //                            y => y.AxisTeachingPositionNumericStatusList).ToList().Find(
        //                            y => y.AxisTeachingPositionNumericStatusTagModel.AxisTeachingPositionNumericStatusTag == x.Key).AxisTeachingPositionNumericStatus = Convert.ToDouble(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //                                y => y.AxisTeachingPositionNumericList).ToList().Exists(
        //                                y => y.AxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //                            y => y.AxisTeachingPositionNumericList).ToList().Find(
        //                            y => y.AxisTeachingPositionNumericTagModel.AxisTeachingPositionNumericTag == x.Key).AxisTeachingPositionNumeric = Convert.ToDouble(x.Value);
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //                                y => y.AxisTeachingPositionStatusList).ToList().Exists(
        //                                y => y.AxisTeachingPositionStatusTagModel.AxisTeachingPositionStatusTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                            y => y.AxisTeachingModel.AxisTeachingPositionList).SelectMany(
        //                            y => y.AxisTeachingPositionStatusList).ToList().Find(
        //                            y => y.AxisTeachingPositionStatusTagModel.AxisTeachingPositionStatusTag == x.Key).AxisTeachingPositionStatus = x.Value;
        //                    }

        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.MotorIOModel.MotorIOInputList).ToList().Exists(
        //                                y => y.MotorIOInputTagModel.MotorIOInputTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                        y => y.MotorIOModel.MotorIOInputList).ToList().Find(
        //                        y => y.MotorIOInputTagModel.MotorIOInputTag == x.Key).MotorIOInputValue = x.Value;
        //                    }

        //                    //else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                    //            y => y.MotorIOModel.MotorIOOutputList).ToList().Exists(
        //                    //            y => y.MotorIOOutputTagModel.MotorIOOutputTag == x.Key))
        //                    //{
        //                    //    MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                    //    y => y.MotorIOModel.MotorIOOutputList).ToList().Find(
        //                    //    y => y.MotorIOOutputTagModel.MotorIOOutputTag == x.Key).MotorIOOutputValue = x.Value;
        //                    //}                            
        //                    else if (MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                                y => y.MotorIOModel.MotorIOOutputList).ToList().Exists(
        //                                y => y.MotorIOOutputTagModel.MotorIOOutputViewTag == x.Key))
        //                    {
        //                        MotorInterfaceModel.MotorModel.AxisList.SelectMany(
        //                        y => y.MotorIOModel.MotorIOOutputList).ToList().Find(
        //                        y => y.MotorIOOutputTagModel.MotorIOOutputViewTag == x.Key).MotorIOOutputValue = x.Value;
        //                    }
        //                });
        //            });
        //        }
                        
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion
    }
}
