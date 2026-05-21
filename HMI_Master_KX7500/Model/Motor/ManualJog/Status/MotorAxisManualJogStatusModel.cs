using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisManualJogStatusKey = string.Empty;
        public string AxisManualJogStatusKey
        {
            get { return axisManualJogStatusKey; }
            set
            {
                if (axisManualJogStatusKey != value)
                {
                    axisManualJogStatusKey = value;
                    RaisePropertyChanged(nameof(AxisManualJogStatusKey));
                }
            }
        }

        private string axisManualJogStatusName = string.Empty;
        [XmlIgnore]
        public string AxisManualJogStatusName
        {
            get { return axisManualJogStatusName; }
            set
            {
                if (axisManualJogStatusName != value)
                {
                    axisManualJogStatusName = value;
                    RaisePropertyChanged(nameof(AxisManualJogStatusName));
                }
            }
        }

        private string axisManualJogStatus = string.Empty;
        [XmlIgnore]
        public string AxisManualJogStatus
        {
            get { return axisManualJogStatus; }
            set
            {
                if (axisManualJogStatus != value)
                {
                    axisManualJogStatus = value;
                    RaisePropertyChanged(nameof(AxisManualJogStatus));
                }
            }
        }

        private MotorAxisManualJogStatusTagModel axisManualJogStatusTagModel = new MotorAxisManualJogStatusTagModel();
        public MotorAxisManualJogStatusTagModel AxisManualJogStatusTagModel
        {
            get { return axisManualJogStatusTagModel; }
            set
            {
                if (axisManualJogStatusTagModel != value)
                {
                    axisManualJogStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisManualJogStatusTagModel));
                }
            }
        }
        #endregion
    }
}