using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.ShellRecipe;
using HMI_Master_KX7500.Service.ShellRecipe;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    RaisePropertyChanged(nameof(RobotPointInfo));
                }
            }
        }

        // which robot points the saved recipe uses (shown in the Robot Points box)
        public string RobotPointInfo
        {
            get { return IsNewRecipe ? "Save the recipe first." : RobotPointService.GetPointInfo(SelectedRecipe); }
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
                    RaisePropertyChanged(nameof(RobotPointInfo));
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
            CreatePointCommand = new RelayCommand(createPointCommand, () => !IsNewRecipe);
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
                RobotPointPrefix = string.Empty;
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

                // robot point prefix: empty = no own points (recipe 0 / 1 use the built-in Black / Transparent points)
                string prefix = RobotPointService.NormalizePrefix(RobotPointPrefix);
                if (prefix.Length > 0 && !RobotPointService.IsValidPrefix(prefix))
                {
                    MessageBox.Show($"Robot Point Prefix must be 1 - {RobotPointService.MaxPrefixLength} letters or numbers and start with a letter (e.g. Blue).",
                        "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (RobotPointService.IsReservedPrefix(editID, prefix))
                {
                    MessageBox.Show($"Robot Point Prefix \"{prefix}\" is used by the built-in Black / Transparent points (recipe 0 / 1).",
                        "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ShellRecipeModel samePrefix = list.FirstOrDefault(x => x.RecipeID != editID && prefix.Length > 0
                    && string.Equals(RobotPointService.NormalizePrefix(x.RobotPointPrefix), prefix, StringComparison.OrdinalIgnoreCase));
                if (samePrefix != null)
                {
                    MessageBox.Show($"Robot Point Prefix \"{prefix}\" is already used by recipe {samePrefix.DisplayName}.", "Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string question = IsNewRecipe
                    ? $"Add recipe {RecipeID} - {name}?"
                    : $"Save recipe {editID} - {SelectedRecipe.RecipeName} as \"{name}\"?";
                string oldPrefix = IsNewRecipe ? string.Empty : RobotPointService.NormalizePrefix(SelectedRecipe.RobotPointPrefix);
                if (oldPrefix.Length > 0 && !string.Equals(oldPrefix, prefix, StringComparison.Ordinal))
                {
                    question += $"\n\nRobot Point Prefix changes from \"{oldPrefix}\" to \"{prefix}\". The robot points {oldPrefix}_P.. stay in the robot, press Create Points to make the new ones.";
                }
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
                recipe.RobotPointPrefix = prefix;

                if (ShellRecipeService.SaveRecipeList(list))
                {
                    logger.Info($"Recipe saved: {editID} - {name}, robot point prefix \"{prefix}\"");
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

        // Robot points for all recipes with a prefix (Black_P01.. -> <prefix>_P01..P21, <prefix>_PxxEntry) in a new installation file
        // + script with the point sets of all recipes. The source is the installation in use on the robot (SFTP, RobotProgramExport.ToRobot);
        // the 2 files go into a new folder "Robot Program (..)" on the robot, the operator copies them over the files in use on the pendant.
        // Robot not reachable -> pendrive way: local source file, same folder and names in a chosen folder.
        private bool creatingPoints;

        private async void createPointCommand()
        {
            if (creatingPoints)
            {
                return;
            }
            try
            {
                ShellRecipeModel recipe = SelectedRecipe;
                if (IsNewRecipe || (recipe != null && !string.Equals(RobotPointService.NormalizePrefix(recipe.RobotPointPrefix),
                    RobotPointService.NormalizePrefix(RobotPointPrefix), StringComparison.Ordinal)))
                {
                    MessageBox.Show("The recipe is not saved, please Save the recipe first.", "Create Points", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                creatingPoints = true;
                RobotSettingModel robot = RobotProgramExport.LoadSettings(FilePath.Robot_Settings_File);
                string source = robot.RemotePath(robot.InstallationFileName);
                if (MessageBox.Show($"Create the robot points from {source} on the robot {robot.Host}?\n\n" +
                    $"The new files go into a new folder \"{RobotProgramExport.FolderName(DateTime.Now)}\" on the robot, nothing on the robot is overwritten.",
                    "Create Points", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                List<ShellRecipeModel> recipes = ShellRecipeService.GetRecipeList();
                DateTime now = DateTime.Now;
                RobotExportResult result = await Task.Run(() =>
                {
                    using (IRobotFiles files = new SftpRobotFiles(robot))
                    {
                        return RobotProgramExport.ToRobot(files, robot, recipes, now);
                    }
                });

                if (result.Status == RobotExportStatus.NotReachable)
                {
                    logger.Warn($"Create Points: robot not reachable: {result.Message}");
                    if (MessageBox.Show($"The robot cannot be reached:\n{result.Message}\n\nUse a pendrive instead?",
                        "Create Points", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    RobotExportResult local = await createPointsLocal(robot, recipes);
                    if (local == null)
                    {
                        return;
                    }
                    result = local;
                }

                if (result.Status != RobotExportStatus.Created)
                {
                    logger.Warn($"Create Points failed: {result.Message}");
                    MessageBox.Show($"Create Points failed: {result.Message}", "Create Points", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                logger.Info($"Robot points: {result.Folder}: {result.Message}");
                MessageBox.Show($"{result.Message}\n\nNew folder:\n{result.Folder}\n  {robot.InstallationFileName}\n  {result.ScriptName}\n\n" +
                    $"On the pendant copy both files over the ones in use ({source} and the script), load them, then adjust the points.",
                    "Create Points", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { logger.Error(ex); }
            finally { creatingPoints = false; }
        }

        // pendrive way: installation copied from the robot -> same folder and names in a chosen folder; null = cancelled
        private async Task<RobotExportResult> createPointsLocal(RobotSettingModel robot, List<ShellRecipeModel> recipes)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = $"Select the robot installation file (copy of {robot.InstallationFileName} from the robot)",
                Filter = "Robot installation (*.installation)|*.installation",
                FileName = robot.InstallationFileName
            };
            if (openDialog.ShowDialog() != true)
            {
                return null;
            }

            string target;
            using (CommonOpenFileDialog folderDialog = new CommonOpenFileDialog
            {
                Title = "Select where to make the \"Robot Program (..)\" folder (e.g. the pendrive)",
                IsFolderPicker = true,
                InitialDirectory = Path.GetDirectoryName(openDialog.FileName)
            })
            {
                if (folderDialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return null;
                }
                target = folderDialog.FileName;
            }

            string source = openDialog.FileName;
            DateTime now = DateTime.Now;
            return await Task.Run(() => RobotProgramExport.ToLocal(source, target, robot, recipes, now));
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
