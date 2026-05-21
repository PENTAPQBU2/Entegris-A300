using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HMI_Master_KX7500
{
    public class IOInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly IOService iOService = new IOService();
        //private readonly IOEditorView iOEditorView = new IOEditorView();
        #endregion

        #region PublicProperties
        public ICommand EditIOCommand { get; set; }

        private bool allowEditIO = false;
        public bool AllowEditIO
        {
            get { return allowEditIO; }
            set 
            {
                if(allowEditIO!=value)
                {
                    allowEditIO = value;
                    RaisePropertyChanged(nameof(AllowEditIO));
                }
            }
        }

        private IOModel ioModel = new IOModel();
        public IOModel IOModel
        {
            get { return ioModel; }
            set 
            {
                if(ioModel!=value)
                {
                    ioModel = value;
                    RaisePropertyChanged(nameof(IOModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public IOInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            EditIOCommand = new RelayCommand(editIOCommand);
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateCommandMethods
        private void editIOCommand()
        {
            //MethodHelper.Action(GetType().Name, nameof(editIOCommand),
            //    () => iOEditorView.ShowDialog(), MessageHelper.UserClickedButton("EditIO"));
        }
        #endregion

        #region PrivateMessengerMethods
        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditIO = UserAccountService.IsUserAccountGroupAccessAllow(nameof(UserAccountGroupAccess.IOEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            IOModel.IOItemsModels.ForEach(x =>
            {
                x.IOInputList.ForEach(y => y.IO_InputName = y.IOInputKey);
                x.IOOutputList.ForEach(y => y.IOOutputName = y.IOOutputKey);
                //x.IOInputList.ForEach(y => y.IO_InputName = HmiHelper.GetResource<string>(y.IOInputKey));
                //x.IOOutputList.ForEach(y => y.IOOutputName = HmiHelper.GetResource<string>(y.IOOutputKey));
            });
        }
        #endregion
    }
}
