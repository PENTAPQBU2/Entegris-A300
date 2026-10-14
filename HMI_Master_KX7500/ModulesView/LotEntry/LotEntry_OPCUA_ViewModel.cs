using CAMLine;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Interface;
using HMI_Master_KX7500.Model.LotEntry;
using HMI_Master_KX7500.Model.LotRecipe;
using HMI_Master_KX7500.Service.LotEntry;
using HMI_Master_KX7500.Service.LotRecipe;
using MahApps.Metro.Controls;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.LotEntry
{
    public class LotEntry_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private List<string> lotEntryTagList = new List<string>();
        //private OPCUA_ opcUa = new OPCUA_();

        private KeyenceDll keyenceDll = null;
        public Entergris_TcpCommService entergris_TcpCommService = new Entergris_TcpCommService();

        private SetupSettingModel _tcpIPModel = new SetupSettingModel();
        #endregion

        #region PublicProperties
        private LotEntryInterfaceModel lotEntryInterfaceModel = new LotEntryInterfaceModel();
        public LotEntryInterfaceModel LotEntryInterfaceModel
        {
            get { return lotEntryInterfaceModel; }
            set
            {
                if (lotEntryInterfaceModel != value)
                {
                    lotEntryInterfaceModel = value;
                    RaisePropertyChanged(nameof(LotEntryInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public LotEntry_OPCUA_ViewModel()
        {
            try
            {
                initializeMessenger();
            }
            catch (Exception ex) { logger.Error(ex); }

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            LoadSettingService.OnUpdate_ServerSetting += new LoadSettingService.UpdateServerSetting(LoadServerSetting);
            LoadSettingService.LoadServerSetting();

            Event.UpdateEvent += Update;
        }

        ~LotEntry_OPCUA_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        public void Update()
        {
            if (Global.PageList["Lot Entry"])
            {
                if (keyenceDll.Read<bool>(PLCTag.Trigger_HMI_Send_Start_Job))
                {
                    try
                    {
                        FileLogger_Utilities.TCPIP_Log("sendStartJob is true");

                        string batchID = string.Empty;
                        string serialNumber = keyenceDll.Read<string>(PLCTag.Foup_Barcode, 20);
                        string equipID = keyenceDll.Read<string>(PLCTag.Equipment_ID, 26);

                        batchID = keyenceDll.Read<string>(PLCTag.Lot_ID, 50);
                        FileLogger_Utilities.TCPIP_Log($"Details : {batchID}, {serialNumber}, {equipID}");

                        entergris_TcpCommService.tcpIpClient?.Send($"START_JOB|BATCHID:{batchID}|SERIAL_NO:{serialNumber}|EQID:{equipID}");
                        keyenceDll.Write(PLCTag.Trigger_HMI_Send_Start_Job, false);
                    }
                    catch (Exception ex) { logger.Error(ex); }
                }
                else if (keyenceDll.Read<bool>(PLCTag.Overall_Pass))
                {
                    string batchID = string.Empty;
                    string serialNumber = keyenceDll.Read<string>(PLCTag.Foup_Barcode, 20);
                    string equipID = keyenceDll.Read<string>(PLCTag.Equipment_ID, 26);
                    //int totalFoupLevel = siemensOPC.ReadTag<int>(Recipe_Tag.TotalContactPosition);
                    //string camlineString = string.Empty;
                    //List<string> camlineCombinedList = new List<string>();

                    //int totalCount = siemensOPC.ReadTag<int>(Recipe_Tag.TotalContactPosition);

                    //for (int i = 1; i <= totalCount; i++)
                    //{
                    //    //Math.Round(Convert.ToDouble(x.Value), 3)
                    //    foupDataList.Find(y => y.Index == i).FoupDataValue = siemensOPC.ReadTag<double>(foupDataList.Find(y => y.Index == i).FoupDataTag);
                    //    camlineString = $"VALUE:FR{foupDataList.Find(y => y.Index == i).Index},{Math.Round(Convert.ToDouble(foupDataList.Find(y => y.Index == i).FoupDataValue), 5)}";
                    //    FileLogger_Utilities.TCPIP_Log($"camlineString : {camlineString}");
                    //    camlineCombinedList.Add(camlineString);
                    //}

                    //string delimiter = "|";
                    //string resultString = string.Join(delimiter, camlineCombinedList);
                    //FileLogger_Utilities.TCPIP_Log($"resultString : {resultString}");

                    //if (resultString != null)
                    //{
                    //    FileLogger_Utilities.TCPIP_Log("Result String Is Not Null");
                    //    FileLogger_Utilities.TCPIP_Log("resultPass is true");
                    //    entergris_TcpCommService.tcpIpClient?.Send($"COMPLETED|BATCHID:{batchID}|SERIAL_NO:{serialNumber}|EQID:{equipID}|RESULT:PASS|{resultString}");
                    //    siemensOPC.WriteTag(LotEntry_Tag.Trigger_HMI_SendJobComplete, false);
                    //}
                    //else { FileLogger_Utilities.TCPIP_Log("Result String Is Null"); }

                    entergris_TcpCommService.tcpIpClient?.Send($"COMPLETED|BATCHID:{batchID}|SERIAL_NO:{serialNumber}|EQID:{equipID}|RESULT:PASS");
                    keyenceDll.Write<bool>(PLCTag.Trigger_HMI_Send_Job_Complete, false);
                }
                else if (keyenceDll.Read<bool>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.TSOColumnTag) != LotEntryInterfaceModel.LotEntryModel.TSOColumnValue)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LotEntryInterfaceModel.LotEntryModel.TSOColumnValue = keyenceDll.Read<bool>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.TSOColumnTag);
                    });
                }
            }           
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            initializeLotEntry();
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        //private void initializeOpcUa(OPCUA_ opcUa_)
        //{
        //    try
        //    {
        //        if (opcUa_.OPC.Module.Contains(OPCModules.LotEntry))
        //        {
        //            opcUa = opcUa_;
        //            lotEntryTagList = opcUa.GetAddresses();
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateCommandMethods
        //New Lot (MT_001): the recipe gives the recipe ID/name and the shell colour bit (= robot point set)
        private void newLotCommand(string lotID)
        {
            LotEntryModel model = LotEntryInterfaceModel.LotEntryModel;
            LotRecipeModel recipe = model.SelectedRecipe;
            if (recipe == null)
            {
                model.LotMessage = "Select a recipe.";
                return;
            }

            writeNewLot(lotID, recipe);
            model.LotMessage = $"Lot {lotID} created with recipe {recipe.RecipeName} (shell colour {recipe.ShellColour}).";
        }

        private void writeNewLot(string lotID, LotRecipeModel recipe)
        {
            try
            {
                keyenceDll.Write<string>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Work_Order_ID_Tag, lotID, 50);
                keyenceDll.Write<string>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.OperatorID_Tag, LotEntryInterfaceModel.LotEntryModel.OperatorID, 10);
                keyenceDll.Write<int>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Recipe_Tag, recipe.RecipeID);
                keyenceDll.Write<string>(PLCTag.Lot_Recipe_Name, recipe.RecipeName, 20);
                keyenceDll.Write<int>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Lot_Quantity_Tag, LotEntryInterfaceModel.LotEntryModel.Lot_Quantity);
                keyenceDll.Write<int>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Current_Quantity_Tag, LotEntryInterfaceModel.LotEntryModel.Current_Quantity);

                //Same as the old Black/Transparent buttons: set the colour bit; on its rising edge the PLC
                //resets the other colour bits and moves the robot point set number to W0102 (shell_int)
                keyenceDll.Write<bool>(PLCTag.ShellColourTag(recipe.ShellColour), true);
                LotEntryInterfaceModel.LotEntryModel.Shell_Colour = recipe.ShellColour.ToString();
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void loadRecipes()
        {
            LotEntryModel model = LotEntryInterfaceModel.LotEntryModel;
            int selectedId = model.SelectedRecipe?.RecipeID ?? 0;
            model.Recipes.Clear();
            foreach (LotRecipeModel recipe in LotRecipeService.LoadAll())
            {
                model.Recipes.Add(recipe);
            }
            model.SelectedRecipe = model.Recipes.FirstOrDefault(x => x.RecipeID == selectedId);
        }

        private void endLotCommand()
        {
            try
            {
                keyenceDll.Write<bool>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.End_Work_Order_Button_Tag, true);
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void shellColourBlackCommand()
        {
            try
            {
                keyenceDll.Write<bool>("MR4500", true);
                LotEntryInterfaceModel.LotEntryModel.Shell_Colour = "Black";
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void transparentCommand()
        {
            try
            {
                keyenceDll.Write<bool>("MR4501", true);
                LotEntryInterfaceModel.LotEntryModel.Shell_Colour = "Transparent";
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeLotEntry()
        {
            try
            {
                LotEntryInterfaceModel.LotEntryModel = LotEntryService.DeserializeLotEntry();
                LotEntryInterfaceModel.LotEntryModel.NewLotCommand = new RelayCommand<string>(newLotCommand);
                LotEntryInterfaceModel.LotEntryModel.TSOColumnCommand = new RelayCommand<object>(TSOColumnCommandEvent);
                LotEntryInterfaceModel.LotEntryModel.EndLotCommand = new RelayCommand(endLotCommand);

                LotEntryInterfaceModel.LotEntryModel.ShellColourBlackCommand = new RelayCommand(shellColourBlackCommand);
                LotEntryInterfaceModel.LotEntryModel.TransparentCommand = new RelayCommand(transparentCommand);

                LotRecipeService.EnsureDefaultRecipes();
                loadRecipes();
                LotRecipeService.RecipesChanged += () => Application.Current?.Dispatcher.BeginInvoke(new Action(loadRecipes));

                //Messenger.Default.Register<OPCUA_>(this, initializeOpcUa);
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void TSOColumnCommandEvent(object obj)
        {
            try
            {
                keyenceDll.Write<bool>(LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.TSOColumnTag, LotEntryInterfaceModel.LotEntryModel.TSOColumnValue);
            }
            catch (Exception ex) { logger.Error(ex);}
        }

        //private void opcUpdate(_OPC opc_)
        //{
        //    try
        //    {
        //        if (opc_.Module.Contains(OPCModules.LotEntry))
        //        {

        //            opc_.Datachange.ToList().ForEach(node =>
        //            {
        //                Task.Run(() =>
        //                {

        //                    if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.LotIDTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.LotID = node.Value;
        //                    }
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.LotCreatedTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.LotCreated = Convert.ToBoolean(node.Value);
        //                    }
        //                    //else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.OkToEndLotTag == node.Key)
        //                    //{
        //                    //    LotEntryInterfaceModel.LotEntryModel.OkToEndLot = Convert.ToBoolean(node.Value);
        //                    //}
        //                    //else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.RecipeValueTag == node.Key)
        //                    //{
        //                    //    LotEntryInterfaceModel.LotEntryModel.RecipeValue = Convert.ToInt32(node.Value);
        //                    //}
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.OperatorIDTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.OperatorID = node.Value;
        //                    }
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.LotQuantityTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.LotQuantity = node.Value;
        //                    }
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Customer_IDTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.Customer_ID = node.Value;
        //                    }
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Recipe_IDTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.Recipe_ID = node.Value;
        //                    }
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Current_Recipe_NameTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.Current_Recipe_Name = node.Value;
        //                    }
        //                    else if (LotEntryInterfaceModel.LotEntryModel.LotEntryTagModel.Pin_Material_IDTag == node.Key)
        //                    {
        //                        LotEntryInterfaceModel.LotEntryModel.Pin_Material_ID = node.Value;
        //                    }
        //                });
        //            });
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}

        private void LoadServerSetting(SetupSettingModel tcpIPSettingModel)
        {
            this._tcpIPModel = tcpIPSettingModel;
        }
        #endregion
    }
}
