using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeModel : ViewModelBase
    {
        #region PublicProperties
        private bool cycleTimeEnable = false;
        [XmlIgnore]
        public bool CycleTimeEnable
        {
            get { return cycleTimeEnable; }
            set
            {
                if (cycleTimeEnable != value)
                {
                    cycleTimeEnable = value;
                    RaisePropertyChanged(nameof(CycleTimeEnable));
                    Messenger.Default.Send(CycleTimeUpdate.Enable);
                }
            }
        }

        private string cycleTimeKey = string.Empty;
        [XmlIgnore]
        public string CycleTimeKey
        {
            get { return cycleTimeKey; }
            set
            {
                if (cycleTimeKey != value)
                {
                    cycleTimeKey = value;
                    RaisePropertyChanged(nameof(CycleTimeKey));
                    Messenger.Default.Send(CycleTimeUpdate.Name);
                }
            }
        }

        private string cycleTimeName = string.Empty;
        [XmlIgnore]
        public string CycleTimeName
        {
            get { return cycleTimeName; }
            set
            {
                if (cycleTimeName != value)
                {
                    cycleTimeName = value;
                    RaisePropertyChanged(nameof(CycleTimeName));
                }
            }
        }

        private CycleTimeTagModel cycleTimeTagModel = new CycleTimeTagModel();
        public CycleTimeTagModel CycleTimeTagModel
        {
            get { return cycleTimeTagModel; }
            set
            {
                if (cycleTimeTagModel != value)
                {
                    cycleTimeTagModel = value;
                    RaisePropertyChanged(nameof(CycleTimeTagModel));
                }
            }
        }

        private List<CycleTimeNumericStatusModel> cycleTimeNumericStatusList = new List<CycleTimeNumericStatusModel>();
        public List<CycleTimeNumericStatusModel> CycleTimeNumericStatusList
        {
            get { return cycleTimeNumericStatusList; }
            set
            {
                if (cycleTimeNumericStatusList != value)
                {
                    cycleTimeNumericStatusList = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusList));
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusListView));
                }
            }
        }

        private ICollectionView cycleTimeNumericStatusListView = null;
        [XmlIgnore]
        public ICollectionView CycleTimeNumericStatusListView
        {
            get
            {
                cycleTimeNumericStatusListView = null;
                cycleTimeNumericStatusListView = CollectionViewSource.GetDefaultView(CycleTimeNumericStatusList);
                return cycleTimeNumericStatusListView;
            }
        }

        private CycleTimeHistoryModel cycleTimeHistoryModel = new CycleTimeHistoryModel();
        public CycleTimeHistoryModel CycleTimeHistoryModel
        {
            get { return cycleTimeHistoryModel; }
            set
            {
                if (cycleTimeHistoryModel != value)
                {
                    cycleTimeHistoryModel = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryModel));
                }
            }
        }
        #endregion
    }
}