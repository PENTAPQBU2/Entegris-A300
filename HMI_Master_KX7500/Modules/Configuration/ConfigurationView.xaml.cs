using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model;
using HMI_Master_KX7500.Model.Alarm;
using HMI_Master_KX7500.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
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
using System.Xml;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor
        public ConfigurationView()
        {
            InitializeComponent();
        }
        #endregion

        #region PrivateEvent

        //private void importCommand(string ALarmListFilePath,string ALarmMdbFilePath)
        //{
        //    try
        //    {
        //        //string text = FileFolderTools.OpenFile("*.xlsx");
        //        //if (string.IsNullOrEmpty(text))
        //        //{
        //        //    return;
        //        //}
        //        //string text2 = FileFolderTools.SaveFile("*.mdb");
        //        //if (string.IsNullOrEmpty(text2))
        //        //{
        //        //    return;
        //        //}
        //        //MessageBox.Show("Please click OK to continue import and please wait.");

        //        using (OleDbConnection oleDbConnection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={ALarmListFilePath};Extended Properties =\"Excel 12.0;HDR=NO;IMEX=1;\""))
        //        {
        //            oleDbConnection.Open();
        //            DataTable dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //            foreach (DataRow dataRow in dataTable.Rows)
        //            {

        //            }
        //        }

        //        string[] Error_WarningSheet = { "Error List", "Warning List" };
        //        Excel.Application xlApp = new Excel.Application();
        //        Excel.Workbook Workbook = xlApp.Workbooks.Open(ALarmListFilePath);

        //        //Workbook = ((Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")))).Workbooks.Open(text, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //       // Range range = null;
        //        AlarmV1Service alarmV1Service = new AlarmV1Service(ALarmMdbFilePath);
        //        if (alarmV1Service.AlarmV1ModelTableExists())
        //        {
        //            alarmV1Service.DropAlarmV1ModelTable();
        //        }
        //        alarmV1Service.CreateAlarmV1ModelTable();
        //        AlarmV1Model alarmV1Model = new AlarmV1Model();

        //        foreach (Excel._Worksheet xlWorkSheet in Workbook.Sheets)
        //        {
        //            if (!Error_WarningSheet.Contains(xlWorkSheet.Name)) continue;
        //            Excel.Range range = xlWorkSheet.UsedRange;
        //            // remember check the file the record is start from row 7
        //            for (int j = 7; j <= range.Rows.Count; j++)
        //            {
        //                if ((dynamic)range.Cells[j, 2] != null && ((dynamic)range.Cells[j, 2]).Value2 != null)
        //                {

        //                    alarmV1Service.InsertAlarmV1Model((xlWorkSheet.Name == "Warning List") ? AlarmType.Warning : AlarmType.Error, (((dynamic)range.Cells[j, 3]).Value2 == null) ? alarmV1Model.AlarmModule : ((dynamic)range.Cells[j, 3]).Value2.ToString(), (((dynamic)range.Cells[j, 2]).Value2 == null) ? alarmV1Model.AlarmCode : ((dynamic)range.Cells[j, 2]).Value2.ToString(), (((dynamic)range.Cells[j, 4]).Value2 == null) ? alarmV1Model.AlarmMessageEnglish : ((dynamic)range.Cells[j, 4]).Value2.ToString(), (((dynamic)range.Cells[j, 5]).Value2 == null) ? alarmV1Model.AlarmActionEnglish : ((dynamic)range.Cells[j, 5]).Value2.ToString(), (((dynamic)range.Cells[j, 6]).Value2 == null) ? alarmV1Model.AlarmMessageChinese : ((dynamic)range.Cells[j, 6]).Value2.ToString(), (((dynamic)range.Cells[j, 7]).Value2 == null) ? alarmV1Model.AlarmActionChinese : ((dynamic)range.Cells[j, 7]).Value2.ToString());
        //                }
        //            }
        //        }

        //        //    for (int i = 13; i <= Workbook.Worksheets.Count; i++)
        //        //{
        //        //    range = ((_Worksheet)(dynamic)Workbook.Worksheets[i]).UsedRange;
        //        //    if (i != 13 && i != 14)
        //        //    {
        //        //        continue;
        //        //    }
        //        //    for (int j = 7; j <= range.Rows.Count; j++)
        //        //    {
        //        //        if ((dynamic)range.Cells[j, 2] != null && ((dynamic)range.Cells[j, 2]).Value2 != null)
        //        //        {
        //        //            alarmV1Service.InsertAlarmV1Model((i == 13) ? AlarmType.Warning : AlarmType.Error, (((dynamic)range.Cells[j, 3]).Value2 == null) ? alarmV1Model.AlarmModule : ((dynamic)range.Cells[j, 3]).Value2.ToString(), (((dynamic)range.Cells[j, 2]).Value2 == null) ? alarmV1Model.AlarmCode : ((dynamic)range.Cells[j, 2]).Value2.ToString(), (((dynamic)range.Cells[j, 4]).Value2 == null) ? alarmV1Model.AlarmMessageEnglish : ((dynamic)range.Cells[j, 4]).Value2.ToString(), (((dynamic)range.Cells[j, 5]).Value2 == null) ? alarmV1Model.AlarmActionEnglish : ((dynamic)range.Cells[j, 5]).Value2.ToString(), (((dynamic)range.Cells[j, 6]).Value2 == null) ? alarmV1Model.AlarmMessageChinese : ((dynamic)range.Cells[j, 6]).Value2.ToString(), (((dynamic)range.Cells[j, 7]).Value2 == null) ? alarmV1Model.AlarmActionChinese : ((dynamic)range.Cells[j, 7]).Value2.ToString());
        //        //        }
        //        //    }
        //        //}
        //        MessageBox.Show("Import done.");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error<Exception>(ex);
        //        MessageBox.Show("Fail to import.");
        //    }
        //}
        private void AlarmListImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //CommonFileDialogFilterCollection ExcelAlarmcommonFileDialogFilters = new CommonFileDialogFilterCollection() { };
                //ExcelAlarmcommonFileDialogFilters.Add ( new CommonFileDialogFilter("Excel Alarm List", "*.xlsx") );
                //CommonFileDialogFilterCollection MdbAlarmcommonFileDialogFilters = new CommonFileDialogFilterCollection();
                //    MdbAlarmcommonFileDialogFilters.Add (new CommonFileDialogFilter("Mdb Alarm List", "*.mdb") );

                if (ShowAlarmOpenFileDialog("xlsx",out string ALarmListFilePath) == CommonFileDialogResult.Ok)
                {
                    if (ShowAlarmOpenFileDialog("mdb",out string ALarmMdbFilePath) == CommonFileDialogResult.Ok)
                    {
                        MessageBox.Show("Please Wait Until Import Is Done. Do Not Close The HMI");
                        //importCommand(ALarmListFilePath, ALarmMdbFilePath);
                        generateAlarmList(ALarmListFilePath, ALarmMdbFilePath);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        private void ioListImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(ShowOpenFileDialog(out string ioListFilePath) == CommonFileDialogResult.Ok)
                {
                    MessageBox.Show("Please Wait Until Import Is Done. Do Not Close The HMI");
                    generateIoList(ioListFilePath);
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMethods
        private static CommonFileDialogResult ShowOpenFileDialog( out string ioListFilePath)
        {
            CommonFileDialogResult commonFileDialogResult = CommonFileDialogResult.None;
            ioListFilePath = string.Empty;

            try
            {
                using (CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog())
                {
                    commonFileDialogResult = commonOpenFileDialog.ShowDialog();

                    if (commonFileDialogResult == CommonFileDialogResult.Ok)
                    {
                        ioListFilePath = commonOpenFileDialog.FileName;

                        if (string.IsNullOrEmpty(ioListFilePath))
                        {
                            commonFileDialogResult = CommonFileDialogResult.None;
                        }
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            return commonFileDialogResult;
        }
        private static CommonFileDialogResult ShowAlarmOpenFileDialog(string FilterFile, out string ioListFilePath)
        {
            CommonFileDialogResult commonFileDialogResult = CommonFileDialogResult.None;
            ioListFilePath = string.Empty;

            try
            {
                using (CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog())
                {
                    if (FilterFile == "xlsx") { commonOpenFileDialog.Filters.Add(new CommonFileDialogFilter("Excel Alarm List", "*.xlsx")); }
                    else if (FilterFile == "mdb") { commonOpenFileDialog.Filters.Add(new CommonFileDialogFilter("Mdb Alarm List", "*.mdb")); }
                    else { commonOpenFileDialog.Filters.Add(new CommonFileDialogFilter("All File", "*.*")); }
                    commonFileDialogResult = commonOpenFileDialog.ShowDialog();

                    if (commonFileDialogResult == CommonFileDialogResult.Ok)
                    {
                        ioListFilePath = commonOpenFileDialog.FileName;

                        if (string.IsNullOrEmpty(ioListFilePath))
                        {
                            commonFileDialogResult = CommonFileDialogResult.None;
                        }
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            return commonFileDialogResult;
        }

        private void generateIoList(string ioListFilePath)
        {
            try
            {
                int RowCount = 0;
                IOModel ioModel = new IOModel();
                List<IOInputModel> initializeInputModel = new List<IOInputModel>();
                List<IOOutputModel> initializeOutputModel = new List<IOOutputModel>();

                string ignorePrintArea = "print_area";

                using (OleDbConnection oleDbConnection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={ioListFilePath};Extended Properties =\"Excel 12.0;HDR=NO;IMEX=1;\""))
                {
                    
                    oleDbConnection.Open();
                    DataTable dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null); 
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        //read the sheet name
                        string sheetName = dataRow["TABLE_NAME"].ToString();
                        string subString = sheetName.Substring(0, 1);

                        if (sheetName.ToLower().Contains(ignorePrintArea)) continue;

                        using (OleDbCommand oleDbCommand = new OleDbCommand($"SELECT * FROM [{dataRow["TABLE_NAME"].ToString()}]", oleDbConnection))
                        {
                            using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
                            {
                                RowCount = 1;
                                while (oleDbDataReader.Read())
                                {
                                    RowCount++;
                                    if (RowCount >= 9 && RowCount <= 25)
                                    {
                                        string module = string.Empty;
                                        string description = string.Empty;

                                        if (subString == "I")
                                        {
                                            #region Input
                                            string inputAddress = string.Empty;
                                            string inputTag = string.Empty;
                                            #endregion

                                            inputAddress = oleDbDataReader[0].ToString();
                                            inputTag = oleDbDataReader[1].ToString();
                                            module = oleDbDataReader[2].ToString();
                                            description = oleDbDataReader[3].ToString();


                                            if (!string.IsNullOrEmpty(inputTag))
                                            {
                                                initializeInputModel.Add(new IOInputModel
                                                {
                                                    IOInputKey = $"{inputAddress} - {inputTag} - {description}",
                                                    IO_InputModule = module,
                                                    IOInputTagModel = new IOInputTagModel
                                                    {
                                                        IOInputTag = inputTag
                                                    }
                                                });
                                            }
                                        }
                                        else if (subString == "O")
                                        {
                                            #region Output
                                            string outputAddress = string.Empty;
                                            string outputTag = string.Empty;
                                            #endregion

                                            outputAddress = oleDbDataReader[0].ToString();
                                            outputTag = oleDbDataReader[1].ToString();
                                            module = oleDbDataReader[2].ToString();
                                            description = oleDbDataReader[3].ToString();

                                            if (!string.IsNullOrEmpty(outputTag))
                                            {
                                                initializeOutputModel.Add(new IOOutputModel
                                                {
                                                    IOOutputKey = $"{outputAddress} - {outputTag} - {description}",
                                                    IO_OutputModule = module,
                                                    IOOutputTagModel = new IOOutputTagModel
                                                    {
                                                        IOOutputTag = outputTag
                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                List<string> moduleList = new List<string>();
                
                var module11 = initializeInputModel.Select(z => z.IO_InputModule).Union(initializeOutputModel.Select(z=>z.IO_OutputModule)).Distinct().ToList();

                foreach (var moduleName in module11)
                {
                    List<IOInputModel> tempInputModel = new List<IOInputModel>();
                    List<IOOutputModel> tempOutputModel = new List<IOOutputModel>();

                    initializeInputModel.ForEach(x =>
                    {
                        if(x.IO_InputModule == moduleName)
                        {
                            tempInputModel.Add(new IOInputModel
                            {
                                IOInputKey = x.IOInputKey,
                                IO_InputModule = x.IO_InputModule,
                                IOInputTagModel = new IOInputTagModel
                                {
                                    IOInputTag = x.IOInputTagModel.IOInputTag
                                }
                            });
                        }
                    });

                    initializeOutputModel.ForEach(x =>
                    {
                        if (x.IO_OutputModule == moduleName)
                        {
                            tempOutputModel.Add(new IOOutputModel
                            {
                                IOOutputKey = x.IOOutputKey,
                                IO_OutputModule = x.IO_OutputModule,
                                IOOutputTagModel = new IOOutputTagModel
                                {
                                    IOOutputTag = x.IOOutputTagModel.IOOutputTag
                                }
                            });
                        }
                    });

                    ioModel.IOItemsModels.Add(new IOItemsModel
                    {
                        IOModule = moduleName,
                        IOInputList = tempInputModel,
                        IOOutputList = tempOutputModel
                    });
                }

                using (StringWriter str = new StringWriter())
                {
                    XmlDocument doc = new XmlDocument();
                    XmlSerializer serializer = new XmlSerializer(ioModel.GetType());
                    serializer.Serialize(str, ioModel);
                    doc.LoadXml(str.ToString());
                    doc.Save(FilePath.IO);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                //xlWorkbook.Close();
                //xlApp.Quit();
                MessageBox.Show("IO List Import Done");
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void generateAlarmList(string ALarmListFilePath, string ALarmMdbFilePath)
        {
            try
            {
                Task.Run(() =>
                {
                    string[] Error_WarningSheet = { "Error List", "Warning List" };
                    AlarmV1Service alarmV1Service = new AlarmV1Service(ALarmMdbFilePath);
                    int RowCount = 0;

                    if (alarmV1Service.AlarmV1ModelTableExists())
                    {
                        alarmV1Service.DropAlarmV1ModelTable();
                    }
                    alarmV1Service.CreateAlarmV1ModelTable();
                    AlarmV1Model alarmV1Model = new AlarmV1Model();

                    using (OleDbConnection oleDbConnection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={ALarmListFilePath};Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\""))
                    {
                        oleDbConnection.Open();
                        DataTable dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            string sheetName = dataRow["TABLE_NAME"].ToString();
                            foreach (string x in Error_WarningSheet)
                            {
                                if (sheetName.Contains(x))
                                {
                                    using (OleDbCommand oleDbCommand = new OleDbCommand($"SELECT * FROM [{dataRow["TABLE_NAME"].ToString()}]", oleDbConnection))
                                    {
                                        using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
                                        {
                                            RowCount = 1;
                                            while (oleDbDataReader.Read())
                                            {
                                                RowCount++;
                                                // remember check the file the record is start from row 7
                                                if (RowCount >= 7 && RowCount <= 1006)
                                                {
                                                    string module = string.Empty;
                                                    module = oleDbDataReader[2].ToString();
                                                    //if (!string.IsNullOrEmpty(module))
                                                    //{
                                                    //alarmV1Service.InsertAlarmV1Model((xlWorkSheet.Name == "Warning List") ? AlarmType.Warning : AlarmType.Error, (((dynamic)range.Cells[j, 3]).Value2 == null) ? alarmV1Model.AlarmModule : ((dynamic)range.Cells[j, 3]).Value2.ToString(), (((dynamic)range.Cells[j, 2]).Value2 == null) ? alarmV1Model.AlarmCode : ((dynamic)range.Cells[j, 2]).Value2.ToString(), (((dynamic)range.Cells[j, 4]).Value2 == null) ? alarmV1Model.AlarmMessageEnglish : ((dynamic)range.Cells[j, 4]).Value2.ToString(), (((dynamic)range.Cells[j, 5]).Value2 == null) ? alarmV1Model.AlarmActionEnglish : ((dynamic)range.Cells[j, 5]).Value2.ToString(), (((dynamic)range.Cells[j, 6]).Value2 == null) ? alarmV1Model.AlarmMessageChinese : ((dynamic)range.Cells[j, 6]).Value2.ToString(), (((dynamic)range.Cells[j, 7]).Value2 == null) ? alarmV1Model.AlarmActionChinese : ((dynamic)range.Cells[j, 7]).Value2.ToString());
                                                    alarmV1Service.InsertAlarmV1Model((sheetName.Contains("Warning List")) ? AlarmType.Warning : AlarmType.Error, (string.IsNullOrEmpty(oleDbDataReader[2].ToString())) ? alarmV1Model.AlarmModule : oleDbDataReader[2].ToString(), (string.IsNullOrEmpty(oleDbDataReader[1].ToString())) ? alarmV1Model.AlarmCode : oleDbDataReader[1].ToString(), (string.IsNullOrEmpty(oleDbDataReader[3].ToString())) ? alarmV1Model.AlarmMessageEnglish : oleDbDataReader[3].ToString(), (string.IsNullOrEmpty(oleDbDataReader[4].ToString())) ? alarmV1Model.AlarmActionEnglish : oleDbDataReader[4].ToString(), (string.IsNullOrEmpty(oleDbDataReader[5].ToString())) ? alarmV1Model.AlarmMessageChinese : oleDbDataReader[5].ToString(), (string.IsNullOrEmpty(oleDbDataReader[6].ToString())) ? alarmV1Model.AlarmActionChinese : oleDbDataReader[6].ToString());
                                                    //}

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    MessageBox.Show("Import done.");
                });
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(ex);
                MessageBox.Show("Fail to import.");
            }
        }
        #endregion
    }
}
