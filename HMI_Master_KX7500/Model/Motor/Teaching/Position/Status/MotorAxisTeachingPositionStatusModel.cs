using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionStatusKey = string.Empty;
        public string AxisTeachingPositionStatusKey
        {
            get { return axisTeachingPositionStatusKey; }
            set
            {
                if (axisTeachingPositionStatusKey != value)
                {
                    axisTeachingPositionStatusKey = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatusKey));
                }
            }
        }

        private string axisTeachingPositionStatusName = string.Empty;
        [XmlIgnore]
        public string AxisTeachingPositionStatusName
        {
            get { return axisTeachingPositionStatusName; }
            set
            {
                if (axisTeachingPositionStatusName != value)
                {
                    axisTeachingPositionStatusName = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatusName));
                }
            }
        }

        private string axisTeachingPositionStatus = string.Empty;
        [XmlIgnore]
        public string AxisTeachingPositionStatus
        {
            get { return axisTeachingPositionStatus; }
            set
            {
                if (axisTeachingPositionStatus != value)
                {
                    axisTeachingPositionStatus = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatus));
                }
            }
        }

        private MotorAxisTeachingPositionStatusTagModel axisTeachingPositionStatusTagModel = new MotorAxisTeachingPositionStatusTagModel();
        public MotorAxisTeachingPositionStatusTagModel AxisTeachingPositionStatusTagModel
        {
            get { return axisTeachingPositionStatusTagModel; }
            set
            {
                if (axisTeachingPositionStatusTagModel != value)
                {
                    axisTeachingPositionStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatusTagModel));
                }
            }
        }
        #endregion
    }
}