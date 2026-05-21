using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeHistoryTypeTimerTagModel : ViewModelBase
    {
        #region PublicProperties
        private string cycleTimeHistoryTypeTimerTag = string.Empty;
        public string CycleTimeHistoryTypeTimerTag
        {
            get { return cycleTimeHistoryTypeTimerTag; }
            set
            {
                if (cycleTimeHistoryTypeTimerTag != value)
                {
                    cycleTimeHistoryTypeTimerTag = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeTimerTag));
                }
            }
        }
        #endregion
    }
}