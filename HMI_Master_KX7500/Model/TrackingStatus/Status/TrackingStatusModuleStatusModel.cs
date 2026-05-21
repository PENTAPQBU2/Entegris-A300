using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.TrackingStatus.Status
{
    public class TrackingStatusModuleStatusModel : ViewModelBase
    {
        private string trackingStatusModuleKey = string.Empty;
        public string TrackingStatusModuleKey
        {
            get { return trackingStatusModuleKey; }
            set
            {
                if (trackingStatusModuleKey != value)
                {
                    trackingStatusModuleKey = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleKey));
                }
            }
        }
        #region PublicProperties
        private string trackingStatusModuleStatusKey = string.Empty;
        public string TrackingStatusModuleStatusKey
        {
            get { return trackingStatusModuleStatusKey; }
            set
            {
                if (trackingStatusModuleStatusKey != value)
                {
                    trackingStatusModuleStatusKey = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusKey));
                }
            }
        }

        private string trackingStatusModuleStatusName = string.Empty;
        [XmlIgnore]
        public string TrackingStatusModuleStatusName
        {
            get { return trackingStatusModuleStatusName; }
            set
            {
                if (trackingStatusModuleStatusName != value)
                {
                    trackingStatusModuleStatusName = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusName));
                }
            }
        }

        private string trackingStatusModuleStatusIndex = "0";
        public string TrackingStatusModuleStatusIndex
        {
            get { return trackingStatusModuleStatusIndex; }
            set
            {
                if (trackingStatusModuleStatusIndex != value)
                {
                    trackingStatusModuleStatusIndex = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndex));
                }
            }
        }

        private string trackingStatusModuleStatusColor = string.Empty;
        public string TrackingStatusModuleStatusColor
        {
            get { return trackingStatusModuleStatusColor; }
            set
            {
                if (trackingStatusModuleStatusColor != value)
                {
                    trackingStatusModuleStatusColor = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusColor));
                }
            }
        }
        #endregion
    }
}
