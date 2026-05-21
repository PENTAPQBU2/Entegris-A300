using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace HMI_Master_KX7500.Model
{
    public enum MotorUpdate
    {
        ImportMotorAxisTeachingPosition
    }

    public class MotorInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly MotorService motorService = new MotorService();
        #endregion

        #region PublicCommandProperties
        public ICommand ImportMotorAxisTeachingPositionCommand { get; set; }

        public ICommand ExportMotorAxisTeachingPositionCommand { get; set; }

        public ICommand EditMotorCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool allowImportMotorAxisTeachingPosition = false;
        public bool AllowImportMotorAxisTeachingPosition
        {
            get { return allowImportMotorAxisTeachingPosition; }
            set
            {
                if (allowImportMotorAxisTeachingPosition != value)
                {
                    allowImportMotorAxisTeachingPosition = value;
                    RaisePropertyChanged(nameof(AllowImportMotorAxisTeachingPosition));
                }
            }
        }

        private bool allowExportMotorAxisTeachingPosition = false;
        public bool AllowExportMotorAxisTeachingPosition
        {
            get { return allowExportMotorAxisTeachingPosition; }
            set
            {
                if (allowExportMotorAxisTeachingPosition != value)
                {
                    allowExportMotorAxisTeachingPosition = value;
                    RaisePropertyChanged(nameof(AllowExportMotorAxisTeachingPosition));
                }
            }
        }

        private bool allowEditMotor = false;
        public bool AllowEditMotor
        {
            get { return allowEditMotor; }
            set
            {
                if (allowEditMotor != value)
                {
                    allowEditMotor = value;
                    RaisePropertyChanged(nameof(AllowEditMotor));
                }
            }
        }

        private MotorModel motorModel = new MotorModel();
        public MotorModel MotorModel
        {
            get { return motorModel; }
            set
            {
                if (motorModel != value)
                {
                    motorModel = value;
                    RaisePropertyChanged(nameof(MotorModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public MotorInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            ImportMotorAxisTeachingPositionCommand = new RelayCommand(importMotorAxisTeachingPositionCommand);
            ExportMotorAxisTeachingPositionCommand = new RelayCommand(exportMotorAxisTeachingPositionCommand);
            //EditMotorCommand = new RelayCommand(editMotorCommand);
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateCommandMethods
        private void importMotorAxisTeachingPositionCommand()
        {
            Messenger.Default.Send(MotorUpdate.ImportMotorAxisTeachingPosition);
        }

        private void exportMotorAxisTeachingPositionCommand()
        {
            if (MotorService.ExportMotorAxisTeachingPosition(MotorModel))
            {
                MessageBox.Show("Export motor axis teaching position");
            }
            else
            {
                MessageBox.Show("Export motor axis teaching position");
            }
        }

        //private void editMotorCommand()
        //{
        //    MethodHelper.Execute(GetType().Name, nameof(editMotorCommand),
        //        () => motorEditorView.ShowDialog(), MessageHelper.UserClickedButton("EditMotor"));
        //}
        #endregion

        #region PrivateMessengerMethods
        private void updateLanguage(LanguageModel model = null)
        {
            MotorModel.AxisList.ForEach(x =>
            {
                x.AxisName = HmiHelper.GetResource<string>(x.AxisKey);

                x.AxisGeneralModel.AxisGeneralStatusList.ForEach(y =>
                {
                    y.AxisGeneralStatusName = HmiHelper.GetResource<string>(y.AxisGeneralStatusKey);
                });

                x.AxisGeneralModel.AxisGeneralNumericList.ForEach(y =>
                {
                    y.AxisGeneralNumericName = HmiHelper.GetResource<string>(y.AxisGeneralNumericKey);
                });

                x.AxisGeneralModel.AxisGeneralActionList.ForEach(y =>
                {
                    y.AxisGeneralActionName = HmiHelper.GetResource<string>(y.AxisGeneralActionKey);
                });

                x.AxisHomingModel.AxisHomingStatusList.ForEach(y =>
                {
                    y.AxisHomingStatusName = HmiHelper.GetResource<string>(y.AxisHomingStatusKey);
                });

                x.AxisHomingModel.AxisHomingActionList.ForEach(y =>
                {
                    y.AxisHomingActionName = HmiHelper.GetResource<string>(y.AxisHomingActionKey);
                });

                x.AxisPositionModel.AxisPositionStatusList.ForEach(y =>
                {
                    y.AxisPositionStatusName = HmiHelper.GetResource<string>(y.AxisPositionStatusKey);
                });

                x.AxisPositionModel.AxisPositionNumericStatusList.ForEach(y =>
                {
                    y.AxisPositionNumericStatusName = HmiHelper.GetResource<string>(y.AxisPositionNumericStatusKey);
                });

                x.AxisManualJogModel.AxisManualJogIntervalList.ForEach(y =>
                {
                    y.AxisManualJogIntervalName = HmiHelper.GetResource<string>(y.AxisManualJogIntervalKey);
                });

                x.AxisManualJogModel.AxisManualJogStatusList.ForEach(y =>
                {
                    y.AxisManualJogStatusName = HmiHelper.GetResource<string>(y.AxisManualJogStatusKey);
                });

                x.AxisManualJogModel.AxisManualJogNumericList.ForEach(y =>
                {
                    y.AxisManualJogNumericName = HmiHelper.GetResource<string>(y.AxisManualJogNumericKey);
                });

                x.AxisMoveAbsoluteModel.AxisMoveAbsoluteStatusList.ForEach(y =>
                {
                    y.AxisMoveAbsoluteStatusName = HmiHelper.GetResource<string>(y.AxisMoveAbsoluteStatusKey);
                });

                x.AxisMoveAbsoluteModel.AxisMoveAbsoluteNumericList.ForEach(y =>
                {
                    y.AxisMoveAbsoluteNumericName = HmiHelper.GetResource<string>(y.AxisMoveAbsoluteNumericKey);
                });

                x.AxisMoveAbsoluteModel.AxisMoveAbsoluteActionList.ForEach(y =>
                {
                    y.AxisMoveAbsoluteActionName = HmiHelper.GetResource<string>(y.AxisMoveAbsoluteActionKey);
                });

                x.AxisTeachingModel.AxisTeachingPositionList.ForEach(y =>
                {
                    y.AxisTeachingPositionName = HmiHelper.GetResource<string>(y.AxisTeachingPositionKey);

                    y.AxisTeachingPositionNumericStatusList.ForEach(z =>
                    {
                        z.AxisTeachingPositionNumericStatusName = HmiHelper.GetResource<string>(z.AxisTeachingPositionNumericStatusKey);
                    });

                    y.AxisTeachingPositionNumericList.ForEach(z =>
                    {
                        z.AxisTeachingPositionNumericName = HmiHelper.GetResource<string>(z.AxisTeachingPositionNumericKey);
                    });

                    y.AxisTeachingPositionStatusList.ForEach(z =>
                    {
                        z.AxisTeachingPositionStatusName = HmiHelper.GetResource<string>(z.AxisTeachingPositionStatusKey);
                    });

                    y.AxisTeachingPositionActionList.ForEach(z =>
                    {
                        z.AxisTeachingPositionActionName = HmiHelper.GetResource<string>(z.AxisTeachingPositionActionKey);
                    });
                });

                x.MotorIOModel.MotorIOInputList.ForEach(y =>
                {
                    y.MotorIOInputName = HmiHelper.GetResource<string>(y.MotorIOInputKey);
                });

                x.MotorIOModel.MotorIOOutputList.ForEach(y =>
                {
                    y.MotorIOOutputName = HmiHelper.GetResource<string>(y.MotorIOOutputKey);
                });
            });
        }


        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowImportMotorAxisTeachingPosition = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.MotorImportAxisTeachingPosition));

        //        AllowExportMotorAxisTeachingPosition = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.MotorExportAxisTeachingPosition));

        //        AllowEditMotor = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.MotorEditor));
        //    });
        //}
        #endregion
    }
}