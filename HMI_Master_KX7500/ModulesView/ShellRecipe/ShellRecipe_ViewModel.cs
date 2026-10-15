using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HMI_Master_KX7500.Model.ShellRecipe;
using HMI_Master_KX7500.Service.ShellRecipe;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMI_Master_KX7500.ModulesView.ShellRecipe
{
    /// <summary>
    /// Recipe page (MT_001), layout follows BU4 sample (Welding HMI Recipe Editor):
    /// New / Save (rename) / Delete a recipe = label for a shell ID.
    /// </summary>
    public class ShellRecipe_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region PublicProperties
        private ObservableCollection<ShellRecipeModel> recipeList = new ObservableCollection<ShellRecipeModel>();
        public ObservableCollection<ShellRecipeModel> RecipeList
        {
            get { return recipeList; }
            set
            {
                if (recipeList != value)
                {
                    recipeList = value;
                    RaisePropertyChanged(nameof(RecipeList));
                }
            }
        }

        private ShellRecipeModel selectedRecipe = null;
        public ShellRecipeModel SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                if (selectedRecipe != value)
                {
                    selectedRecipe = value;
                    RaisePropertyChanged(nameof(SelectedRecipe));

                    if (selectedRecipe != null)
                    {
                        RecipeID = selectedRecipe.RecipeID;
                        RecipeName = selectedRecipe.RecipeName;
                        RobotPointPrefix = selectedRecipe.RobotPointPrefix;
                        IsNewRecipe = false;
                    }
                }
            }
        }

        private int recipeID = 0;
        public int RecipeID
        {
            get { return recipeID; }
            set
            {
                if (recipeID != value)
                {
                    recipeID = value;
                    RaisePropertyChanged(nameof(RecipeID));
                }
            }
        }

        private string recipeName = string.Empty;
        public string RecipeName
        {
            get { return recipeName; }
            set
            {
                if (recipeName != value)
                {
                    recipeName = value;
                    RaisePropertyChanged(nameof(RecipeName));
                }
            }
        }

        private string robotPointPrefix = string.Empty;
        public string RobotPointPrefix
        {
            get { return robotPointPrefix; }
            set
            {
                if (robotPointPrefix != value)
                {
                    robotPointPrefix = value;
                    RaisePropertyChanged(nameof(RobotPointPrefix));
                }
            }
        }

        // Recipe ID (shell ID) can only be keyed in for a new recipe, existing recipe can only be renamed
        private bool isNewRecipe = false;
        public bool IsNewRecipe
        {
            get { return isNewRecipe; }
            set
            {
                if (isNewRecipe != value)
                {
                    isNewRecipe = value;
                    RaisePropertyChanged(nameof(IsNewRecipe));
                }
            }
        }
        #endregion

        #region PublicCommandProperties
        public ICommand NewRecipeCommand { get; set; }
        public ICommand SaveRecipeCommand { get; set; }
        public ICommand DeleteRecipeCommand { get; set; }
        public ICommand CreatePointCommand { get; set; }
        #endregion

        #region Constructor
        public ShellRecipe_ViewModel()
        {
            NewRecipeCommand = new RelayCommand(newRecipeCommand);
            SaveRecipeCommand = new RelayCommand(saveRecipeCommand, () => IsNewRecipe || SelectedRecipe != null);
            DeleteRecipeCommand = new RelayCommand(deleteRecipeCommand, () => !IsNewRecipe && SelectedRecipe != null);
            CreatePointCommand = new RelayCommand(createPointCommand, () => !IsNewRecipe && SelectedRecipe != null);
        }
        #endregion

        #region PublicMethods
        public void Refresh()
        {
            try
            {
                int? keepRecipeID = IsNewRecipe ? null : SelectedRecipe?.RecipeID;
                RecipeList = new ObservableCollection<ShellRecipeModel>(ShellRecipeService.GetRecipeList());

                if (keepRecipeID.HasValue)
                {
                    selectedRecipe = null;
                    SelectedRecipe = RecipeList.FirstOrDefault(x => x.RecipeID == keepRecipeID.Value);
                }
                if (!IsNewRecipe && SelectedRecipe == null)
                {
                    clearForm();
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateCommandMethods
        private void newRecipeCommand()
        {
            try
            {
                selectedRecipe = null;
                RaisePropertyChanged(nameof(SelectedRecipe));

                // suggest the next free shell ID
                int nextID = ShellRecipeService.MinRecipeID;
                while (RecipeList.Any(x => x.RecipeID == nextID)) { nextID++; }

                RecipeID = nextID;
                RecipeName = string.Empty;
                IsNewRecipe = true;
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void saveRecipeCommand()
        {
            try
            {
                string name = (RecipeName ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Please fill in the Recipe Name.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (RecipeID < ShellRecipeService.MinRecipeID || RecipeID > ShellRecipeService.MaxRecipeID)
                {
                    MessageBox.Show($"Recipe ID must be {ShellRecipeService.MinRecipeID} - {ShellRecipeService.MaxRecipeID}.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                List<ShellRecipeModel> list = ShellRecipeService.GetRecipeList();
                int editID = IsNewRecipe ? RecipeID : SelectedRecipe.RecipeID;

                if (IsNewRecipe && list.Any(x => x.RecipeID == RecipeID))
                {
                    MessageBox.Show($"Recipe ID {RecipeID} already exists.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (list.Any(x => x.RecipeID != editID && string.Equals(x.RecipeName, name, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Recipe Name \"{name}\" already exists.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string question = IsNewRecipe
                    ? $"Add recipe {RecipeID} - {name}?"
                    : $"Rename recipe {editID} - {SelectedRecipe.RecipeName} to \"{name}\"?";
                if (MessageBox.Show(question, "Recipe", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                ShellRecipeModel recipe = list.FirstOrDefault(x => x.RecipeID == editID);
                if (recipe == null)
                {
                    recipe = new ShellRecipeModel { RecipeID = editID };
                    list.Add(recipe);
                }
                recipe.RecipeName = name;

                if (ShellRecipeService.SaveRecipeList(list))
                {
                    logger.Info($"Recipe saved: {editID} - {name}");
                    IsNewRecipe = false;
                    selectedRecipe = recipe;
                    Refresh();
                }
                else
                {
                    MessageBox.Show("Save recipe failed, see log.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void deleteRecipeCommand()
        {
            try
            {
                ShellRecipeModel recipe = SelectedRecipe;
                if (MessageBox.Show($"Delete recipe {recipe.DisplayName}?", "Recipe",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }

                List<ShellRecipeModel> list = ShellRecipeService.GetRecipeList();
                list.RemoveAll(x => x.RecipeID == recipe.RecipeID);

                if (ShellRecipeService.SaveRecipeList(list))
                {
                    logger.Info($"Recipe deleted: {recipe.DisplayName}");
                    selectedRecipe = null;
                    RaisePropertyChanged(nameof(SelectedRecipe));
                    clearForm();
                    Refresh();
                }
                else
                {
                    MessageBox.Show("Delete recipe failed, see log.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        // Robot points for the selected recipe: copy of the Black points (Point_1b -> <prefix>_Point_1 ...), in a UR installation file
        private void createPointCommand()
        {
            try
            {
                ShellRecipeModel recipe = SelectedRecipe;
                if (!RobotPointService.IsValidPrefix(RobotPointPrefix))
                {
                    MessageBox.Show($"Prefix must be 1 - {RobotPointService.MaxPrefixLength} letters or numbers and start with a letter (e.g. Blue).",
                        "Create Point", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string prefix = RobotPointService.NormalizePrefix(RobotPointPrefix);

                List<ShellRecipeModel> list = ShellRecipeService.GetRecipeList();
                ShellRecipeModel samePrefix = list.FirstOrDefault(x => x.RecipeID != recipe.RecipeID
                    && string.Equals(RobotPointService.NormalizePrefix(x.RobotPointPrefix), prefix, StringComparison.OrdinalIgnoreCase));
                if (samePrefix != null)
                {
                    MessageBox.Show($"Prefix \"{prefix}\" is already used by recipe {samePrefix.DisplayName}.", "Create Point", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                OpenFileDialog openDialog = new OpenFileDialog
                {
                    Title = "Select the robot installation file (latest copy from the robot)",
                    Filter = "Robot installation (*.installation)|*.installation",
                    FileName = "default.installation"
                };
                if (openDialog.ShowDialog() != true)
                {
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Title = "Save the new robot installation file",
                    Filter = "Robot installation (*.installation)|*.installation",
                    InitialDirectory = Path.GetDirectoryName(openDialog.FileName),
                    FileName = Path.GetFileName(openDialog.FileName)
                };
                if (saveDialog.ShowDialog() != true)
                {
                    return;
                }

                int count = RobotPointService.CreatePoints(openDialog.FileName, saveDialog.FileName, prefix, out string error);
                if (count == 0)
                {
                    MessageBox.Show($"Create Point failed: {error}", "Create Point", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ShellRecipeModel saved = list.FirstOrDefault(x => x.RecipeID == recipe.RecipeID);
                if (saved != null)
                {
                    saved.RobotPointPrefix = prefix;
                    ShellRecipeService.SaveRecipeList(list);
                    Refresh();
                }

                MessageBox.Show($"{count} points created ({prefix}_Point_1 ...) for recipe {recipe.DisplayName}.\n\n" +
                    $"File: {saveDialog.FileName}\n\n" +
                    "Load this installation file on the robot, then adjust the points on the teach pendant.",
                    "Create Point", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMethods
        private void clearForm()
        {
            RecipeID = 0;
            RecipeName = string.Empty;
            RobotPointPrefix = string.Empty;
            IsNewRecipe = false;
        }
        #endregion
    }
}
