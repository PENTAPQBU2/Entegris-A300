using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.TrackingStatus.Module;
using HMI_Master_KX7500.Model.TrackingStatus.ReadOnly;
using HMI_Master_KX7500.Model.TrackingStatus.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.TrackingStatus
{
    public class TrackingStatusModel : ViewModelBase
    {
        #region PublicProperties
        private List<TrackingStatusModuleModel> trackingStatusModuleList = new List<TrackingStatusModuleModel>();
        public List<TrackingStatusModuleModel> TrackingStatusModuleList
        {
            get { return trackingStatusModuleList; }
            set
            {
                if (trackingStatusModuleList != value)
                {
                    trackingStatusModuleList = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleList));
                    RaisePropertyChanged(nameof(TrackingStatusModuleListView));
                }
            }
        }

        private ICollectionView trackingStatusModuleListView = null;
        [XmlIgnore]
        public ICollectionView TrackingStatusModuleListView
        {
            get
            {
                trackingStatusModuleListView = null;
                trackingStatusModuleListView = CollectionViewSource.GetDefaultView(TrackingStatusModuleList);
                return trackingStatusModuleListView;
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


        private List<TrackingStatusReadOnlyGroup> trackingStatusReadOnlyGroupList = new List<TrackingStatusReadOnlyGroup>();
        public List<TrackingStatusReadOnlyGroup> TrackingStatusReadOnlyGroupList
        {
            get { return trackingStatusReadOnlyGroupList; }
            set
            {
                if (trackingStatusReadOnlyGroupList != value)
                {
                    trackingStatusReadOnlyGroupList = value;
                    RaisePropertyChanged(nameof(TrackingStatusReadOnlyGroupList));
                }
            }
        }
        #endregion
    }
}
