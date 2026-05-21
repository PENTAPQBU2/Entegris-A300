using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorIOInputModel : ViewModelBase
    {
        private string motorIOInputKey = string.Empty;
        public string MotorIOInputKey
        {
            get { return motorIOInputKey; }
            set
            {
                if (motorIOInputKey != value)
                {
                    motorIOInputKey = value;
                    RaisePropertyChanged(nameof(MotorIOInputKey));
                }
            }
        }

        private string motorIOInputName = string.Empty;
        [XmlIgnore]
        public string MotorIOInputName
        {
            get { return motorIOInputName; }
            set
            {
                if (motorIOInputName != value)
                {
                    motorIOInputName = value;
                    RaisePropertyChanged(nameof(MotorIOInputName));
                }
            }
        }

        private string motorIOInputValue = string.Empty;
        [XmlIgnore]
        public string MotorIOInputValue
        {
            get { return motorIOInputValue; }
            set
            {
                if (motorIOInputValue != value)
                {
                    motorIOInputValue = value;
                    RaisePropertyChanged(nameof(MotorIOInputValue));
                }
            }
        }

        private MotorIOInputTagModel motorIOInputTagModel = new MotorIOInputTagModel();
        public MotorIOInputTagModel MotorIOInputTagModel
        {
            get { return motorIOInputTagModel; }
            set
            {
                if (motorIOInputTagModel != value)
                {
                    motorIOInputTagModel = value;
                    RaisePropertyChanged(nameof(MotorIOInputTagModel));
                }
            }
        }

    }
}
