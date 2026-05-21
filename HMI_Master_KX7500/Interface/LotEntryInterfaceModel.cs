using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.LotEntry;
using HMI_Master_KX7500.Service.LotEntry;

namespace HMI_Master_KX7500.Interface
{
    public class LotEntryInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly LotEntryService lotEntryService = new LotEntryService();
        //private readonly LotEntryEditorView lotEntryEditorView = new LotEntryEditorView();
        #endregion

        #region PublicProperties
        private LotEntryModel lotEntryModel = new LotEntryModel();
        public LotEntryModel LotEntryModel
        {
            get { return lotEntryModel; }
            set
            {
                if (lotEntryModel != value)
                {
                    lotEntryModel = value;
                    RaisePropertyChanged(nameof(LotEntryModel));
                }
            }
        }
        #endregion

        #region Constructor
        public LotEntryInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            //MethodHelper.Action(GetType().Name, nameof(initializeCommand),
            //    () => EditLotEntryCommand = new RelayCommand(editLotEntryCommand));
        }

        private void initializeMessenger()
        {
            //MethodHelper.Action(GetType().Name, nameof(initializeMessenger),
            //    () => Messenger.Default.Register<UserAccountModel>(this, updateUserAccount));
        }
        #endregion

        #region PrivateMessengerMethods
        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditLotEntry = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.LotEntryEditor));
        //    });
        //}
        #endregion
    }
}
