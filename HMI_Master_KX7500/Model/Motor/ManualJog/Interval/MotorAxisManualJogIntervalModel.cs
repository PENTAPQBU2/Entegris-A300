using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogIntervalModel : ViewModelBase
    {
        #region PublicProperties
        private string axisManualJogIntervalKey = string.Empty;
        public string AxisManualJogIntervalKey
        {
            get { return axisManualJogIntervalKey; }
            set
            {
                if (axisManualJogIntervalKey != value)
                {
                    axisManualJogIntervalKey = value;
                    RaisePropertyChanged(nameof(AxisManualJogIntervalKey));
                }
            }
        }

        private string axisManualJogIntervalName = string.Empty;
        [XmlIgnore]
        public string AxisManualJogIntervalName
        {
            get { return axisManualJogIntervalName; }
            set
            {
                if (axisManualJogIntervalName != value)
                {
                    axisManualJogIntervalName = value;
                    RaisePropertyChanged(nameof(AxisManualJogIntervalName));
                }
            }
        }

        private bool axisManualJogInterval = false;
        [XmlIgnore]
        public bool AxisManualJogInterval
        {
            get { return axisManualJogInterval; }
            set
            {
                if (axisManualJogInterval != value)
                {
                    axisManualJogInterval = value;
                    RaisePropertyChanged(nameof(AxisManualJogInterval));
                }
            }
        }

        private MotorAxisManualJogIntervalTagModel axisManualJogIntervalTagModel = new MotorAxisManualJogIntervalTagModel();
        public MotorAxisManualJogIntervalTagModel AxisManualJogIntervalTagModel
        {
            get { return axisManualJogIntervalTagModel; }
            set
            {
                if (axisManualJogIntervalTagModel != value)
                {
                    axisManualJogIntervalTagModel = value;
                    RaisePropertyChanged(nameof(AxisManualJogIntervalTagModel));
                }
            }
        }
        #endregion
    }
}