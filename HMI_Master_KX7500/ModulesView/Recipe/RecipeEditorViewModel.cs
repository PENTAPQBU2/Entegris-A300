using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Model.OEE;
using HMI_Master_KX7500.Model.Recipe;
using HMI_Master_KX7500.Model.Recipe.Parameter;
using HMI_Master_KX7500.Service.Recipe;
using HMI_Master_KX7500.Tags;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace HMI_Master_KX7500.ModulesView.Recipe
{
    public class RecipeEditorViewModel : ViewModelBase
    {
        #region PrivateFields
        private OPCUA_ opcUa = new OPCUA_();
        private List<string> tagList = new List<string>();
        private Window window = null;
        private List<string> recipeEditorTagList = new List<string>();
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
        public RecipeEditorViewModel(Window _window)
        {
            initializeCommand();
            initializeMessenger();
            window = _window;
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
            Messenger.Default.Register<OPCUA_>(this, initializeOpc);
        }

        private void initializeOpc(OPCUA_ opcUa_)
        {
            try
            {
                if (opcUa_.OPC.Module.Contains(OPCModules.RecipeEditor))
                {
                    opcUa = opcUa_;
                    recipeEditorTagList = opcUa.GetAddresses();
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            //recipeTagList = RecipeService.GetRecipeTagList();
            //// To get parameter tag from PLCConfig
            //recipeTagList = recipeTagList.Union(PLCConfigService.GetPLCConfigTag(PLCConfigService.DeserializePLCConfigParameter())).ToList();

            //List<RecipeParameterModel> RecipePara = RecipeService.DeserializeRecipeParameterList();
            //recipeTagList = recipeTagList.Union(RecipePara.Select(x => x.RecipeParameterTagModel.RecipeParameterTag)).ToList();

            //siemensOPC = new SiemensOPC(recipeTagList);
        }
        #endregion

        #region PrivateCommandMethods
        private void recipeEditorViewClosingCommand(EventArgsModel eventArgsModel)
        {
            CancelEventArgs cancelEventArgs = eventArgsModel.EventArgs as CancelEventArgs;
            cancelEventArgs.Cancel = true;

            window = eventArgsModel.Parameter as Window;
            window.Hide();
        }

        private void recipeComboBoxPreviewMouseDownCommand()
        {
            RecipeList = null;
            RecipeList = CollectionViewSource.GetDefaultView(RecipeService.GetRecipeList());
        }

        private void recipeComboBoxSelectionChangedCommand(object recipeName)
        {
            if (recipeName != null)
            {
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
        }

        #region NotUse
        //private void saveRecipeCommand(RecipeEditorModel recipeEditorModel)
        //{
        //    if (MessageBox.Show($"Are You Sure You Want To Save Recipe?", "Confirmation", MessageBoxButton.YesNo,
        //        MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
        //    {
        //        List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipeParameterList();
        //        opcUa.ReadAllAddresses();
        //        newRecipe.ForEach(x =>
        //        {
        //            x.RecipeParameter = opcUa.Read<string>(x.RecipeParameterTagModel.RecipeParameterTag);
        //        });

        //        //siemensOPC.WriteSiemensOPCTag(RecipeTag.RequestRecipeName, recipeEditorModel.OldRecipeName);
        //        opcUa.Write(RecipeTag.SaveRecipeHMI, true);

        //        do
        //        {
        //            Thread.Sleep(1000);
        //            opcUa.ReadAllAddresses();
        //            //if (!siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.SaveRecipePLC))
        //            //{
        //            //    siemensOPC.WriteSiemensOPCTag(RecipeTag.SaveRecipePLC, false);
        //            //    Messenger.Default.Send((PLCConfigUpdate.Save, recipeEditorModel.OldRecipeName));
        //            //    RecipeService.SerializeRecipe(newRecipe, recipeEditorModel.OldRecipeName);

        //            //    MessageBoxHelper.ShowMessageBox("Recipe save successfully.");
        //            //    LogHelper.CreateDone(nameof(RecipeEditorViewModel), nameof(newRecipe));
        //            //    break;
        //            //}
        //            //else 
        //            if (opcUa.Read<bool>(RecipeTag.RecipeNotExist))
        //            {
        //                opcUa.Write(RecipeTag.RecipeNotExist, false);
        //                //LogHelper.Error(nameof(saveRecipeCommand), "Recipe not exists in PLC.");
        //                MessageBox.Show("Recipe not exists in PLC.");
        //                break;
        //            }
        //            //else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNameNullPLC))
        //            //{
        //            //    siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNameNullPLC, false);
        //            //    LogHelper.Error(nameof(saveRecipeCommand), "PLC receive null recipe name.");
        //            //    MessageBoxHelper.ShowMessageBox("PLC receive null recipe name.");
        //            //    break;
        //            //}
        //            //else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeFullPLC))
        //            //{
        //            //    siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeFullPLC, false);
        //            //    LogHelper.Error(nameof(saveRecipeCommand), "PLC recipe full.");
        //            //    MessageBoxHelper.ShowMessageBox("PLC recipe full. Please delete some recipe.");
        //            //    break;
        //            //}
        //        } while (true);
        //    }
        //}

        /*private void addNewRecipeCommand(string recipeName)
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
        }*/

        /*private void AddRecipeCommand(RecipeEditorModel recipeEditorModel)
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
        }*/
        #endregion

        private void SaveRecipe(string recipeName)
        {
            //siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeProcessNumber, 2);
            AddRecipeCommand(recipeName);
            //MessageBox.Show("Load Recipe Done: " + recipeName);
            //MessageBox.Show("Save recipe successfully.");
        }

        private void AddRecipe(string recipeName)
        {
            //siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeProcessNumber, 1);
            AddRecipeCommand(recipeName);
            //MessageBox.Show("Load Recipe Done: " + recipeName);
            //MessageBox.Show("Save recipe successfully.");
        }

        private void AddRecipeCommand(string recipe)
        {
            if (MessageBox.Show($"Are You Sure You Want To Save Recipe?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                try
                {
                    List<string> CurrentRecipeList = RecipeService.GetRecipeList();

                    if (recipe.Trim().Length == 0)
                    {
                        MessageBox.Show("Recipe name is empty.");
                    }
                    else if (CurrentRecipeList.Exists(x => x == recipe))
                    {
                        MessageBox.Show($"Recipe {recipe} already exists.");
                    }
                    else
                    {
                        List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipeParameterList();
                        //opcUa.ReadAllAddresses();

                        opcUa.Write(RecipeTag.SaveRecipeHMI, true);

                        bool isReady = false;
                        bool exceed30Loops = false;
                        int loopingTimes = 0;

                        Task.Run(() =>
                        {
                            do
                            {
                                if (loopingTimes == 30)
                                {
                                    exceed30Loops = true;
                                }

                                if (!exceed30Loops)
                                {
                                    Thread.Sleep(1000);
                                    isReady = opcUa.Read<bool>(RecipeTag.SaveRecipeDataReady);
                                }
                                else
                                {
                                    if (Application.Current.Dispatcher.CheckAccess())
                                    {
                                        MessageBox.Show(window, $"Looping already exceed 30 times. {RecipeTag.SaveRecipeDataReady} address not turn true.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                                            MessageBox.Show(window, $"Looping already exceed 30 times. {RecipeTag.SaveRecipeDataReady} address not turn true.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }));
                                    }
                                    //MessageBox.Show($"Looping already exceed 30 times. {RecipeTag.SaveRecipeDataReady} address not turn true.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                                }
                                loopingTimes++;

                            } while (!isReady);

                            if (!exceed30Loops)
                            {
                                newRecipe.ForEach(x =>
                                {
                                    x.RecipeParameter = opcUa.Read<string>(x.RecipeParameterTagModel.RecipeParameterTag);
                                });

                                //newRecipe.Where(x => x.RecipeParameterName == "Recipe_Name").First().RecipeParameter = recipeName;
                                newRecipe.Find(x => x.RecipeParameterKey == "Recipe_Name").RecipeParameter = recipeName;

                                opcUa.Write(RecipeTag.RecipeName, recipeName);

                                opcUa.Write(RecipeTag.SaveRecipeDataReady, false);
                                RecipeService.SerializeRecipe(newRecipe, newRecipe.Find(x => x.RecipeParameterKey == "Recipe_Name").RecipeParameter);

                                if (Application.Current.Dispatcher.CheckAccess())
                                {
                                    MessageBox.Show(window, $"Save recipe successfully.");
                                }
                                else
                                {
                                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                                        MessageBox.Show(window, $"Save recipe successfully.");
                                    }));
                                }
                                
                            }
                            else
                            {
                                if (Application.Current.Dispatcher.CheckAccess())
                                {
                                    MessageBox.Show(window, $"Save recipe not successful.");
                                }
                                else
                                {
                                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                                        MessageBox.Show(window, $"Save recipe not successful.");
                                    }));
                                }
                                //MessageBox.Show($"Save recipe not successful.");
                            }
                        });
                    }
                }
                catch (Exception ex) { logger.Error($"{nameof(AddRecipeCommand)} | Error: {ex}"); }
            }
        }

        #region NotUse
        private void addNewRecipeCommand(string recipeName)
        {
            if (MessageBox.Show($"Are You Sure You Want To Add New Recipe?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                List<string> CurrentRecipeList = RecipeService.GetRecipeList();

                if (recipeName.Trim().Length == 0)
                {
                    MessageBox.Show("Recipe name is empty.");
                }
                else if (CurrentRecipeList.Exists(x => x == recipeName))
                {
                    MessageBox.Show($"Recipe {recipeName} already exists.");
                }
                else
                {
                    List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipeParameterList();
                    newRecipe.ForEach(x =>
                    {
                        x.RecipeParameter = opcUa.Read<string>(x.RecipeParameterTagModel.RecipeParameterTag);
                    });

                    //siemensOPC.WriteSiemensOPCTag(RecipeTag.RequestRecipeName, recipeName);
                    //siemensOPC.WriteSiemensOPCTag(RecipeTag.CreateNewRecipeHMI, true);

                    do
                    {
                        Thread.Sleep(1000);
                        opcUa.ReadAllAddresses();
                        //if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.CreateRecipeSuccessfulPLC))
                        //{
                        //    siemensOPC.WriteSiemensOPCTag(RecipeTag.CreateRecipeSuccessfulPLC, false);
                        //    Messenger.Default.Send((PLCConfigUpdate.Save, recipeName));

                        //    RecipeService.SerializeRecipe(newRecipe, recipeName);
                        //    MessageBoxHelper.ShowMessageBox("Recipe created successfully.");
                        //    LogHelper.CreateDone(nameof(RecipeEditorViewModel), nameof(newRecipe));
                        //    break;
                        //}
                        //else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNameNullPLC))
                        //{
                        //    siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNameNullPLC, false);

                        //    LogHelper.Error(nameof(addNewRecipeCommand), "PLC receive null recipe name.");
                        //    MessageBoxHelper.ShowMessageBox("PLC receive null receive name.");
                        //    break;
                        //}
                        //else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeFullPLC))
                        //{
                        //    siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeFullPLC, false);

                        //    LogHelper.Error(nameof(addNewRecipeCommand), "PLC recipe full.");
                        //    MessageBoxHelper.ShowMessageBox("PLC recipe full. Please delete some recipe.");
                        //    break;
                        //}

                    } while (true);
                }
            }
        }
        #endregion

        private void deleteRecipeCommand(object recipeName)
        {
            if (MessageBox.Show($"Are You Sure You Want To Delete Recipe?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                RecipeService.DeleteRecipe(recipeName.ToString());
                Messenger.Default.Send((PLCConfigUpdate.Delete, recipeName.ToString()));
                initializeRecipe();

                opcUa.Write(RecipeTag.DeleteRecipeHMI, true);
                MessageBox.Show("Recipe delete successfully.");
                //do
                //{
                //    Thread.Sleep(1000);
                //    opcUa.ReadAllAddresses();
                    //if (!siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.DeleteRecipePLC))
                    //{
                    //    RecipeService.DeleteRecipe(recipeName.ToString());
                    //    Messenger.Default.Send((PLCConfigUpdate.Delete, recipeName.ToString()));

                    //    LogHelper.Done(nameof(RecipeEditorViewModel), $"Delete recipe {recipeName.ToString()}");
                    //    MessageBoxHelper.ShowMessageBox("Recipe delete successfully.");
                    //    initializeRecipe();
                    //    break;
                    //}
                    //else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNameNullPLC))
                    //{
                    //    siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNameNullPLC, false);
                    //    LogHelper.Error(nameof(deleteRecipeCommand), "PLC receive null recipe name.");
                    //    MessageBoxHelper.ShowMessageBox("PLC receive null recipe name.");
                    //    break;
                    //}
                    //else if (siemensOPC.ReadSiemensOPCTag<bool>(RecipeTag.RecipeNotExistsPLC))
                    //{
                    //    siemensOPC.WriteSiemensOPCTag(RecipeTag.RecipeNotExistsPLC, false);
                    //    LogHelper.Error(nameof(deleteRecipeCommand), "Recipe not exists in PLC.");
                    //    MessageBoxHelper.ShowMessageBox("Recipe not exists in PLC.");
                    //    break;
                    //}
                //} while (true);
            }
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
            //tagList = RecipeService.GetRecipeParameterTagList();
            //tagList = tagList.Union(RecipeService.GetRecipeTagList()).ToList();
            //opcUa = new OPCUA_(tagList);

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
