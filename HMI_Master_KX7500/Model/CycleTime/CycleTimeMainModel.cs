using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeMainModel : ViewModelBase
    {
        #region PublicProperties
        private List<CycleTimeModel> cycleTimeList = new List<CycleTimeModel>();
        public List<CycleTimeModel> CycleTimeList
        {
            get { return cycleTimeList; }
            set
            {
                if (cycleTimeList != value)
                {
                    cycleTimeList = value;
                    RaisePropertyChanged(nameof(CycleTimeList));
                    RaisePropertyChanged(nameof(CycleTimeListView));
                }
            }
        }

        private ICollectionView cycleTimeListView = null;
        [XmlIgnore]
        public ICollectionView CycleTimeListView
        {
            get
            {
                cycleTimeListView = null;
                cycleTimeListView = CollectionViewSource.GetDefaultView(CycleTimeList);
                return cycleTimeListView;
            }
        }
        #endregion
    }
}