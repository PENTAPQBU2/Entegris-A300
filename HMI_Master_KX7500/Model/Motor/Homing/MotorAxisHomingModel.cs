using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisHomingModel : ViewModelBase
    {
        #region PublicProperties
        private List<MotorAxisHomingStatusModel> axisHomingStatusList = new List<MotorAxisHomingStatusModel>();
        public List<MotorAxisHomingStatusModel> AxisHomingStatusList
        {
            get { return axisHomingStatusList; }
            set
            {
                if (axisHomingStatusList != value)
                {
                    axisHomingStatusList = value;
                    RaisePropertyChanged(nameof(AxisHomingStatusList));
                    RaisePropertyChanged(nameof(AxisHomingStatusListView));
                }
            }
        }

        private ICollectionView axisHomingStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisHomingStatusListView
        {
            get
            {
                axisHomingStatusListView = null;
                axisHomingStatusListView = CollectionViewSource.GetDefaultView(AxisHomingStatusList);
                return axisHomingStatusListView;
            }
        }

        private List<MotorAxisHomingActionModel> axisHomingActionList = new List<MotorAxisHomingActionModel>();
        public List<MotorAxisHomingActionModel> AxisHomingActionList
        {
            get { return axisHomingActionList; }
            set
            {
                if (axisHomingActionList != value)
                {
                    axisHomingActionList = value;
                    RaisePropertyChanged(nameof(AxisHomingActionList));
                    RaisePropertyChanged(nameof(AxisHomingActionListView));
                }
            }
        }

        private ICollectionView axisHomingActionListView = null;
        [XmlIgnore]
        public ICollectionView AxisHomingActionListView
        {
            get
            {
                axisHomingActionListView = null;
                axisHomingActionListView = CollectionViewSource.GetDefaultView(AxisHomingActionList);
                return axisHomingActionListView;
            }
        }
        #endregion
    }
}