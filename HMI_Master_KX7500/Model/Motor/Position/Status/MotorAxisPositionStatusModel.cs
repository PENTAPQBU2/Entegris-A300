using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisPositionStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisPositionStatusKey = string.Empty;
        public string AxisPositionStatusKey
        {
            get { return axisPositionStatusKey; }
            set
            {
                if (axisPositionStatusKey != value)
                {
                    axisPositionStatusKey = value;
                    RaisePropertyChanged(nameof(AxisPositionStatusKey));
                }
            }
        }

        private string axisPositionStatusName = string.Empty;
        [XmlIgnore]
        public string AxisPositionStatusName
        {
            get { return axisPositionStatusName; }
            set
            {
                if (axisPositionStatusName != value)
                {
                    axisPositionStatusName = value;
                    RaisePropertyChanged(nameof(AxisPositionStatusName));
                }
            }
        }

        private string axisPositionStatus = string.Empty;
        [XmlIgnore]
        public string AxisPositionStatus
        {
            get { return axisPositionStatus; }
            set
            {
                if (axisPositionStatus != value)
                {
                    axisPositionStatus = value;
                    RaisePropertyChanged(nameof(AxisPositionStatus));
                }
            }
        }

        private MotorAxisPositionStatusTagModel axisPositionStatusTagModel = new MotorAxisPositionStatusTagModel();
        public MotorAxisPositionStatusTagModel AxisPositionStatusTagModel
        {
            get { return axisPositionStatusTagModel; }
            set
            {
                if (axisPositionStatusTagModel != value)
                {
                    axisPositionStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisPositionStatusTagModel));
                }
            }
        }
        #endregion
    }
}