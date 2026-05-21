using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Services;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Linq;
using System.Windows.Input;

namespace HMI_Master_KX7500.Models
{
    public class SettingsGeneralInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly SettingsGeneralService settingsGeneralService = new SettingsGeneralService();
        //private readonly SettingsGeneralEditorView settingsGeneralEditorView = new SettingsGeneralEditorView();
        #endregion

        #region PublicCommandProperties
        public ICommand EditSettingsGeneralCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool allowEditSettingsGeneral = false;
        public bool AllowEditSettingsGeneral
        {
            get { return allowEditSettingsGeneral; }
            set
            {
                if (allowEditSettingsGeneral != value)
                {
                    allowEditSettingsGeneral = value;
                    RaisePropertyChanged(nameof(AllowEditSettingsGeneral));
                }
            }
        }

        private SettingsGeneralModel settingsGeneralModel = new SettingsGeneralModel();
        public SettingsGeneralModel SettingsGeneralModel
        {
            get { return settingsGeneralModel; }
            set
            {
                if (settingsGeneralModel != value)
                {
                    settingsGeneralModel = value;
                    RaisePropertyChanged(nameof(SettingsGeneralModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public SettingsGeneralInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            //MethodHelper.Action(GetType().Name, nameof(initializeCommand),
            //    () => EditSettingsGeneralCommand = new RelayCommand(editSettingsGeneralCommand));
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateCommandMethods
        private void editSettingsGeneralCommand()
        {
            //MethodHelper.Execute(GetType().Name, nameof(editSettingsGeneralCommand),
            //    () => settingsGeneralEditorView.ShowDialog(),
            //    MessageHelper.UserClickedButton("EditSettingsGeneral"));
        }
        #endregion

        #region PrivateMessengerMethods
        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        //AllowEditSettingsGeneral = UserAccountService.IsUserAccountGroupAccessAllow(
        //        //    nameof(UserAccountGroupAccess.SettingsGeneralEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            SettingsGeneralModel.SettingsGeneralGroupList.ForEach(x =>
            {
                x.SettingsGeneralGroupName = HmiHelper.GetResource<string>(x.SettingsGeneralGroupKey);

                x.SettingsGeneralGroupNumericList.ForEach(y =>
                {
                    y.SettingsGeneralGroupNumericName = HmiHelper.GetResource<string>(y.SettingsGeneralGroupNumericKey);
                    //y.SettingsGeneralGroupNumericKey = HmiHelper.GetResource<string>(y.SettingsGeneralGroupNumericKey);
                });

                x.SettingsGeneralGroupToggleList.ForEach(y =>
                {
                    y.SettingsGeneralGroupToggleName = HmiHelper.GetResource<string>(y.SettingsGeneralGroupToggleKey);

                    y.SettingsGeneralGroupToggleOffLabelName = HmiHelper.GetResource<string>(y.SettingsGeneralGroupToggleOffLabelKey);

                    y.SettingsGeneralGroupToggleOnLabelKey = HmiHelper.GetResource<string>(y.SettingsGeneralGroupToggleOnLabelKey);
                });

                x.SettingsGeneralGroupReadOnlyList.ForEach(y =>
                {
                    y.SettingsGeneralGroupReadOnlyName = HmiHelper.GetResource<string>(y.SettingsGeneralGroupReadOnlyKey);

                    y.SettingsGeneralGroupReadOnlyOffLabelName = HmiHelper.GetResource<string>(y.SettingsGeneralGroupReadOnlyOffLabelKey);

                    y.SettingsGeneralGroupReadOnlyOnLabelKey = HmiHelper.GetResource<string>(y.SettingsGeneralGroupReadOnlyOnLabelKey);
                });
            });
        }
        #endregion

    }
}