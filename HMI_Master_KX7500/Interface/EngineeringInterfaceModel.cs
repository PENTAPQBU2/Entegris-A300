using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using System.Windows.Input;

namespace HMI_Master_KX7500.Model
{
    public class EngineeringInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        //private readonly EngineeringEditorView engineeringEditorView = new EngineeringEditorView();
        #endregion

        #region PublicCommandProperties
        public ICommand showEditorCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool allowEditor = false;
        public bool AllowEditor
        {
            get { return allowEditor; }
            set
            {
                if (allowEditor != value)
                {
                    allowEditor = value;
                    RaisePropertyChanged(nameof(AllowEditor));
                }
            }
        }
        private EngineeringMainModel engineeringMainModel = new EngineeringMainModel();
        public EngineeringMainModel EngineeringMainModel
        {
            get { return engineeringMainModel; }
            set
            {
                if (engineeringMainModel != value)
                {
                    engineeringMainModel = value;
                    RaisePropertyChanged(nameof(EngineeringMainModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public EngineeringInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            showEditorCommand = new RelayCommand(showEditorViewCommand);
        }
        private void initializeMessenger()
        {
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            //Messenger.Default.Register(this, updateLanguage);
        }
        #endregion

        #region PrivateCommandMethods
        private void showEditorViewCommand()
        {
            //engineeringEditorView.ShowDialog();
        }
        #endregion

        #region PrivateMessengerMethods
        private void updateLanguage()
        {
            EngineeringMainModel.EngineeringGroupModel.ForEach(x =>
            {
                x.EngineeringGroupName = HmiHelper.GetResource<string>(x.EngineeringGroupKey);
                x.EngineeringGroupToggleList.ForEach(y => y.EngineeringGroupToggleName = HmiHelper.GetResource<string>(y.EngineeringGroupToggleKey));
                x.EngineeringGroupNumericList.ForEach(y => y.EngineeringGroupNumericName = HmiHelper.GetResource<string>(y.EngineeringGroupNumericKey));
                x.EngineeringGroupReadOnlyList.ForEach(y => y.EngineeringGroupReadOnlyName = HmiHelper.GetResource<string>(y.EngineeringGroupReadOnlyKey));
                x.EngineeringGroupTextList.ForEach(y => y.EngineeringGroupTextName = HmiHelper.GetResource<string>(y.EngineeringGroupTextKey));
            });

            RaisePropertyChanged(nameof(EngineeringMainModel));
        }

        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditor = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.EngineeringEditor));

        //        EngineeringMainModel.EngineeringGroupModel.ForEach(x =>
        //        {
        //            if (x.IsAccessControlled)
        //                x.IsAllowedAccess = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.EngineeringPenta));
        //        });
        //    });
        //}

        #endregion
    }
}
