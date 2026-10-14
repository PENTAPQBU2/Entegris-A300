using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Interface;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Recipe.Parameter;
using HMI_Master_KX7500.Service.Recipe;
using HMI_Master_KX7500.Tags;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace HMI_Master_KX7500.ModulesView.Recipe
{
    public class Recipe_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private List<string> recipeTagList = new List<string>();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private OPCUA_ opcUa = new OPCUA_();
        private static OPCModules moduleName = OPCModules.Recipe;
        private Timer timeoutTimer = new Timer(500);
        private int timerElapsed = 0;

        private KeyenceDll keyenceDll = null;
        #endregion

        #region Command Properties
        public ICommand LoadRecipeCommand { get; set; }
        public ICommand RecipeComboBoxSelectionChangedCommand { get; set; }
        public ICommand RecipeComboBoxPreviewMouseDownCommand { get; set; }
        #endregion

        #region PublicProperties
        private RecipeInterfaceModel recipeInterfaceModel = new RecipeInterfaceModel();
        public RecipeInterfaceModel RecipeInterfaceModel
        {
            get { return recipeInterfaceModel; }
            set
            {
                if (recipeInterfaceModel != value)
                {
                    recipeInterfaceModel = value;
                    RaisePropertyChanged(nameof(RecipeInterfaceModel));
                }
            }
        }

        private bool loadRecipeAllowed = false;
        public bool LoadRecipeAllowed
        {
            get { return loadRecipeAllowed; }
            set
            {
                if (loadRecipeAllowed != value)
                {
                    loadRecipeAllowed = value;
                    RaisePropertyChanged(nameof(LoadRecipeAllowed));
                }
            }
        }

        private bool loadingRecipe = false;
        public bool LoadingRecipe
        {
            get { return loadingRecipe; }
            set
            {
                if (loadingRecipe != value)
                {
                    LoadRecipeAllowed = !value;
                    loadingRecipe = value;

                    if (LoadingRecipe)
                        timeoutTimer.Start();
                    else
                    {
                        timeoutTimer.Stop();
                        timerElapsed = 0;
                    }

                    RaisePropertyChanged(nameof(LoadingRecipe));
                }
            }
        }

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

        private string selectedRecipe = string.Empty;
        public string SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                if (selectedRecipe != value)
                {
                    selectedRecipe = value;
                    RaisePropertyChanged(nameof(SelectedRecipe));
                }
            }
        }
        #endregion

        #region Constructor
        public Recipe_OPCUA_ViewModel()
        {
            initializeMessenger();
            initializeCommand();
            Messenger.Default.Send(new RecipeInitializeModel
            {
                RecipeParameter = FilePath.RecipeParameter
            });
            timeoutTimer.Elapsed += TimeoutTimer_Elapsed;

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~Recipe_OPCUA_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //No read operations
            });
        }

        private void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (LoadingRecipe)
            {
                //opcUa.Read<bool>(RecipeTag.LoadRecipeComplete)               
                if (keyenceDll.Read<bool>(RecipeTag.LoadRecipeComplete))
                {
                    //opcUa.Write(RecipeTag.LoadRecipeComplete, false);
                    keyenceDll.Write<bool>(RecipeTag.LoadRecipeComplete, false);
                    LoadingRecipe = false;
                    if (Application.Current.Dispatcher.CheckAccess())
                    {
                        MessageBox.Show("Recipe Load Successfully.");
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                            MessageBox.Show("Recipe Load Successfully.");
                        }));
                    }
                    //MessageBox.Show("Recipe Load Successfully.");
                }
                else
                {
                    timerElapsed++;
                    if (timerElapsed > 10)
                    {
                        LoadingRecipe = false;
                        //opcUa.Write(RecipeTag.CheckRecipeFail, true);
                        keyenceDll.Write<bool>(RecipeTag.CheckRecipeFail, false);
                        if (Application.Current.Dispatcher.CheckAccess())
                        {
                            MessageBox.Show("Fail to load recipe");
                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                                MessageBox.Show("Fail to load recipe");
                            }));
                        }
                        //MessageBox.Show("Fail to load recipe");
                    }
                }
            }
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            RecipeComboBoxSelectionChangedCommand = new RelayCommand<object>(recipeComboBoxSelectionChangedCommand);
            RecipeComboBoxPreviewMouseDownCommand = new RelayCommand(recipeComboBoxPreviewMouseDownCommand);
            //LoadRecipeCommand = new RelayCommand<object>(TriggerLoadRecipe);
            LoadRecipeCommand = new RelayCommand<object>(loadRecipeCommand);
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<OPCUA_>(this, initializeOpc);
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        //private void initializeOpc(OPCUA_ opcUa_)
        //{
        //    try
        //    {
        //        if (opcUa_.OPC.Module.Contains(OPCModules.Recipe))
        //        {
        //            opcUa = opcUa_;
        //            recipeTagList = opcUa.GetAddresses();
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //    //recipeTagList = RecipeService.GetRecipeTagList();
        //    //// To get parameter tag from PLCConfig
        //    //recipeTagList = recipeTagList.Union(PLCConfigService.GetPLCConfigTag(PLCConfigService.DeserializePLCConfigParameter())).ToList();

        //    //List<RecipeParameterModel> RecipePara = RecipeService.DeserializeRecipeParameterList();
        //    //recipeTagList = recipeTagList.Union(RecipePara.Select(x => x.RecipeParameterTagModel.RecipeParameterTag)).ToList();

        //    //siemensOPC = new SiemensOPC(recipeTagList);
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void opcUpdate(_OPC _oPC)
        //{
        //    if (_oPC.Module.Contains(moduleName))
        //    {
        //        Task.Run(() => 
        //        {
        //            _oPC.Datachange.ToList().ForEach(node =>
        //            {
        //                if (node.Value != null)
        //                {
        //                    if (LoadingRecipe)
        //                    {
        //                        if (node.Key == RecipeTag.RecipeNotExist)
        //                        {
        //                            if (Convert.ToBoolean(node.Value))
        //                            {
        //                                opcUa.Write(RecipeTag.RecipeNotExist, false);
        //                                opcUa.Write(RecipeTag.CheckRecipeFail, true);
        //                                //LogHelper.KeyValue(GetType().Name, "PLC Recipe not exists", Convert.ToBoolean(siemensOPCModel.SiemensOPCTagModel.SiemensOPCValue));
        //                                MessageBox.Show("The recipe does not exist in PLC.");
        //                                LoadingRecipe = false;
        //                            }
        //                        }
        //                        else if (node.Key == RecipeTag.LoadRecipeComplete)
        //                        {
        //                            if (Convert.ToBoolean(node.Value))
        //                            {
        //                                LoadingRecipe = false;
        //                                opcUa.Write(RecipeTag.LoadRecipeComplete, false);
        //                                MessageBox.Show("Recipe Load Successfully.");
        //                            }
        //                        }
        //                        //else if (siemensOPCModel.SiemensOPCTagModel.SiemensOPCTag == RecipeTag.LoadRecipePLC)
        //                        //{
        //                        //    if (!Convert.ToBoolean(siemensOPCModel.SiemensOPCTagModel.SiemensOPCValue))
        //                        //    {
        //                        //        LoadingRecipe = false;
        //                        //        MessageBoxHelper.ShowMessageBox(MessageHelper.Done("Recipe load"));
        //                        //    }
        //                        //}
        //                    }
        //                }
        //            });
        //        });

        //    }
        //}
        #endregion

        #region PrivateCommandMethods
        private void recipeComboBoxPreviewMouseDownCommand()
        {
            RecipeList = null;
            RecipeList = CollectionViewSource.GetDefaultView(RecipeService.GetRecipeList());
        }
        private void recipeComboBoxSelectionChangedCommand(object recipeName)
        {
            if (recipeName != null)
            {
                SelectedRecipe = recipeName.ToString();
                if (!LoadingRecipe)
                    LoadRecipeAllowed = true;
            }
        }

        //bool isManual = false;
        string recipeNow;
        private void loadRecipeCommand(object recipeName)
        {
            try
            {
                //opcUa.Write(RecipeTag.CheckRecipePass, false);
                keyenceDll.Write<bool>(RecipeTag.CheckRecipePass, false);
                //opcUa.Write(RecipeTag.CheckRecipeFail, false);
                keyenceDll.Write<bool>(RecipeTag.CheckRecipeFail, false);
                //opcUa.Write(RecipeTag.LoadRecipeHMI, true);
                keyenceDll.Write<bool>(RecipeTag.LoadRecipeHMI, false);

                if (recipeName.ToString().Trim().Length != 0)
                {
                    List<RecipeParameterModel> newRecipe = RecipeService.DeserializeRecipe(recipeName.ToString());

                    newRecipe.ForEach(x =>
                    {
                        opcUa.Write(x.RecipeParameterTagModel.RecipeParameterTag, x.RecipeParameter);
                        System.Threading.Thread.Sleep(1);
                    });
                    //opcUa.Write(RecipeTag.CheckRecipePass, true);
                    keyenceDll.Write<bool>(RecipeTag.CheckRecipePass, true);

                    //bool isComplete = false;
                    //bool exceed30Loops = false;
                    //int loopingTimes = 0;
                    //Task.Run(() =>
                    //{
                    //    do
                    //    {
                    //        if (loopingTimes == 30)
                    //        {
                    //            exceed30Loops = true;
                    //        }

                    //        if (!exceed30Loops)
                    //        {
                    //            System.Threading.Thread.Sleep(1000);
                    //            isComplete = opcUa.Read<bool>(RecipeTag.LoadRecipeComplete);
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show($"Looping already exceed 30 times. {RecipeTag.LoadRecipeComplete} address not turn true.");
                    //            break;
                    //        }
                    //        loopingTimes++;

                    //    } while (!isComplete);

                    //    //MessageBox.Show("Recipe Loaded: " + recipeName.ToString());
                    //    if (!exceed30Loops)
                    //    {
                    //        MessageBox.Show($"Recipe Load Successfully.");
                    //        opcUa.Write(RecipeTag.LoadRecipeComplete, false);
                    LoadingRecipe = true;
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show($"Recipe Load Not Successful.");
                    //        opcUa.Write(RecipeTag.LoadRecipeComplete, false);
                    //        LoadingRecipe = false;
                    //    }
                    //});
                }
            }
            catch (Exception ex)
            {
                //opcUa.Write(RecipeTag.CheckRecipeFail, true);
                keyenceDll.Write<bool>(RecipeTag.CheckRecipeFail, true);
                LoadingRecipe = false;
                logger.Error(ex);
            }
        }

        #region NotUse
        private void TriggerLoadRecipe(object recipeName)
        {
            //isManual = true;
            recipeNow = recipeName.ToString();
            //opcUa.Write(RecipeTag.RequestRecipeName, recipeName);
            opcUa.Write(RecipeTag.LoadRecipeHMI, true);
            //opcUa.Write(RecipeTag.RecipeProcessNumber, 3);
        }
        #endregion
        #endregion
    }
}
