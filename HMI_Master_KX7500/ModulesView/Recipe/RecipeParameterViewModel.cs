using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Model.OEE;
using HMI_Master_KX7500.Model.Recipe.Parameter;
using HMI_Master_KX7500.Service.Recipe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HMI_Master_KX7500.ModulesView.Recipe
{
    public class RecipeParameterViewModel : ViewModelBase
    {
        #region PublicCommandProperties
        public ICommand RecipeParameterEditorViewClosingCommand { get; set; }

        public ICommand SaveRecipeParameterListCommand { get; set; }
        #endregion

        #region PublicProperties
        private ICollectionView recipeParameterList = null;
        public ICollectionView RecipeParameterList
        {
            get { return recipeParameterList; }
            set
            {
                if (recipeParameterList != value)
                {
                    recipeParameterList = value;
                    RaisePropertyChanged(nameof(RecipeParameterList));
                }
            }
        }
        #endregion

        #region Constructor
        public RecipeParameterViewModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            RecipeParameterEditorViewClosingCommand = new RelayCommand<EventArgsModel>(recipeParameterEditorViewClosingCommand);
            SaveRecipeParameterListCommand = new RelayCommand<Window>(saveRecipeParameterListCommand);
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<RecipeInitializeModel>(this, initializeRecipeParameterList);
        }
        #endregion

        #region PrivateCommandMethods
        private void recipeParameterEditorViewClosingCommand(EventArgsModel eventArgsModel)
        {
            CancelEventArgs cancelEventArgs = eventArgsModel.EventArgs as CancelEventArgs;
            cancelEventArgs.Cancel = true;

            Window window = eventArgsModel.Parameter as Window;
            window.Hide();
        }

        private void saveRecipeParameterListCommand(Window window)
        {
            if (MessageBox.Show($"Are You Sure You Want To Save ?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                if (RecipeService.SerializeRecipeParameterList(
                        RecipeParameterList.SourceCollection as List<RecipeParameterModel>))
                {
                    window.Hide();
                }
            }
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeRecipeParameterList(RecipeInitializeModel recipeInitializeModel)
        {
            RecipeParameterList = null;
            RecipeParameterList = CollectionViewSource.GetDefaultView(
                RecipeService.DeserializeRecipeParameterList());
        }
        #endregion
    }
}
