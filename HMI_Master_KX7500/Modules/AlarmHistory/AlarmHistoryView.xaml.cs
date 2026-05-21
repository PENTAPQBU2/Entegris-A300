using HMI_Master_KX7500.Modules.Alarm;
using HMI_SSDC_PIM.Modules.AlarmHistory;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HMI_Master_KX7500
{
    
    public partial class AlarmHistoryView : UserControl, INotifyPropertyChanged
    {
        #region Fields
        private Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties
        private ObservableCollection<AlarmMessageModel> alarmHistoryCollection;
        public ObservableCollection<AlarmMessageModel> AlarmHistoryCollection
        {
            get { return alarmHistoryCollection; }
            set
            {
                alarmHistoryCollection = value;
                OnPropertyChanged(nameof(AlarmHistoryCollection));
            }
        }

        private DateTime startDate = DateTime.Today;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime endDate = DateTime.Today;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private bool isErrorChecked = false;
        public bool IsErrorChecked
        {
            get { return isErrorChecked; }
            set
            {
                isErrorChecked = value;
                OnPropertyChanged(nameof(IsErrorChecked));
            }
        }

        private bool isWarningChecked = false;
        public bool IsWarningChecked
        {
            get { return isWarningChecked; }
            set
            {
                isWarningChecked = value;
                OnPropertyChanged(nameof(IsWarningChecked));
            }
        }
        #endregion

        #region Constructor
        public AlarmHistoryView()
        {
            try
            {
                InitializeComponent();
                DataContext = this;
            }
            catch (Exception ex)
            {
                // Log the error, if any
                logger.Error(ex);
            }
        }
        #endregion

        #region Public Methods
        public void AddAlarm(List<AlarmMessageModel> alarmList)
        {
            try
            {
                if(AlarmHistoryCollection == null)
                {
                    AlarmHistoryCollection = new ObservableCollection<AlarmMessageModel>(alarmList);
                }
                else
                {
                    List<AlarmMessageModel> oriList = new List<AlarmMessageModel>();
                    logger.Log(LogLevel.Debug, $"AlarmHistoryCollection Count: {AlarmHistoryCollection.Count}");
                    if (AlarmHistoryCollection.Count > 0)
                    {
                        oriList = AlarmHistoryCollection.ToList();
                        oriList = MergeNewErrorEntries(oriList, alarmList);
                    }
                    else
                    {
                        oriList.AddRange(alarmList);
                    }
                    if (oriList.Count > 20)
                        oriList.RemoveRange(0, oriList.Count - 20);
                    AlarmHistoryCollection = new ObservableCollection<AlarmMessageModel>(oriList);
                }
            }
            catch (Exception ex)
            {
                // Log the error, if any
                logger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private List<AlarmMessageModel> MergeNewErrorEntries(List<AlarmMessageModel> originalList, List<AlarmMessageModel> newList)
        {
            try
            {
                if (originalList == null || newList == null) return new List<AlarmMessageModel>();

                // Create a set of unique keys from the original list for fast lookup
                var existing = new HashSet<string>(
                    originalList.Select(e => $"{e.Code}|{e.Type}|{e.Time}"),
                    StringComparer.OrdinalIgnoreCase);

                foreach (var newItem in newList)
                {
                    string key = $"{newItem.Code}|{newItem.Type}|{newItem.Time}";
                    if (!existing.Contains(key))
                    {
                        originalList.Add(newItem);
                        existing.Add(key);
                    }
                }
                return originalList;
            }
            catch
            {
                throw;
            }
        }

        private async Task<List<AlarmMessageModel>> SearchAlarmLogs(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<AlarmMessageModel> alarmLogList = new List<AlarmMessageModel>();
                List<string> errorResult = new List<string>();
                List<string> warningResult = new List<string>();
                if (IsErrorChecked)
                {
                    errorResult = await AlarmLogger.ReadAlarmLog("Error", StartDate, EndDate);
                    logger.Log(LogLevel.Debug, $"Read Error Log Complete");
                }
                if (IsWarningChecked)
                {
                    warningResult = await AlarmLogger.ReadAlarmLog("Warning", StartDate, EndDate);
                    logger.Log(LogLevel.Debug, $"Read Warning Log Complete");
                }

                if (IsErrorChecked)
                {
                    foreach (string item in errorResult)
                    {
                        var details = item.Split(',');
                        var alarmLog = new AlarmMessageModel { Code = details[0], Type = details[1], Message = details[2], Time = details[3] };
                        alarmLogList.Add(alarmLog);
                        await Task.Delay(10);
                    }
                    logger.Log(LogLevel.Debug, $"Compile Error Log Done");
                }

                if (IsWarningChecked)
                {
                    foreach (string item in warningResult)
                    {
                        var details = item.Split(',');
                        var alarmLog = new AlarmMessageModel { Code = details[0], Type = details[1], Message = details[2], Time = details[3] };
                        alarmLogList.Add(alarmLog);
                        await Task.Delay(10);
                    }
                    logger.Log(LogLevel.Debug, $"Compile Warning Log Done");
                }

                return alarmLogList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region UI Events
        private async void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if(StartDate == DateTime.Today && EndDate == DateTime.Today)
                {
                    MessageBox.Show("Please select start date and end date", "Select Date", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    if (MessageBox.Show("This might takes awahile.\nNot encourage to use this function while running process.\nContinue?", "Search Alarm Log", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        logger.Log(LogLevel.Info, "User proceed to search alarm log");
                        this.IsEnabled = false;
                        // start searching
                        var res = await SearchAlarmLogs(StartDate, EndDate);

                        AlarmHistorySearchResultDialog resultWindow = new AlarmHistorySearchResultDialog(StartDate.ToString("yyyy - MMM - dd"), EndDate.ToString("yyyy - MMM - dd"), IsErrorChecked, IsWarningChecked, res);
                        resultWindow.ShowDialog();
                        this.IsEnabled = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                this.IsEnabled = true;
            }
        }
        #endregion

        #region Notify Property Change
        public event PropertyChangedEventHandler PropertyChanged;

        // The method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
