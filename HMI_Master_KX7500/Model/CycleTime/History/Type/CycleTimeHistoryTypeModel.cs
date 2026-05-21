using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeHistoryTypeModel : ViewModelBase
    {
        #region PublicProperties
        private string cycleTimeHistoryTypeKey = string.Empty;
        public string CycleTimeHistoryTypeKey
        {
            get { return cycleTimeHistoryTypeKey; }
            set
            {
                if (cycleTimeHistoryTypeKey != value)
                {
                    cycleTimeHistoryTypeKey = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeKey));
                }
            }
        }

        private string cycleTimeHistoryTypeName = string.Empty;
        [XmlIgnore]
        public string CycleTimeHistoryTypeName
        {
            get { return cycleTimeHistoryTypeName; }
            set
            {
                if (cycleTimeHistoryTypeName != value)
                {
                    cycleTimeHistoryTypeName = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeName));
                }
            }
        }

        private List<CycleTimeHistoryTypeTimerModel> cycleTimeHistoryTypeTimerList = new List<CycleTimeHistoryTypeTimerModel>();
        public List<CycleTimeHistoryTypeTimerModel> CycleTimeHistoryTypeTimerList
        {
            get { return cycleTimeHistoryTypeTimerList; }
            set
            {
                if (cycleTimeHistoryTypeTimerList != value)
                {
                    cycleTimeHistoryTypeTimerList = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeTimerList));
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeTimerListView));
                }
            }
        }

        private ICollectionView cycleTimeHistoryTypeTimerListView = null;
        [XmlIgnore]
        public ICollectionView CycleTimeHistoryTypeTimerListView
        {
            get
            {
                cycleTimeHistoryTypeTimerListView = null;
                cycleTimeHistoryTypeTimerListView = CollectionViewSource.GetDefaultView(CycleTimeHistoryTypeTimerList);
                return cycleTimeHistoryTypeTimerListView;
            }
        }
        #endregion
    }
}