using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisPositionModel : ViewModelBase
    {
        #region PublicProperties
        private List<MotorAxisPositionStatusModel> axisPositionStatusList = new List<MotorAxisPositionStatusModel>();
        public List<MotorAxisPositionStatusModel> AxisPositionStatusList
        {
            get { return axisPositionStatusList; }
            set
            {
                if (axisPositionStatusList != value)
                {
                    axisPositionStatusList = value;
                    RaisePropertyChanged(nameof(AxisPositionStatusList));
                    RaisePropertyChanged(nameof(AxisPositionStatusListView));
                }
            }
        }

        private ICollectionView axisPositionStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisPositionStatusListView
        {
            get
            {
                axisPositionStatusListView = null;
                axisPositionStatusListView = CollectionViewSource.GetDefaultView(AxisPositionStatusList);
                return axisPositionStatusListView;
            }
        }

        private List<MotorAxisPositionNumericStatusModel> axisPositionNumericStatusList = new List<MotorAxisPositionNumericStatusModel>();
        public List<MotorAxisPositionNumericStatusModel> AxisPositionNumericStatusList
        {
            get { return axisPositionNumericStatusList; }
            set
            {
                if (axisPositionNumericStatusList != value)
                {
                    axisPositionNumericStatusList = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusList));
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusListView));
                }
            }
        }

        private ICollectionView axisPositionNumericStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisPositionNumericStatusListView
        {
            get
            {
                axisPositionNumericStatusListView = null;
                axisPositionNumericStatusListView = CollectionViewSource.GetDefaultView(AxisPositionNumericStatusList);
                return axisPositionNumericStatusListView;
            }
        }
        #endregion
    }
}