using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace HMI_Master_KX7500.Models
{
    public class ComponentLifeCycleInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        //private readonly ComponentLifeCycleEditorView componentLifeCycleEditorView = new ComponentLifeCycleEditorView();
        #endregion

        #region PublicCommandProperties
        public ICommand EditComponentLifeCycleListCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool allowEditComponentLifeCycleList = false;
        public bool AllowEditComponentLifeCycleList
        {
            get { return allowEditComponentLifeCycleList; }
            set
            {
                if (allowEditComponentLifeCycleList != value)
                {
                    allowEditComponentLifeCycleList = value;
                    RaisePropertyChanged(nameof(AllowEditComponentLifeCycleList));
                }
            }
        }

        private ICollectionView componentLifeCycleView = null;
        public ICollectionView ComponentLifeCycleView
        {
            get
            {
                componentLifeCycleView = null;
                componentLifeCycleView = CollectionViewSource.GetDefaultView(ComponentLifeCycleList);
                componentLifeCycleView.Filter = x =>
                {
                    ComponentLifeCycleModel componentLifeCycleModel = x as ComponentLifeCycleModel;
                    return componentLifeCycleModel.ComponentLifeCycleEnable == true;
                };
                return componentLifeCycleView;
            }
        }

        private List<ComponentLifeCycleModel> componentLifeCycleList = new List<ComponentLifeCycleModel>();
        public List<ComponentLifeCycleModel> ComponentLifeCycleList
        {
            get { return componentLifeCycleList; }
            set
            {
                if (componentLifeCycleList != value)
                {
                    componentLifeCycleList = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleList));
                }
            }
        }
        #endregion

        #region Constructor
        public ComponentLifeCycleInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            //MethodHelper.Action(GetType().Name, nameof(initializeCommand),
            //    () => EditComponentLifeCycleListCommand = new RelayCommand(editComponentLifeCycleListCommand));
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<ComponentLifeCycleUpdate>(this, updateComponentLifeCycle);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateCommandMethods
        private void editComponentLifeCycleListCommand()
        {
            //omponentLifeCycleEditorView.ShowDialog();
        }
        #endregion

        #region PrivateMessengerMethods
        private void updateComponentLifeCycle(ComponentLifeCycleUpdate componentLifeCycleUpdate)
        {
            switch (componentLifeCycleUpdate)
            {
                case ComponentLifeCycleUpdate.Enable:
                    RaisePropertyChanged(nameof(ComponentLifeCycleView));
                    break;
                case ComponentLifeCycleUpdate.Name:
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
        //        AllowEditComponentLifeCycleList = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.ComponentLifeCycleEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            ComponentLifeCycleList.ForEach(x =>
            {
                x.ComponentLifeCycleName = x.ComponentLifeCycleKey;
                //x.ComponentLifeCycleName = HmiHelper.GetResource<string>(x.ComponentLifeCycleKey);
            });
        }
        #endregion
    }
}