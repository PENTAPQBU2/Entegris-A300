using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteModel : ViewModelBase
    {
        #region PublicProperties
        private List<MotorAxisMoveAbsoluteStatusModel> axisMoveAbsoluteStatusList = new List<MotorAxisMoveAbsoluteStatusModel>();
        public List<MotorAxisMoveAbsoluteStatusModel> AxisMoveAbsoluteStatusList
        {
            get { return axisMoveAbsoluteStatusList; }
            set
            {
                if (axisMoveAbsoluteStatusList != value)
                {
                    axisMoveAbsoluteStatusList = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatusList));
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatusListView));
                }
            }
        }

        private ICollectionView axisMoveAbsoluteStatusListView = null;
        [XmlIgnore]
        public ICollectionView AxisMoveAbsoluteStatusListView
        {
            get
            {
                axisMoveAbsoluteStatusListView = null;
                axisMoveAbsoluteStatusListView = CollectionViewSource.GetDefaultView(AxisMoveAbsoluteStatusList);
                return axisMoveAbsoluteStatusListView;
            }
        }

        private List<MotorAxisMoveAbsoluteNumericModel> axisMoveAbsoluteNumericList = new List<MotorAxisMoveAbsoluteNumericModel>();
        public List<MotorAxisMoveAbsoluteNumericModel> AxisMoveAbsoluteNumericList
        {
            get { return axisMoveAbsoluteNumericList; }
            set
            {
                if (axisMoveAbsoluteNumericList != value)
                {
                    axisMoveAbsoluteNumericList = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericList));
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericListView));
                }
            }
        }

        private ICollectionView axisMoveAbsoluteNumericListView = null;
        [XmlIgnore]
        public ICollectionView AxisMoveAbsoluteNumericListView
        {
            get
            {
                axisMoveAbsoluteNumericListView = null;
                axisMoveAbsoluteNumericListView = CollectionViewSource.GetDefaultView(AxisMoveAbsoluteNumericList);
                return axisMoveAbsoluteNumericListView;
            }
        }

        private List<MotorAxisMoveAbsoluteActionModel> axisMoveAbsoluteActionList = new List<MotorAxisMoveAbsoluteActionModel>();
        public List<MotorAxisMoveAbsoluteActionModel> AxisMoveAbsoluteActionList
        {
            get { return axisMoveAbsoluteActionList; }
            set
            {
                if (axisMoveAbsoluteActionList != value)
                {
                    axisMoveAbsoluteActionList = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionList));
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionListView));
                }
            }
        }

        private ICollectionView axisMoveAbsoluteActionListView = null;
        [XmlIgnore]
        public ICollectionView AxisMoveAbsoluteActionListView
        {
            get
            {
                axisMoveAbsoluteActionListView = null;
                axisMoveAbsoluteActionListView = CollectionViewSource.GetDefaultView(AxisMoveAbsoluteActionList);
                return axisMoveAbsoluteActionListView;
            }
        }
        #endregion
    }
}