using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionKey = string.Empty;
        public string AxisTeachingPositionKey
        {
            get { return axisTeachingPositionKey; }
            set
            {
                if (axisTeachingPositionKey != value)
                {
                    axisTeachingPositionKey = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionKey));
                }
            }
        }

        private string axisTeachingPositionName = string.Empty;
        [XmlIgnore]
        public string AxisTeachingPositionName
        {
            get { return axisTeachingPositionName; }
            set
            {
                if (axisTeachingPositionName != value)
                {
                    axisTeachingPositionName = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionName));
                }
            }
        }

        private List<MotorAxisTeachingPositionNumericStatusModel> axisTeachingPositionNumericStatusList = new List<MotorAxisTeachingPositionNumericStatusModel>();
        public List<MotorAxisTeachingPositionNumericStatusModel> AxisTeachingPositionNumericStatusList
        {
            get { return axisTeachingPositionNumericStatusList; }
            set
            {
                if (axisTeachingPositionNumericStatusList != value)
                {
                    axisTeachingPositionNumericStatusList = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusList));
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusListView));
                }
            }
        }

        private ICollectionView axisTeachingPositionNumericStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisTeachingPositionNumericStatusListView
        {
            get
            {
                axisTeachingPositionNumericStatusListView = null;
                axisTeachingPositionNumericStatusListView = CollectionViewSource.GetDefaultView(AxisTeachingPositionNumericStatusList);
                return axisTeachingPositionNumericStatusListView;
            }
        }

        private List<MotorAxisTeachingPositionNumericModel> axisTeachingPositionNumericList = new List<MotorAxisTeachingPositionNumericModel>();
        public List<MotorAxisTeachingPositionNumericModel> AxisTeachingPositionNumericList
        {
            get { return axisTeachingPositionNumericList; }
            set
            {
                if (axisTeachingPositionNumericList != value)
                {
                    axisTeachingPositionNumericList = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericList));
                }
            }
        }

        private ICollectionView axisTeachingPositionNumericListView = null;
        [XmlIgnore]
        public ICollectionView AxisTeachingPositionNumericListView
        {
            get
            {
                axisTeachingPositionNumericListView = null;
                axisTeachingPositionNumericListView = CollectionViewSource.GetDefaultView(AxisTeachingPositionNumericList);
                return axisTeachingPositionNumericListView;
            }
        }

        private List<MotorAxisTeachingPositionStatusModel> axisTeachingPositionStatusList = new List<MotorAxisTeachingPositionStatusModel>();
        public List<MotorAxisTeachingPositionStatusModel> AxisTeachingPositionStatusList
        {
            get { return axisTeachingPositionStatusList; }
            set
            {
                if (axisTeachingPositionStatusList != value)
                {
                    axisTeachingPositionStatusList = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatusList));
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatusListView));
                }
            }
        }

        private ICollectionView axisTeachingPositionStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisTeachingPositionStatusListView
        {
            get
            {
                axisTeachingPositionStatusListView = null;
                axisTeachingPositionStatusListView = CollectionViewSource.GetDefaultView(AxisTeachingPositionStatusList);
                return axisTeachingPositionStatusListView;
            }
        }

        private List<MotorAxisTeachingPositionActionModel> axisTeachingPositionActionList = new List<MotorAxisTeachingPositionActionModel>();
        public List<MotorAxisTeachingPositionActionModel> AxisTeachingPositionActionList
        {
            get { return axisTeachingPositionActionList; }
            set
            {
                if (axisTeachingPositionActionList != value)
                {
                    axisTeachingPositionActionList = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionList));
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionListView));
                }
            }
        }

        private ICollectionView axisTeachingPositionActionListView = null;
        [XmlIgnore]
        public ICollectionView AxisTeachingPositionActionListView
        {
            get
            {
                axisTeachingPositionActionListView = null;
                axisTeachingPositionActionListView = CollectionViewSource.GetDefaultView(AxisTeachingPositionActionList);
                return axisTeachingPositionActionListView;
            }
        }
        #endregion
    }
}