using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HMI_LiXiang_WeldingMachine.ModulesView.Settings
{
    public class RecipeEditorViewModel : ViewModelBase
    {
        #region PrivateFields
        private OPCUA_ opcUa = new OPCUA_();
        private List<string> tagList = new List<string>();
        #endregion

        #region PublicCommandProperties
        public ICommand RecipeEditorViewClosingCommand { get; set; }

        public ICommand RecipeComboBoxPreviewMouseDownCommand { get; set; }

        public ICommand RecipeComboBoxSelectionChangedCommand { get; set; }

        public ICommand SaveRecipeCommand { get; set; }

        public ICommand AddNewRecipeCommand { get; set; }

        public ICommand DeleteRecipeCommand { get; set; }

        public ICommand KeyDownCommand { get; set; }
        #endregion

        #region PublicProperties
        private ICollectionView recipeList = null;
        public ICollectionView RecipeList
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

        private int recipeComboBoxSelectedIndex = -1;
        public int RecipeComboBoxSelectedIndex
        {
            get { return recipeComboBoxSelectedIndex; }
            set
            {
                if (recipeComboBoxSelectedIndex != value)
                {
                    recipeComboBoxSelectedIndex = value;
                    RaisePropertyChanged(nameof(RecipeComboBoxSelectedIndex));
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

        private bool recipeNameEditable = true;
        public bool RecipeNameEditable
        {
            get { return recipeNameEditable; }
            set
            {
                if (recipeNameEditable != value)
                {
                    recipeNameEditable = value;
                    RaisePropertyChanged(nameof(RecipeNameEditable));
                }
            }
        }

        private bool newRecipeButtonEnabled = false;
        public bool NewRecipeButtonEnabled
        {
            get { return newRecipeButtonEnabled; }
            set
            {
                if (newRecipeButtonEnabled != value)
                {
                    newRecipeButtonEnabled = value;
                    RaisePropertyChanged(nameof(NewRecipeButtonEnabled));
                }
            }
        }
        #endregion

        #region Constructor
        public RecipeEditorViewModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            RecipeEditorViewClosingCommand = new RelayCommand<EventArgsModel>(recipeEditorViewClosingCommand);
            RecipeComboBoxPreviewMouseDownCommand = new RelayCommand(recipeComboBoxPreviewMouseDownCommand);
            RecipeComboBoxSelectionChangedCommand = new RelayCommand<object>(recipeComboBoxSelectionChangedCommand);
            SaveRecipeCommand = new RelayCommand<string>(SaveRecipe);
            AddNewRecipeCommand = new RelayCommand<string>(AddRecipe);
            //AddNewRecipeCommand = new RelayCommand<string>(addNewRecipeCommand);
            DeleteRecipeCommand = new RelayCommand<object>(deleteRecipeCommand);
            KeyDownCommand = new RelayCommand(keyDownCommand);
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<RecipeInitializeModel>(this, initializeRecipe);
        }
        #endregion

        #region PrivateCommandMethods
        private void recipeEditorViewClosingCommand(EventArgsModel eventArgsModel)
        {
            MethodHelper.Execute(GetType().Name, nameof(recipeEditorViewClosingCommand), () =>
            {
                CancelEventArgs cancelEventArgs = eventArgsModel.EventArgs as CancelEventArgs;
                cancelEventArgs.Cancel = true;

                Window window = eventArgsModel.Parameter as Window;
                window.Hide();
            });
        }

        private void recipeComboBoxPreviewMouseDownCommand()
        {
            MethodHelper.Execute(GetType().Name, nameof(recipeComboBoxPreviewMouseDownCommand), () =>
            {
                RecipeList = null;
                RecipeList = CollectionViewSource.GetDefaultView(RecipeService.GetRecipeList());
            });
        }

        private void recipeComboBoxSelectionChangedCommand(object recipeName)
        {
            MethodHelper.Execute(GetType().Name, nameof(recipeComboBoxSelectionChangedCommand), () =>
            {
                if (recipeName != null)
                {
                    LogHelper.Event(GetType().Name, MessageHelper.UserSelectedRecipe(recipeName.ToString()));

                    NewRecipeButtonEnabled = false;
                    RecipeNameEditable = false;
                    updateRecipe(recipeName.ToString());
                }
                else
                {
                    updateRecipe();
                    NewRecipeButtonEnabled = false;
                    RecipeNameEditable = true;
                }
            });
        }

        private void AddRecipeCommand(RecipeEditorModel recipeEditorModel)
        {
            MethodHelper.Execute(GetType().Name, nameof(AddRecipeCommand),
                MessageHelper.AreYouSureYouWantTo("save recipe"), () =>
                {
                    List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipeParameterList();
                    siemensOPC.ReadOPC();

                    siemensOPC.WriteSiemensOPCTag(RecipeTag.SaveRecipeHMI, true);

                    bool isReady = false;
                    do
                    {
                        Thread.Sleep(1000);
                        isReady = siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.SaveRecipeDataReady);

                    } while (!isReady);

                    newRecipe.ForEach(x =>
                    {
                        x.RecipeParameter = siemensOPC.ReadSiemensOPCTag<string>(x.RecipeParameterTagModel.RecipeParameterTag);
                    });

                    siemensOPC.WriteSiemensOPCTag(RecipeTag.SaveRecipeDataReady, false);
                    RecipeService.SerializeRecipe(newRecipe, newRecipe.Where(x => x.RecipeParameterName == "Recipe_Name").First().RecipeParameter);

                }, MessageHelper.UserClickedButton("SaveRecipe"));
        }

        private void SaveRecipe(string recipeName)
        {
            siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeProcessNumber, 2);
            AddRecipeCommand(recipeName);
            MessageBox.Show("Load Recipe Done: " + recipeName);
        }

        private void AddRecipe(string recipeName)
        {
            siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeProcessNumber, 1);
            AddRecipeCommand(recipeName);
            MessageBox.Show("Load Recipe Done: " + recipeName);
        }


        private void AddRecipeCommand(string recipeName)
        {
            MethodHelper.Execute(GetType().Name, nameof(AddRecipeCommand),
                MessageHelper.AreYouSureYouWantTo("save recipe"), () =>
                {
                    List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipeParameterList();
                    siemensOPC.ReadOPC();

                    siemensOPC.WriteSiemensOPCTag(RecipeTag.SaveRecipeHMI, true);

                    bool isReady = false;
                    do
                    {
                        Thread.Sleep(1000);
                        isReady = siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.SaveRecipeDataReady);

                    } while (!isReady);

                    newRecipe.ForEach(x =>
                    {
                        x.RecipeParameter = siemensOPC.ReadSiemensOPCTag<string>(x.RecipeParameterTagModel.RecipeParameterTag);
                    });

                    newRecipe.Where(x => x.RecipeParameterName == "Recipe_Name").First().RecipeParameter = recipeName;
                    siemensOPC.WriteSiemensOPCTag(RecipeTag.CurrentRecipeName, recipeName);

                    siemensOPC.WriteSiemensOPCTag(RecipeTag.SaveRecipeDataReady, false);
                    RecipeService.SerializeRecipe(newRecipe, newRecipe.Where(x => x.RecipeParameterName == "Recipe_Name").First().RecipeParameter);

                }, MessageHelper.UserClickedButton("SaveRecipe"));
        }

        private void addNewRecipeCommand(string recipeName)
        {
            MethodHelper.Execute(GetType().Name, nameof(addNewRecipeCommand),
                MessageHelper.AreYouSureYouWantTo("add new recipe"), () =>
                {
                    List<string> CurrentRecipeList = RecipeService.GetRecipeList();

                    if (recipeName.Trim().Length == 0)
                    {
                        LogHelper.Error(nameof(addNewRecipeCommand), $"Recipe name is empty.");
                        MessageBoxHelper.ShowMessageBox($"Recipe name is empty.");
                    }
                    else if (CurrentRecipeList.Exists(x => x == recipeName))
                    {
                        LogHelper.Error(nameof(addNewRecipeCommand), $"Recipe {recipeName} already exists.");
                        MessageBoxHelper.ShowMessageBox($"Recipe {recipeName} already exists.");
                    }
                    else
                    {
                        List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipeParameterList();
                        newRecipe.ForEach(x =>
                        {
                            x.RecipeParameter = siemensOPC.ReadSiemensOPCTag<string>(x.RecipeParameterTagModel.RecipeParameterTag);
                        });

                        siemensOPC.WriteSiemensOPCTag(RecipeTag.RequestRecipeName, recipeName);
                        siemensOPC.WriteSiemensOPCTag(RecipeTag.CreateNewRecipeHMI, true);

                        do
                        {
                            Thread.Sleep(1000);
                            siemensOPC.ReadOPC();
                            if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.CreateRecipeSuccessfulPLC))
                            {
                                siemensOPC.WriteSiemensOPCTag(RecipeTag.CreateRecipeSuccessfulPLC, false);
                                Messenger.Default.Send((PLCConfigUpdate.Save, recipeName));

                                RecipeService.SerializeRecipe(newRecipe, recipeName);
                                MessageBoxHelper.ShowMessageBox("Recipe created successfully.");
                                LogHelper.CreateDone(nameof(RecipeEditorViewModel), nameof(newRecipe));
                                break;
                            }
                            else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNameNullPLC))
                            {
                                siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNameNullPLC, false);

                                LogHelper.Error(nameof(addNewRecipeCommand), "PLC receive null recipe name.");
                                MessageBoxHelper.ShowMessageBox("PLC receive null recipe name.");
                                break;
                            }
                            else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeFullPLC))
                            {
                                siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeFullPLC, false);

                                LogHelper.Error(nameof(addNewRecipeCommand), "PLC recipe full.");
                                MessageBoxHelper.ShowMessageBox("PLC recipe full. Please delete some recipe.");
                                break;
                            }
                        } while (true);
                    }
                }, MessageHelper.UserClickedButton("NewRecipe"));
        }

        private void deleteRecipeCommand(object recipeName)
        {
            MethodHelper.Execute(GetType().Name, nameof(deleteRecipeCommand),
                MessageHelper.AreYouSureYouWantTo("delete recipe"), () =>
                {
                    File.Delete($@"D:\HMI_ROHM\Recipe\{recipeName}.xml");

                    //siemensOPC.WriteSiemensOPCTag(RecipeTag.RequestRecipeName, recipeName.ToString());
                    //siemensOPC.WriteSiemensOPCTag(RecipeTag.DeleteRecipeHMI, true);

                    //do
                    //{
                    //    Thread.Sleep(1000);
                    //    siemensOPC.ReadOPC();
                    //    if (!siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.DeleteRecipePLC))
                    //    {
                    //        RecipeService.DeleteRecipe(recipeName.ToString());
                    //        Messenger.Default.Send((PLCConfigUpdate.Delete, recipeName.ToString()));

                    //        LogHelper.Done(nameof(RecipeEditorViewModel), $"Delete recipe {recipeName.ToString()}");
                    //        MessageBoxHelper.ShowMessageBox("Recipe delete successfully.");
                    //        initializeRecipe();
                    //        break;
                    //    }
                    //    else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNameNullPLC))
                    //    {
                    //        siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNameNullPLC, false);
                    //        LogHelper.Error(nameof(deleteRecipeCommand), "PLC receive null recipe name.");
                    //        MessageBoxHelper.ShowMessageBox("PLC receive null recipe name.");
                    //        break;
                    //    }
                    //    else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNotExistsPLC))
                    //    {
                    //        siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNotExistsPLC, false);
                    //        LogHelper.Error(nameof(deleteRecipeCommand), "Recipe not exists in PLC.");
                    //        MessageBoxHelper.ShowMessageBox("Recipe not exists in PLC.");
                    //        break;
                    //    }
                    //} while (true);
                }, MessageHelper.UserClickedButton("DeleteRecipe"));
        }

        private void keyDownCommand()
        {
            if (RecipeNameEditable)
                NewRecipeButtonEnabled = true;
            else
                NewRecipeButtonEnabled = false;
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeRecipe(RecipeInitializeModel recipeInitializeModel = null)
        {
            tagList = RecipeService.GetRecipeParameterTagList();
            tagList = tagList.Union(RecipeService.GetRecipeTagList()).ToList();
            siemensOPC = new SiemensOPC(tagList);

            updateRecipe();
            RecipeComboBoxSelectedIndex = -1;
        }
        #endregion

        #region PrivateMethods
        private void updateRecipe(string recipeName = "")
        {
            RecipeName = recipeName;
        }
        #endregion
    }
}
