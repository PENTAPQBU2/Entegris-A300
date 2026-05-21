using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.LotEntry;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.Service.LotEntry
{
    public class LotEntryService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string IsInDesignMode = "IsInDesignMode";

        #region PublicMethods
        public static bool SerializeLotEntry(LotEntryModel lotEntryModel)
        {
            bool value = false;
            try
            {
                if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.Work_Order_Started_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.Work_Order_Started_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.Work_Order_ID_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.Work_Order_ID_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.Set_Work_Order_Info_Button_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.Set_Work_Order_Info_Button_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.End_Work_Order_Button_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.End_Work_Order_Button_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.OperatorID_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.OperatorID_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.Recipe_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.Recipe_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.Lot_Quantity_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.Lot_Quantity_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.OK_To_End_Work_Order_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.OK_To_End_Work_Order_Tag)} Cannot Be Null/Empty");
                }
                else if (string.IsNullOrEmpty(lotEntryModel.LotEntryTagModel.Current_Quantity_Tag))
                {
                    MessageBox.Show($"{nameof(LotEntryTagModel.Current_Quantity_Tag)} Cannot Be Null/Empty");
                }
                else
                {
                    XmlTools.Serialize(FilePath.LotEntry, lotEntryModel);

                    value = true;
                }
            }
            catch (Exception ex) { logger.Error(ex); }

            return value;
        }

        public static LotEntryModel DeserializeLotEntry()
        {
            LotEntryModel lotEntryModel = new LotEntryModel();

            try
            {
                if (!File.Exists(FilePath.LotEntry))
                {
                    lotEntryModel = new LotEntryModel
                    {
                        LotEntryTagModel = new LotEntryTagModel
                        {
                            Work_Order_Started_Tag = "MR49500",
                            Work_Order_ID_Tag = "EM1501",
                            Set_Work_Order_Info_Button_Tag = "MR49501",
                            End_Work_Order_Button_Tag = "MR49502",
                            OperatorID_Tag = "EM1553",
                            Recipe_Tag = "EM1559",
                            Lot_Quantity_Tag = "EM1561",
                            OK_To_End_Work_Order_Tag = "MR49503",
                            Current_Quantity_Tag = "EM1634",
                            TSOColumnTag = "MR4515",
                           
                            //OkToStopLotTag = SiemensOPCHelper.GetBooleanTag(s7Connection, dataBlockNumber, 514.3),
                            //OkToEndLotTag = OPCUAHelper.CombinedNodeString(DB_Lot_Info, "Stop_Lot_Bit"),
                            //StopLotTag = SiemensOPCHelper.GetBooleanTag(s7Connection, dataBlockNumber, 514.1),
                            //RecipeValueTag = SiemensOPCHelper.GetS7ConnectionTag(1) + "DB850,DINT528"
                        }
                    };

                    SerializeLotEntry(lotEntryModel);
                }

                lotEntryModel = XmlTools.Deserialize<LotEntryModel>(FilePath.LotEntry, lotEntryModel);
            }
            catch (Exception ex) { logger.Error(ex); }

            return lotEntryModel;
        }

        public static List<string> GetLotEntryTagList()
        {
            LotEntryModel lotEntryModel = DeserializeLotEntry();
            List<string> lotEntryTagList = new List<string>();

            lotEntryTagList = lotEntryModel.LotEntryTagModel.GetType().GetProperties().Where(
                   x => x.Name != IsInDesignMode).Select(
                   x => x.GetValue(lotEntryModel.LotEntryTagModel).ToString()).ToList();

            return lotEntryTagList;
        }
        #endregion
    }
}
