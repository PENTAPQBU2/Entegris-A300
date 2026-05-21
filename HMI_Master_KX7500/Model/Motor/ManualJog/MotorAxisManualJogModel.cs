using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand AxisManualJogOffCommand { get; set; }

        [XmlIgnore]
        public ICommand AxisManualJogOnCommand { get; set; }

        [XmlIgnore]
        public ICommand SetAxisManualJogIntervalCommand { get; set; }

        //[XmlIgnore]
        //public ICommand AxisManualJogStatusCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool axisManualJog = false;
        [XmlIgnore]
        public bool AxisManualJog
        {
            get { return axisManualJog; }
            set
            {
                if (axisManualJog != value)
                {
                    axisManualJog = value;
                    RaisePropertyChanged(nameof(AxisManualJog));
                }
            }
        }

        private MotorAxisManualJogTagModel axisManualJogTagModel = new MotorAxisManualJogTagModel();
        public MotorAxisManualJogTagModel AxisManualJogTagModel
        {
            get { return axisManualJogTagModel; }
            set
            {
                if (axisManualJogTagModel != value)
                {
                    axisManualJogTagModel = value;
                    RaisePropertyChanged(nameof(AxisManualJogTagModel));
                }
            }
        }

        private int axisManualJogIntervalSelectedIndex = -1;
        [XmlIgnore]
        public int AxisManualJogIntervalSelectedIndex
        {
            get { return axisManualJogIntervalSelectedIndex; }
            set
            {
                if (axisManualJogIntervalSelectedIndex != value)
                {
                    axisManualJogIntervalSelectedIndex = value;
                    RaisePropertyChanged(nameof(AxisManualJogIntervalSelectedIndex));
                }
            }
        }

        private List<MotorAxisManualJogIntervalModel> axisManualJogIntervalList = new List<MotorAxisManualJogIntervalModel>();
        public List<MotorAxisManualJogIntervalModel> AxisManualJogIntervalList
        {
            get { return axisManualJogIntervalList; }
            set
            {
                if (axisManualJogIntervalList != value)
                {
                    axisManualJogIntervalList = value;
                    RaisePropertyChanged(nameof(AxisManualJogIntervalList));
                    RaisePropertyChanged(nameof(AxisManualJogIntervalListView));
                }
            }
        }

        private ICollectionView axisManualJogIntervalListView = null;
        [XmlIgnore]
        public ICollectionView AxisManualJogIntervalListView
        {
            get
            {
                axisManualJogIntervalListView = null;
                axisManualJogIntervalListView = CollectionViewSource.GetDefaultView(AxisManualJogIntervalList);
                return axisManualJogIntervalListView;
            }
        }

        private List<MotorAxisManualJogStatusModel> axisManualJogStatusList = new List<MotorAxisManualJogStatusModel>();
        public List<MotorAxisManualJogStatusModel> AxisManualJogStatusList
        {
            get { return axisManualJogStatusList; }
            set
            {
                if (axisManualJogStatusList != value)
                {
                    axisManualJogStatusList = value;
                    RaisePropertyChanged(nameof(AxisManualJogStatusList));
                    RaisePropertyChanged(nameof(AxisManualJogStatusListView));
                }
            }
        }

        private ICollectionView axisManualJogStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisManualJogStatusListView
        {
            get
            {
                axisManualJogStatusListView = null;
                axisManualJogStatusListView = CollectionViewSource.GetDefaultView(AxisManualJogStatusList);
                return axisManualJogStatusListView;
            }
        }

        private List<MotorAxisManualJogNumericModel> axisManualJogNumericList = new List<MotorAxisManualJogNumericModel>();
        public List<MotorAxisManualJogNumericModel> AxisManualJogNumericList
        {
            get { return axisManualJogNumericList; }
            set
            {
                if (axisManualJogNumericList != value)
                {
                    axisManualJogNumericList = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumericList));
                    RaisePropertyChanged(nameof(AxisManualJogNumericListView));
                }
            }
        }

        private ICollectionView axisManualJogNumericListView = null;
        [XmlIgnore]
        public ICollectionView AxisManualJogNumericListView
        {
            get
            {
                axisManualJogNumericListView = null;
                axisManualJogNumericListView = CollectionViewSource.GetDefaultView(AxisManualJogNumericList);
                return axisManualJogNumericListView;
            }
        }
        #endregion
    }
}