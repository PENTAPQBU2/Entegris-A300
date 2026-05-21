using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisHomingStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisHomingStatusKey = string.Empty;
        public string AxisHomingStatusKey
        {
            get { return axisHomingStatusKey; }
            set
            {
                if (axisHomingStatusKey != value)
                {
                    axisHomingStatusKey = value;
                    RaisePropertyChanged(nameof(AxisHomingStatusKey));
                }
            }
        }

        private string axisHomingStatusName = string.Empty;
        [XmlIgnore]
        public string AxisHomingStatusName
        {
            get { return axisHomingStatusName; }
            set
            {
                if (axisHomingStatusName != value)
                {
                    axisHomingStatusName = value;
                    RaisePropertyChanged(nameof(AxisHomingStatusName));
                }
            }
        }

        private string axisHomingStatus = string.Empty;
        [XmlIgnore]
        public string AxisHomingStatus
        {
            get { return axisHomingStatus; }
            set
            {
                if (axisHomingStatus != value)
                {
                    axisHomingStatus = value;
                    RaisePropertyChanged(nameof(AxisHomingStatus));
                }
            }
        }

        private MotorAxisHomingStatusTagModel axisHomingStatusTagModel = new MotorAxisHomingStatusTagModel();
        public MotorAxisHomingStatusTagModel AxisHomingStatusTagModel
        {
            get { return axisHomingStatusTagModel; }
            set
            {
                if (axisHomingStatusTagModel != value)
                {
                    axisHomingStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisHomingStatusTagModel));
                }
            }
        }
        #endregion
    }
}