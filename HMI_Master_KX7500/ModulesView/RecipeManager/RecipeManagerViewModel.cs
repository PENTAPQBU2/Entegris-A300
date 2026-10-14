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

            LotRecipeService.EnsureDefaultRecipes();
            reload();
            LotRecipeService.RecipesChanged += () => Application.Current?.Dispatcher.BeginInvoke(new Action(reload));
        }

        public RelayCommand CreateCopyCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand RefreshCommand { get; }

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
                show($"Recipe {oldName} saved as {NewName.Trim()}, robot point set {pointSet}.");
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
