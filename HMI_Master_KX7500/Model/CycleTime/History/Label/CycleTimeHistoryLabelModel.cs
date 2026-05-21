using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeHistoryLabelModel : ViewModelBase
    {
        #region PublicProperties
        private string cycleTimeHistoryLabelKey = string.Empty;
        public string CycleTimeHistoryLabelKey
        {
            get { return cycleTimeHistoryLabelKey; }
            set
            {
                if (cycleTimeHistoryLabelKey != value)
                {
                    cycleTimeHistoryLabelKey = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryLabelKey));
                }
            }
        }

        private string cycleTimeHistoryLabelName = string.Empty;
        [XmlIgnore]
        public string CycleTimeHistoryLabelName
        {
            get { return cycleTimeHistoryLabelName; }
            set
            {
                if (cycleTimeHistoryLabelName != value)
                {
                    cycleTimeHistoryLabelName = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryLabelName));
                }
            }
        }
        #endregion
    }
}