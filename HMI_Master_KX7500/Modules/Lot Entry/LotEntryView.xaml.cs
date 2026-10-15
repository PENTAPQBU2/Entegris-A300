using CAMLine;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.LotEntry;
using HMI_Master_KX7500.ModulesView.LotEntry;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for LotEntryView.xaml
    /// </summary>
    public partial class LotEntryView : UserControl
    {
        #region PrivateFields
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        //private OPCUA_ opcUA = null;
        //private static LotEntryModel lotEntryModel = new LotEntryModel();

        public Entergris_TcpCommService entergris_TcpCommService = new Entergris_TcpCommService();
        #endregion

        #region Constructor
        public LotEntryView()
        {
            InitializeComponent();
            //try
            //{
            //    Messenger.Default.Register<OPCUA_>(this, initializeOPC);
            //    Messenger.Default.Register<_OPC>(this, opcUpdate);
            //    initializeLotEntry();
            //}
            //catch (Exception ex) { logger.Error(ex); }

            DataContext = new LotEntry_OPCUA_ViewModel();
        }
        #endregion

        #region PrivateInitialize
        //private void initializeLotEntry()
        //{
        //    try
        //    {
        //        if (!File.Exists(FilePath.LotEntry))
        //        {
        //            lotEntryModel = new LotEntryModel();

        //            lotEntryModel.NewLotBitNode = $"{Global.NodeNameSpace}DB_Lot_Info.New_Lot_Bit";
        //            lotEntryModel.EndLotBitNode = $"{Global.NodeNameSpace}DB_Lot_Info.End_Lot_Bit";
        //            lotEntryModel.LotIDNode = $"{Global.NodeNameSpace}DB_Lot_Info.New_Lot_ID_String";
        //            lotEntryModel.OperatorIDNode = $"{Global.NodeNameSpace}DB_Lot_Info.Operator_ID";
        //            lotEntryModel.LotQuantityNode = $"{Global.NodeNameSpace}DB_Lot_Info.Quantity";
        //            lotEntryModel.LotCreatedNode = $"{Global.NodeNameSpace}DB_Lot_Info.New_Lot_Created";
        //            //lotEntryModel.RecipeNode = $"{Global.NodeNameSpace}DB_Lot_Info.Current_Recipe_Name";
        //            //lotEntryModel.RecipeNode = $"{Global.NodeNameSpace}DB_Lot_Info.Recipe_ID";

        //            XmlTools.Serialize(FilePath.LotEntry, lotEntryModel);
        //        }

        //        lotEntryModel = XmlTools.Deserialize<LotEntryModel>(FilePath.LotEntry, lotEntryModel);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateMessenger
        //private void initializeOPC(OPCUA_ opcUA_)
        //{
        //    try
        //    {
        //        opcUA = opcUA_;
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}

        //private void opcUpdate(_OPC opcUA_)
        //{
        //    try
        //    {
        //        if (opcUA_.Module.Contains(OPCModules.LotEntry))
        //        {
        //            Dispatcher.Invoke(new Action(() =>
        //            {
        //                opcUA_.Datachange.ToList().ForEach(x =>
        //                {
        //                    if (x.Key == lotEntryModel.LotIDNode)
        //                    {
        //                        TextBoxLotID.Text = x.Value;
        //                    }
        //                    else if (x.Key == lotEntryModel.OperatorIDNode)
        //                    {
        //                        TextBoxOptID.Text = x.Value;
        //                    }
        //                    else if (x.Key == lotEntryModel.LotQuantityNode)
        //                    {
        //                        NumericUpDownLotQuantity.Value = Convert.ToDouble(x.Value);
        //                    }
        //                    else if (x.Key == lotEntryModel.LotCreatedNode)
        //                    {
        //                        NumericUpDownLotCreated.Value = Convert.ToDouble(x.Value);
        //                    }
        //                    //else if (x.Key == lotEntryModel.RecipeNode)
        //                    //{
        //                    //    ComboBoxRecipe.SelectedItem = x.Value;
        //                    //}
        //                });
        //            }));
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateEvents
        // MT_001: reload recipes (may be added/renamed on Recipe page)
        private void RecipeComboBox_DropDownOpened(object sender, EventArgs e)
        {
            (DataContext as LotEntry_OPCUA_ViewModel)?.RefreshRecipeList();
        }

        //private void NewLotClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string lotID = TextBoxLotID.Text;
        //        string optID = TextBoxOptID.Text;
        //        double? lotQuantity = NumericUpDownLotQuantity.Value;
        //        double? lotCreated = NumericUpDownLotCreated.Value;
        //        //string recipeName = ComboBoxRecipe.SelectedItem.ToString();

        //        if (string.IsNullOrEmpty(lotID))
        //        {
        //            MessageBox.Show("Please Fill In Lot ID");
        //        }
        //        else if (string.IsNullOrEmpty(optID))
        //        {
        //            MessageBox.Show("Please Fill In Operator ID");
        //        }
        //        else if (lotQuantity < 0)
        //        {
        //            MessageBox.Show("Please Fill Lot Quantity");
        //        }
        //        else if (lotCreated < 0)
        //        {
        //            MessageBox.Show("Please Fill Lot Created");
        //        }
        //        //else if (string.IsNullOrEmpty(recipeName))
        //        //{
        //        //    MessageBox.Show("Please Choose The Recipe");
        //        //}
        //        else
        //        {
        //            opcUA.Write(lotEntryModel.LotIDNode, lotID);
        //            opcUA.Write(lotEntryModel.OperatorIDNode, optID);
        //            opcUA.Write(lotEntryModel.LotQuantityNode, lotQuantity);
        //            opcUA.Write(lotEntryModel.LotCreatedNode, lotQuantity);
        //            //opcUA.Write(lotEntryModel.RecipeNode, recipeName);
        //            opcUA.Write(lotEntryModel.NewLotBitNode, true);
        //        }
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}

        //private void EndLotClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        opcUA.Write(lotEntryModel.EndLotBitNode, true);
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion
    }
}
