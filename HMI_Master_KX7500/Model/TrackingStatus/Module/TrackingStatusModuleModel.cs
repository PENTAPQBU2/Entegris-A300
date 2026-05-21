using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.TrackingStatus.Module.Indicator;
using HMI_Master_KX7500.Model.TrackingStatus.Status;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.TrackingStatus.Module
{
    public class TrackingStatusModuleModel : ViewModelBase
    {
        #region PublicProperties
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

        private string trackingStatusModuleName = string.Empty;
        [XmlIgnore]
        public string TrackingStatusModuleName
        {
            get { return trackingStatusModuleName; }
            set
            {
                if (trackingStatusModuleName != value)
                {
                    trackingStatusModuleName = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleName));
                }
            }
        }

        private string trackingStatusModuleImage = string.Empty;
        public string TrackingStatusModuleImage
        {
            get { return trackingStatusModuleImage; }
            set
            {
                if (trackingStatusModuleImage != value)
                {
                    trackingStatusModuleImage = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleImage));
                }
            }
        }

        private List<TrackingStatusModuleStatusIndicatorModel> trackingStatusModuleStatusIndicatorList = new List<TrackingStatusModuleStatusIndicatorModel>();
        public List<TrackingStatusModuleStatusIndicatorModel> TrackingStatusModuleStatusIndicatorList
        {
            get { return trackingStatusModuleStatusIndicatorList; }
            set
            {
                if (trackingStatusModuleStatusIndicatorList != value)
                {
                    trackingStatusModuleStatusIndicatorList = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorList));
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorListView));
                }
            }
        }

        private ICollectionView trackingStatusModuleStatusIndicatorListView = null;
        [XmlIgnore]
        public ICollectionView TrackingStatusModuleStatusIndicatorListView
        {
            get
            {
                trackingStatusModuleStatusIndicatorListView = null;
                trackingStatusModuleStatusIndicatorListView = CollectionViewSource.GetDefaultView(TrackingStatusModuleStatusIndicatorList);
                return trackingStatusModuleStatusIndicatorListView;
            }
        }

        private List<TrackingStatusModuleStatusModel> trackingStatusModuleStatusList = new List<TrackingStatusModuleStatusModel>();
        public List<TrackingStatusModuleStatusModel> TrackingStatusModuleStatusList
        {
            get { return trackingStatusModuleStatusList; }
            set
            {
                if (trackingStatusModuleStatusList != value)
                {
                    trackingStatusModuleStatusList = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusList));
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusListView));
                }
            }
        }
        private ICollectionView trackingStatusModuleStatusListView = null;
        [XmlIgnore]
        public ICollectionView TrackingStatusModuleStatusListView
        {
            get
            {
                trackingStatusModuleStatusListView = null;
                trackingStatusModuleStatusListView = CollectionViewSource.GetDefaultView(TrackingStatusModuleStatusList);
                return trackingStatusModuleStatusListView;
            }
        }
        #endregion
    }
}
