using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingModel : ViewModelBase
    {
        #region PublicProperties
        private List<MotorAxisTeachingPositionModel> axisTeachingPositionList = new List<MotorAxisTeachingPositionModel>();
        public List<MotorAxisTeachingPositionModel> AxisTeachingPositionList
        {
            get { return axisTeachingPositionList; }
            set
            {
                if (axisTeachingPositionList != value)
                {
                    axisTeachingPositionList = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionList));
                    RaisePropertyChanged(nameof(AxisTeachingPositionListView));
                }
            }
        }

        private ICollectionView axisTeachingPositionListView = null;
        [XmlIgnore]
        public ICollectionView AxisTeachingPositionListView
        {
            get
            {
                axisTeachingPositionListView = null;
                axisTeachingPositionListView = CollectionViewSource.GetDefaultView(AxisTeachingPositionList);
                return axisTeachingPositionListView;
            }
        }
        #endregion
    }
}