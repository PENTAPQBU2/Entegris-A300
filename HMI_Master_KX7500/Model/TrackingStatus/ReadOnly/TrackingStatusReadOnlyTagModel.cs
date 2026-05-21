using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.TrackingStatus.ReadOnly
{
    public class TrackingStatusReadOnlyTagModel : ViewModelBase
    {
        private string trackingStatusReadOnlyTag = string.Empty;
        public string TrackingStatusReadOnlyTag
        {
            get { return trackingStatusReadOnlyTag; }
            set
            {
                if (trackingStatusReadOnlyTag != value)
                {
                    trackingStatusReadOnlyTag = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyTag));
                }
            }
        }
    }
}
