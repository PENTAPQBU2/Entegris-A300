using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Robot;
using HMI_Master_KX7500.ModulesView.Robot;
using HMI_Master_KX7500.Service.Robot;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.RecipeManager
{
    //Recipe page (MT_001): create, copy, rename and delete recipes. Robot points are taught on the Robot page.
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
            RenameCommand = new RelayCommand(rename, () => SelectedRecipe != null);
            DeleteCommand = new RelayCommand(delete, () => SelectedRecipe != null && Recipes.Count > 1);
            RefreshCommand = new RelayCommand(reload);

            RobotRecipeService.EnsureDefaultRecipes();
            reload();
            RobotRecipeService.RecipesChanged += () => Application.Current?.Dispatcher.BeginInvoke(new Action(reload));
        }

        public RelayCommand CreateCopyCommand { get; }
        public RelayCommand RenameCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand RefreshCommand { get; }

        public ObservableCollection<RobotRecipeModel> Recipes { get; } = new ObservableCollection<RobotRecipeModel>();
        public ObservableCollection<RobotPointItem> Points { get; } = new ObservableCollection<RobotPointItem>();

        private RobotRecipeModel selectedRecipe;
        public RobotRecipeModel SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                Set(ref selectedRecipe, value);
                Points.Clear();
                if(value != null)
                {
                    foreach(RobotPointModel p in value.Points)
                    {
                        Points.Add(new RobotPointItem
                        {
                            Variable = p.Variable,
                            Label = p.Label,
                            Kind = p.Variable.EndsWith("_E") ? RobotPointKind.Entry : RobotPointKind.Screw,
                            RecipePoint = p
                        });
                    }
                    NewName = value.RecipeName;
                }
            }
        }

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
            foreach(RobotRecipeModel recipe in RobotRecipeService.LoadAll())
            {
                Recipes.Add(recipe);
            }
            SelectedRecipe = Recipes.FirstOrDefault(x => x.RecipeID == selectedId) ?? Recipes.FirstOrDefault();
            NewId = RobotRecipeService.NextRecipeId().ToString();
        }

        private void createCopy()
        {
            int id;
            if(!int.TryParse((NewId ?? string.Empty).Trim(), out id))
            {
                show("Recipe ID must be a number.");
                return;
            }

            string problem = RobotRecipeService.Validate(NewName, id);
            if(problem != null)
            {
                show(problem);
                return;
            }

            try
            {
                RobotRecipeModel copy = RobotRecipeService.CreateCopy(SelectedRecipe, NewName, id);
                reload();
                SelectedRecipe = Recipes.FirstOrDefault(x => x.RecipeID == copy.RecipeID);
                show($"Recipe {copy.RecipeName} (ID {copy.RecipeID}) created with the robot points of the copied recipe. Teach its points on the Robot page.");
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Create failed: " + ex.Message);
            }
        }

        private void rename()
        {
            string problem = RobotRecipeService.Validate(NewName, SelectedRecipe.RecipeID, SelectedRecipe);
            if(problem != null)
            {
                show(problem);
                return;
            }

            try
            {
                string oldName = SelectedRecipe.RecipeName;
                RobotRecipeService.Rename(SelectedRecipe, NewName);
                reload();
                show($"Recipe {oldName} renamed to {NewName.Trim()}.");
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                show("Rename failed: " + ex.Message);
            }
        }

        private void delete()
        {
            RobotRecipeModel recipe = SelectedRecipe;
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
                RobotRecipeService.Delete(recipe);
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
