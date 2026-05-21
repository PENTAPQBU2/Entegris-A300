using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralModel : ViewModelBase
    {
        #region PublicProperties
        private List<MotorAxisGeneralStatusModel> axisGeneralStatusList = new List<MotorAxisGeneralStatusModel>();
        public List<MotorAxisGeneralStatusModel> AxisGeneralStatusList
        {
            get { return axisGeneralStatusList; }
            set
            {
                if (axisGeneralStatusList != value)
                {
                    axisGeneralStatusList = value;
                    RaisePropertyChanged(nameof(AxisGeneralStatusList));
                    RaisePropertyChanged(nameof(AxisGeneralStatusListView));
                }
            }
        }

        private ICollectionView axisGeneralStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisGeneralStatusListView
        {
            get
            {
                axisGeneralStatusListView = null;
                axisGeneralStatusListView = CollectionViewSource.GetDefaultView(AxisGeneralStatusList);
                return axisGeneralStatusListView;
            }
        }

        private List<MotorAxisGeneralNumericModel> axisGeneralNumericList = new List<MotorAxisGeneralNumericModel>();
        public List<MotorAxisGeneralNumericModel> AxisGeneralNumericList
        {
            get { return axisGeneralNumericList; }
            set
            {
                if (axisGeneralNumericList != value)
                {
                    axisGeneralNumericList = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericList));
                    RaisePropertyChanged(nameof(AxisGeneralNumericListView));
                }
            }
        }

        private ICollectionView axisGeneralNumericListView = null;
        [XmlIgnore]
        public ICollectionView AxisGeneralNumericListView
        {
            get
            {
                axisGeneralNumericListView = null;
                axisGeneralNumericListView = CollectionViewSource.GetDefaultView(AxisGeneralNumericList);
                return axisGeneralNumericListView;
            }
        }

        private List<MotorAxisGeneralActionModel> axisGeneralActionList = new List<MotorAxisGeneralActionModel>();
        public List<MotorAxisGeneralActionModel> AxisGeneralActionList
        {
            get { return axisGeneralActionList; }
            set
            {
                if (axisGeneralActionList != value)
                {
                    axisGeneralActionList = value;
                    RaisePropertyChanged(nameof(AxisGeneralActionList));
                    RaisePropertyChanged(nameof(AxisGeneralActionListView));
                }
            }
        }

        private ICollectionView axisGeneralActionListView = null;
        [XmlIgnore]
        public ICollectionView AxisGeneralActionListView
        {
            get
            {
                axisGeneralActionListView = null;
                axisGeneralActionListView = CollectionViewSource.GetDefaultView(AxisGeneralActionList);
                return axisGeneralActionListView;
            }
        }
        #endregion
    }
}