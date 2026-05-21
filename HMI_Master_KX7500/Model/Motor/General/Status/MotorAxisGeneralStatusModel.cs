using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisGeneralStatusKey = string.Empty;
        public string AxisGeneralStatusKey
        {
            get { return axisGeneralStatusKey; }
            set
            {
                if (axisGeneralStatusKey != value)
                {
                    axisGeneralStatusKey = value;
                    RaisePropertyChanged(nameof(AxisGeneralStatusKey));
                }
            }
        }

        private string axisGeneralStatusName = string.Empty;
        [XmlIgnore]
        public string AxisGeneralStatusName
        {
            get { return axisGeneralStatusName; }
            set
            {
                if (axisGeneralStatusName != value)
                {
                    axisGeneralStatusName = value;
                    RaisePropertyChanged(nameof(AxisGeneralStatusName));
                }
            }
        }

        private string axisGeneralStatus = string.Empty;
        [XmlIgnore]
        public string AxisGeneralStatus
        {
            get { return axisGeneralStatus; }
            set
            {
                if (axisGeneralStatus != value)
                {
                    axisGeneralStatus = value;
                    RaisePropertyChanged(nameof(AxisGeneralStatus));
                }
            }
        }

        private MotorAxisGeneralStatusTagModel axisGeneralStatusTagModel = new MotorAxisGeneralStatusTagModel();
        public MotorAxisGeneralStatusTagModel AxisGeneralStatusTagModel
        {
            get { return axisGeneralStatusTagModel; }
            set
            {
                if (axisGeneralStatusTagModel != value)
                {
                    axisGeneralStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisGeneralStatusTagModel));
                }
            }
        }
        #endregion
    }
}