using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.TrackingStatus.ReadOnly
{
    public class TrackingStatusReadOnlyGroup : ViewModelBase
    {
        private List<TrackingStatusReadOnlyModel> trackingStatusReadOnlyList = new List<TrackingStatusReadOnlyModel>();
        public List<TrackingStatusReadOnlyModel> TrackingStatusReadOnlyList
        {
            get { return trackingStatusReadOnlyList; }
            set
            {
                if (trackingStatusReadOnlyList != value)
                {
                    trackingStatusReadOnlyList = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyList));
                }
            }
        }

        private string trackingStatusReadOnlyGroupKey = string.Empty;
        public string TrackingStatusReadOnlyGroupKey
        {
            get { return trackingStatusReadOnlyGroupKey; }
            set
            {
                if (trackingStatusReadOnlyGroupKey != value)
                {
                    trackingStatusReadOnlyGroupKey = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyGroupKey));
                }
            }
        }

        private string trackingStatusReadOnlyGroupName = string.Empty;
        [XmlIgnore]
        public string TrackingStatusReadOnlyGroupName
        {
            get { return trackingStatusReadOnlyGroupName; }
            set
            {
                if (trackingStatusReadOnlyGroupName != value)
                {
                    trackingStatusReadOnlyGroupName = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyGroupName));
                }
            }
        }
    }
}
