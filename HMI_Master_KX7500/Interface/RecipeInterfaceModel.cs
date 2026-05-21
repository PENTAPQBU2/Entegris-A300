using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Model;
using HMI_Master_KX7500.Modules.Recipe;
using HMI_Master_KX7500.Service.Recipe;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HMI_Master_KX7500.Interface
{
    public class RecipeInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly RecipeService recipeService = new RecipeService();
        //private readonly RecipeParameterEditorView recipeParameterEditorView = new RecipeParameterEditorView();
        private readonly RecipeEditorView recipeEditorView = new RecipeEditorView();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly PLCConfigEditorView pLCConfigEditorView = new PLCConfigEditorView();
        #endregion

        #region PublicCommandProperties
        //public ICommand EditRecipeParameterCommand { get; set; }
        public ICommand EditRecipeCommand { get; set; }
        //public ICommand EditPLCConfigCommand { get; set; }
        #endregion

        #region PublicProperties
        //private bool allowEditRecipeParameter = false;
        //public bool AllowEditRecipeParameter
        //{
        //    get { return allowEditRecipeParameter; }
        //    set
        //    {
        //        if (allowEditRecipeParameter != value)
        //        {
        //            allowEditRecipeParameter = value;
        //            RaisePropertyChanged(nameof(AllowEditRecipeParameter));
        //        }
        //    }
        //}

        private bool allowEditRecipe = true;
        public bool AllowEditRecipe
        {
            get { return allowEditRecipe; }
            set
            {
                if (allowEditRecipe != value)
                {
                    allowEditRecipe = value;
                    RaisePropertyChanged(nameof(AllowEditRecipe));
                }
            }
        }

        //private bool allowEditPLCConfig = false;
        //public bool AllowEditPLCConfig
        //{
        //    get { return allowEditPLCConfig; }
        //    set
        //    {
        //        if (allowEditPLCConfig != value)
        //        {
        //            allowEditPLCConfig = value;
        //            RaisePropertyChanged(nameof(AllowEditPLCConfig));
        //        }
        //    }
        //}

        #endregion

        #region Constructor
        public RecipeInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            //EditRecipeParameterCommand = new RelayCommand(editRecipeParameterCommand);
            EditRecipeCommand = new RelayCommand(editRecipeCommand);
            //EditPLCConfigCommand = new RelayCommand(editPLCConfigCommand);
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
        }
        #endregion

        #region PrivateCommandMethods
        //private void editRecipeParameterCommand()
        //{
        //    MethodHelper.Execute(GetType().Name, nameof(editRecipeParameterCommand),
        //        () => recipeParameterEditorView.ShowDialog(), MessageHelper.UserClickedButton("EditRecipeParameter"));
        //}

        private void editRecipeCommand()
        {
            try
            {
                recipeEditorView.ShowDialog();
            } catch (Exception ex) { logger.Error(ex); }
            
        }

        private void editPLCConfigCommand()
        {
            //pLCConfigEditorView.ShowDialog();
        }
        #endregion

        #region PrivateMessengerMethods
        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    //AllowEditRecipeParameter = UserAccountService.IsUserAccountGroupAccessAllow(
        //    //    nameof(UserAccountGroupAccess.RecipeParameterEditor));

        //    AllowEditRecipe = UserAccountService.IsUserAccountGroupAccessAllow(
        //        nameof(UserAccountGroupAccess.RecipeEditor));

        //    AllowEditPLCConfig = UserAccountService.IsUserAccountGroupAccessAllow(
        //        nameof(UserAccountGroupAccess.PLCConfigEditor));
        //}
        #endregion
    }
}
