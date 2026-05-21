using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisMoveAbsoluteStatusKey = string.Empty;
        public string AxisMoveAbsoluteStatusKey
        {
            get { return axisMoveAbsoluteStatusKey; }
            set
            {
                if (axisMoveAbsoluteStatusKey != value)
                {
                    axisMoveAbsoluteStatusKey = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatusKey));
                }
            }
        }

        private string axisMoveAbsoluteStatusName = string.Empty;
        [XmlIgnore]
        public string AxisMoveAbsoluteStatusName
        {
            get { return axisMoveAbsoluteStatusName; }
            set
            {
                if (axisMoveAbsoluteStatusName != value)
                {
                    axisMoveAbsoluteStatusName = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatusName));
                }
            }
        }

        private string axisMoveAbsoluteStatus = string.Empty;
        [XmlIgnore]
        public string AxisMoveAbsoluteStatus
        {
            get { return axisMoveAbsoluteStatus; }
            set
            {
                if (axisMoveAbsoluteStatus != value)
                {
                    axisMoveAbsoluteStatus = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatus));
                }
            }
        }

        private MotorAxisMoveAbsoluteStatusTagModel axisMoveAbsoluteStatusTagModel = new MotorAxisMoveAbsoluteStatusTagModel();
        public MotorAxisMoveAbsoluteStatusTagModel AxisMoveAbsoluteStatusTagModel
        {
            get { return axisMoveAbsoluteStatusTagModel; }
            set
            {
                if (axisMoveAbsoluteStatusTagModel != value)
                {
                    axisMoveAbsoluteStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatusTagModel));
                }
            }
        }
        #endregion
    }
}