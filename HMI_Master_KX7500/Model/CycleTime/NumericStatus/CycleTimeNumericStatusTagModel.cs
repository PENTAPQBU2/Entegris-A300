using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeNumericStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string cycleTimeNumericStatusTag = string.Empty;
        public string CycleTimeNumericStatusTag
        {
            get { return cycleTimeNumericStatusTag; }
            set
            {
                if (cycleTimeNumericStatusTag != value)
                {
                    cycleTimeNumericStatusTag = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusTag));
                }
            }
        }
        #endregion
    }
}