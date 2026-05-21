using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeTagModel : ViewModelBase
    {
        #region PublicProperties
        private string cycleTimeEnableTag = string.Empty;
        public string CycleTimeEnableTag
        {
            get { return cycleTimeEnableTag; }
            set
            {
                if (cycleTimeEnableTag != value)
                {
                    cycleTimeEnableTag = value;
                    RaisePropertyChanged(nameof(CycleTimeEnableTag));
                }
            }
        }

        private string cycleTimeNameTag = string.Empty;
        public string CycleTimeNameTag
        {
            get { return cycleTimeNameTag; }
            set
            {
                if (cycleTimeNameTag != value)
                {
                    cycleTimeNameTag = value;
                    RaisePropertyChanged(nameof(CycleTimeNameTag));
                }
            }
        }

        private string cycleTimeCaptureTag = string.Empty;
        public string CycleTimeCaptureTag
        {
            get { return cycleTimeCaptureTag; }
            set
            {
                if (cycleTimeCaptureTag != value)
                {
                    cycleTimeCaptureTag = value;
                    RaisePropertyChanged(nameof(CycleTimeCaptureTag));
                }
            }
        }

        private string cycleTimeStartTag = string.Empty;
        public string CycleTimeStartTag
        {
            get { return cycleTimeStartTag; }
            set
            {
                if (cycleTimeStartTag != value)
                {
                    cycleTimeStartTag = value;
                    RaisePropertyChanged(nameof(CycleTimeStartTag));
                }
            }
        }
        #endregion
    }
}