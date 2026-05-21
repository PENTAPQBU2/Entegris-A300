using HMI_Master_KX7500.Modules.Alarm;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HMI_SSDC_PIM.Modules.AlarmHistory
{
    /// <summary>
    /// Interaction logic for AlarmHistorySearchResultDialog.xaml
    /// </summary>
    public partial class AlarmHistorySearchResultDialog : Window
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

        private string startDate;
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private string endDate;
        public string EndDate
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

        public AlarmHistorySearchResultDialog(string startDate, string endDate, bool isErrorChecked, bool isWarningChecked, List<AlarmMessageModel> AlarmList)
        {

            try
            {
                InitializeComponent();
                DataContext = this;
                this.StartDate = startDate;
                this.EndDate = endDate;
                this.IsErrorChecked = isErrorChecked;
                this.IsWarningChecked = isWarningChecked;

                AlarmHistoryCollection = new ObservableCollection<AlarmMessageModel>(AlarmList);
                logger.Log(LogLevel.Debug, "Window Show");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

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
