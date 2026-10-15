using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.LotRecipe;
using HMI_Master_KX7500.Service.LotRecipe;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.RecipeManager
{
    //Recipe page (MT_001): create, copy, edit and delete recipes. Each recipe uses one robot point set
    //(waypoints taught on the robot pendant).
    public class RecipeManagerViewModel : ViewModelBase
    {
        public const string PageName = "Recipe";

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly KeyenceDll keyenceDll;

        public RecipeManagerViewModel()
        {
            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort,
                DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            CreateCopyCommand = new RelayCommand(createCopy, () => SelectedRecipe != null);
            SaveCommand = new RelayCommand(save, () => SelectedRecipe != null);
            DeleteCommand = new RelayCommand(delete, () => SelectedRecipe != null && Recipes.Count > 1);
            RefreshCommand = new RelayCommand(reload);
            BrowseInstallationCommand = new RelayCommand(browseInstallation);
            CreatePointSetCommand = new RelayCommand(createPointSet, () => SourcePointSet != null && !string.IsNullOrEmpty(InstallationFile));
            WriteScriptCommand = new RelayCommand(writeScript, () => !string.IsNullOrEmpty(InstallationFile));

            LotRecipeService.EnsureDefaultRecipes();
            reload();
            LotRecipeService.RecipesChanged += () => Application.Current?.Dispatcher.BeginInvoke(new Action(reload));
        }

        public RelayCommand CreateCopyCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand RefreshCommand { get; }
        public RelayCommand BrowseInstallationCommand { get; }
        public RelayCommand CreatePointSetCommand { get; }
        public RelayCommand WriteScriptCommand { get; }

        public ObservableCollection<LotRecipeModel> Recipes { get; } = new ObservableCollection<LotRecipeModel>();
        private LotRecipeModel selectedRecipe;
        public LotRecipeModel SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                Set(ref selectedRecipe, value);
                if(value != null)
                {
                    NewName = value.RecipeName;
                    NewPointSet = value.RobotPointSet.ToString();
                }
            }
        }

        private string newPointSet = "0";
        public string NewPointSet { get => newPointSet; set => Set(ref newPointSet, value); }

        private string newName = string.Empty;
        public string NewName { get => newName; set => Set(ref newName, value); }

        private string newId = string.Empty;
        public string NewId { get => newId; set => Set(ref newId, value); }

        private string message = string.Empty;
        public string Message { get => message; private set => Set(ref message, value); }

        #region Robot point sets
        public ObservableCollection<RobotPointSetModel> PointSets { get; } = new ObservableCollection<RobotPointSetModel>();

        private RobotPointSetModel sourcePointSet;
        public RobotPointSetModel SourcePointSet { get => sourcePointSet; set => Set(ref sourcePointSet, value); }

        private string installationFile = string.Empty;
        public string InstallationFile { get => installationFile; set => Set(ref installationFile, value); }

        private string newSetNo = string.Empty;
        public string NewSetNo { get => newSetNo; set => Set(ref newSetNo, value); }

        private string newSetName = string.Empty;
        public string NewSetName { get => newSetName; set => Set(ref newSetName, value); }

        private string newSetPrefix = string.Empty;
        public string NewSetPrefix { get => newSetPrefix; set => Set(ref newSetPrefix, value); }

        private void reloadPointSets()
        {
            int sourceNo = SourcePointSet?.SetNo ?? 0;
            PointSets.Clear();
            foreach(RobotPointSetModel set in RobotPointSetService.LoadAll())
            {
                PointSets.Add(set);
            }
            SourcePointSet = PointSets.FirstOrDefault(x => x.SetNo == sourceNo) ?? PointSets.FirstOrDefault();
            NewSetNo = PointSets.Count == 0 ? "0" : (PointSets.Max(x => x.SetNo) + 1).ToString();
        }

        //The robot's installation file, copied to a USB stick (default.installation in the robot's programs folder)
        private void browseInstallation()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select the robot installation file",
                Filter = "UR installation (*.installation)|*.installation|All files (*.*)|*.*"
            };
            if(dialog.ShowDialog() == true)
            {
                InstallationFile = dialog.FileName;
            }
        }

        private void createPointSet()
        {
            int setNo;
            if(!int.TryParse((NewSetNo ?? string.Empty).Trim(), out setNo))
            {
                show("Point set number must be a number.");
                return;
            }
            string prefix = (NewSetPrefix ?? string.Empty).Trim();
            string problem = RobotPointSetService.Validate(RobotPointSetService.LoadAll(), SourcePointSet, setNo, NewSetName, prefix);
            if(problem != null)
            {
                show(problem);
                return;
            }

            if(MessageBoxHelper.ShowMessageBox($"Add point set {setNo} \"{NewSetName.Trim()}\" ({prefix}_P01 ... {prefix}_P33, copied from {SourcePointSet.Name}) to\n{InstallationFile}\nand write {RobotPointSetService.ScriptFileName} next to it?\n\nA copy of the file is kept in the HMI RobotBackup folder first.",
                   "Create point set", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                string backup = RobotPointSetService.CreatePointSet(InstallationFile, SourcePointSet, setNo, NewSetName, prefix);
                List<string> skipped = RobotPointSetService.WriteScript(InstallationFile);
                reloadPointSets();
                NewSetName = string.Empty;
                NewSetPrefix = string.Empty;
                show($"Point set {setNo} created: {prefix}_P01 ... {prefix}_P33 added to the installation and {RobotPointSetService.ScriptFileName} written. " +
                     $"Copy both files to the robot, load the installation, then teach the points. Backup: {backup}" +
                     (skipped.Count > 0 ? $" Not in the script (points missing in this installation): {string.Join(", ", skipped)}." : string.Empty));
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Create point set failed: " + ex.Message);
            }
        }

        //Writes recipe_points.script for the installation without adding points (e.g. after a set was deleted on the robot)
        private void writeScript()
        {
            try
            {
                List<string> skipped = RobotPointSetService.WriteScript(InstallationFile);
                show($"{RobotPointSetService.ScriptFileName} written next to {InstallationFile}." +
                     (skipped.Count > 0 ? $" Not included (points missing in this installation): {string.Join(", ", skipped)}." : string.Empty));
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Write script failed: " + ex.Message);
            }
        }
        #endregion

        private void reload()
        {
            int selectedId = SelectedRecipe?.RecipeID ?? 0;
            Recipes.Clear();
            foreach(LotRecipeModel recipe in LotRecipeService.LoadAll())
            {
                Recipes.Add(recipe);
            }
            SelectedRecipe = Recipes.FirstOrDefault(x => x.RecipeID == selectedId) ?? Recipes.FirstOrDefault();
            NewId = LotRecipeService.NextRecipeId().ToString();
            reloadPointSets();
        }

        private void createCopy()
        {
            int id;
            if(!int.TryParse((NewId ?? string.Empty).Trim(), out id))
            {
                show("Recipe ID must be a number.");
                return;
            }

            int pointSet;
            string problem = parsePointSet(out pointSet) ?? LotRecipeService.Validate(NewName, id);
            if(problem != null)
            {
                show(problem);
                return;
            }

            try
            {
                LotRecipeModel copy = LotRecipeService.CreateCopy(SelectedRecipe, NewName, id, pointSet);
                reload();
                SelectedRecipe = Recipes.FirstOrDefault(x => x.RecipeID == copy.RecipeID);
                show($"Recipe {copy.RecipeName} (ID {copy.RecipeID}) created with robot point set {copy.RobotPointSet}.");
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Create failed: " + ex.Message);
            }
        }

        private string parsePointSet(out int pointSet)
        {
            if(!int.TryParse((NewPointSet ?? string.Empty).Trim(), out pointSet))
            {
                return "Robot point set must be a number.";
            }
            return LotRecipeService.ValidatePointSet(pointSet);
        }

        //Saves the name and robot point set of the selected recipe
        private void save()
        {
            int pointSet;
            string problem = parsePointSet(out pointSet) ?? LotRecipeService.Validate(NewName, SelectedRecipe.RecipeID, SelectedRecipe);
            if(problem != null)
            {
                show(problem);
                return;
            }

            try
            {
                string oldName = SelectedRecipe.RecipeName;
                SelectedRecipe.RobotPointSet = pointSet;
                LotRecipeService.Rename(SelectedRecipe, NewName);
                reload();
                show($"Recipe {oldName} saved as {NewName.Trim()}, robot point set {pointSet}." +
                     (PointSets.Any(x => x.SetNo == pointSet) ? string.Empty : $" Point set {pointSet} does not exist yet: create it below before using this recipe."));
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Save failed: " + ex.Message);
            }
        }

        private void delete()
        {
            LotRecipeModel recipe = SelectedRecipe;
            int lotRecipe = keyenceDll.Read<int>(PLCTag.Lot_Recipe_ID);
            if(lotRecipe == recipe.RecipeID)
            {
                show($"Recipe {recipe.RecipeName} is used by the current lot and cannot be deleted.");
                return;
            }

            if(MessageBoxHelper.ShowMessageBox($"Delete recipe {recipe.RecipeName} (ID {recipe.RecipeID})? The file is moved to Recipe\\Deleted.",
                   "Delete recipe", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                LotRecipeService.Delete(recipe);
                reload();
                show($"Recipe {recipe.RecipeName} deleted.");
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Delete failed: " + ex.Message);
            }
        }

        private void show(string text)
        {
            LogHelper.KeyValue(GetType().Name, "Recipe page", text);
            Message = $"{DateTime.Now:HH:mm:ss}  {text}";
        }
    }
}
