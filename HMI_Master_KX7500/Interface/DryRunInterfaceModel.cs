using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace HMI_Master_KX7500.Model.DryRun
{
    public class DryRunInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        //private readonly DryRunEditorView dryRunEditorView = new DryRunEditorView();
        #endregion

        #region PublicCommandProperties

        public ICommand EditDryRunListCommand { get; set; }

        public ICommand DryRunStartCommand { get; set; }

        public ICommand DryRunStopCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool allowEditDryRunList = false;
        public bool AllowEditDryRunList
        {
            get { return allowEditDryRunList; }
            set
            {
                if (allowEditDryRunList != value)
                {
                    allowEditDryRunList = value;
                    RaisePropertyChanged(nameof(AllowEditDryRunList));
                }
            }
        }

        private ICollectionView dryRunView = null;
        public ICollectionView DryRunView
        {
            get
            {
                dryRunView = null;
                dryRunView = CollectionViewSource.GetDefaultView(DryRunList);
                dryRunView.Filter = x =>
                {
                    DryRunModel dryRunModel = x as DryRunModel;
                    return dryRunModel.DryRunEnable == true;
                };
                return dryRunView;
            }
        }

        private List<DryRunModel> dryRunList = new List<DryRunModel>();
        public List<DryRunModel> DryRunList
        {
            get { return dryRunList; }
            set
            {
                if (dryRunList != value)
                {
                    dryRunList = value;
                    RaisePropertyChanged(nameof(DryRunList));
                }
            }
        }
        #endregion

        #region Constructor
        public DryRunInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            EditDryRunListCommand = new RelayCommand(editDryRunListCommand);
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<DryRunUpdate>(this, updateDryRun);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateCommandMethods
        private void editDryRunListCommand()
        {
            //dryRunEditorView.ShowDialog();
        }
        #endregion

        #region PrivateMessengerMethods
        private void updateDryRun(DryRunUpdate dryRunUpdate)
        {
            switch (dryRunUpdate)
            {
                case DryRunUpdate.Enable:
                    RaisePropertyChanged(nameof(DryRunView));
                    break;
                case DryRunUpdate.Name:
                    updateLanguage();
                    break;
                default:
                    break;
            }
        }

        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditDryRunList = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.DryRunEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            DryRunList.ForEach(x =>
            {

                x.DryRunName = x.DryRunKey;

                x.DryRunTimeList.ToList().ForEach(y =>
                {
                    y.DryRunTimeName = HmiHelper.GetResource<string>(y.DryRunTimeKey);
                });

                x.DryRunCountList.ToList().ForEach(y =>
                {
                    y.DryRunCountName = HmiHelper.GetResource<string>(y.DryRunCountKey);
                });

                x.DryRunActionList.ToList().ForEach(y =>
                {
                    y.DryRunActionName = HmiHelper.GetResource<string>(y.DryRunActionKey);
                });
            });
        }
        #endregion
    }
}
