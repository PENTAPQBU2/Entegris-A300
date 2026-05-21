using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.TrackingStatus.Module.Indicator
{
    public class TrackingStatusModuleStatusIndicatorTagModel : ViewModelBase
    {
        #region PublicProperties
        private string trackingStatusModuleStatusIndicatorTag = string.Empty;
        public string TrackingStatusModuleStatusIndicatorTag
        {
            get { return trackingStatusModuleStatusIndicatorTag; }
            set
            {
                if (trackingStatusModuleStatusIndicatorTag != value)
                {
                    trackingStatusModuleStatusIndicatorTag = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorTag));
                }
            }
        }

        private string trackingStatusModuleStatusIndicatorTriggerTag = string.Empty;
        public string TrackingStatusModuleStatusIndicatorTriggerTag
        {
            get { return trackingStatusModuleStatusIndicatorTriggerTag; }
            set
            {
                if (trackingStatusModuleStatusIndicatorTriggerTag != value)
                {
                    trackingStatusModuleStatusIndicatorTriggerTag = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorTriggerTag));
                }
            }
        }
        #endregion
    }
}
