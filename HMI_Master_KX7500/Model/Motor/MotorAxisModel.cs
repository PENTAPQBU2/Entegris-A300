using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisModel : ViewModelBase
    {
        #region PublicProperties
        private string axisKey = string.Empty;
        public string AxisKey
        {
            get { return axisKey; }
            set
            {
                if (axisKey != value)
                {
                    axisKey = value;
                    RaisePropertyChanged(nameof(AxisKey));
                }
            }
        }

        private string axisName = string.Empty;
        [XmlIgnore]
        public string AxisName
        {
            get { return axisName; }
            set
            {
                if (axisName != value)
                {
                    axisName = value;
                    RaisePropertyChanged(nameof(AxisName));
                }
            }
        }

        private MotorAxisGeneralModel axisGeneralModel = new MotorAxisGeneralModel();
        public MotorAxisGeneralModel AxisGeneralModel
        {
            get { return axisGeneralModel; }
            set
            {
                if (axisGeneralModel != value)
                {
                    axisGeneralModel = value;
                    RaisePropertyChanged(nameof(AxisGeneralModel));
                }
            }
        }

        private MotorAxisHomingModel axisHomingModel = new MotorAxisHomingModel();
        public MotorAxisHomingModel AxisHomingModel
        {
            get { return axisHomingModel; }
            set
            {
                if (axisHomingModel != value)
                {
                    axisHomingModel = value;
                    RaisePropertyChanged(nameof(AxisHomingModel));
                }
            }
        }

        private MotorAxisPositionModel axisPositionModel = new MotorAxisPositionModel();
        public MotorAxisPositionModel AxisPositionModel
        {
            get { return axisPositionModel; }
            set
            {
                if (axisPositionModel != value)
                {
                    axisPositionModel = value;
                    RaisePropertyChanged(nameof(AxisPositionModel));
                }
            }
        }

        private MotorAxisManualJogModel axisManualJogModel = new MotorAxisManualJogModel();
        public MotorAxisManualJogModel AxisManualJogModel
        {
            get { return axisManualJogModel; }
            set
            {
                if (axisManualJogModel != value)
                {
                    axisManualJogModel = value;
                    RaisePropertyChanged(nameof(AxisManualJogModel));
                }
            }
        }

        private MotorAxisMoveAbsoluteModel axisMoveAbsoluteModel = new MotorAxisMoveAbsoluteModel();
        public MotorAxisMoveAbsoluteModel AxisMoveAbsoluteModel
        {
            get { return axisMoveAbsoluteModel; }
            set
            {
                if (axisMoveAbsoluteModel != value)
                {
                    axisMoveAbsoluteModel = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteModel));
                }
            }
        }

        private MotorAxisTeachingModel axisTeachingModel = new MotorAxisTeachingModel();
        public MotorAxisTeachingModel AxisTeachingModel
        {
            get { return axisTeachingModel; }
            set
            {
                if (axisTeachingModel != value)
                {
                    axisTeachingModel = value;
                    RaisePropertyChanged(nameof(AxisTeachingModel));
                }
            }
        }

        private MotorIOModel motorIOModel = new MotorIOModel();
        public MotorIOModel MotorIOModel
        {
            get { return motorIOModel; }
            set
            {
                if (motorIOModel != value)
                {
                    motorIOModel = value;
                    RaisePropertyChanged(nameof(MotorIOModel));
                }
            }
        }
        #endregion
    }
}