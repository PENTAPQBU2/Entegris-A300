using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.TrackingStatus.ReadOnly
{
    public class TrackingStatusReadOnlyModel : ViewModelBase
    {
        private string trackingStatusReadOnlyKey = string.Empty;
        public string TrackingStatusReadOnlyKey
        {
            get { return trackingStatusReadOnlyKey; }
            set
            {
                if (trackingStatusReadOnlyKey != value)
                {
                    trackingStatusReadOnlyKey = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyKey));
                }
            }
        }

        private string trackingStatusReadOnlyName = string.Empty;
        [XmlIgnore]
        public string TrackingStatusReadOnlyName
        {
            get { return trackingStatusReadOnlyName; }
            set
            {
                if (trackingStatusReadOnlyName != value)
                {
                    trackingStatusReadOnlyName = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyName));
                }
            }
        }

        private string trackingStatusReadOnlyValue = string.Empty;
        [XmlIgnore]
        public string TrackingStatusReadOnlyValue
        {
            get { return trackingStatusReadOnlyValue; }
            set
            {
                if (trackingStatusReadOnlyValue != value)
                {
                    trackingStatusReadOnlyValue = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyValue));
                }
            }
        }

        private TrackingStatusReadOnlyTagModel trackingStatusReadOnlyTagModel = new TrackingStatusReadOnlyTagModel();
        public TrackingStatusReadOnlyTagModel TrackingStatusReadOnlyTagModel
        {
            get { return trackingStatusReadOnlyTagModel; }
            set
            {
                if (trackingStatusReadOnlyTagModel != value)
                {
                    trackingStatusReadOnlyTagModel = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyTagModel));
                }
            }
        }
    }
}
