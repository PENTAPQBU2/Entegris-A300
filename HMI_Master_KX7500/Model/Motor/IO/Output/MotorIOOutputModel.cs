using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorIOOutputModel : ViewModelBase
    {
        private string motorIOOutputKey = string.Empty;
        public string MotorIOOutputKey
        {
            get { return motorIOOutputKey; }
            set
            {
                if (motorIOOutputKey != value)
                {
                    motorIOOutputKey = value;
                    RaisePropertyChanged(nameof(MotorIOOutputKey));
                }
            }
        }

        private string motorIOOutputName = string.Empty;
        [XmlIgnore]
        public string MotorIOOutputName
        {
            get { return motorIOOutputName; }
            set
            {
                if (motorIOOutputName != value)
                {
                    motorIOOutputName = value;
                    RaisePropertyChanged(nameof(MotorIOOutputName));
                }
            }
        }

        private string motorIOOutputValue = string.Empty;
        [XmlIgnore]
        public string MotorIOOutputValue
        {
            get { return motorIOOutputValue; }
            set
            {
                if (motorIOOutputValue != value)
                {
                    motorIOOutputValue = value;
                    RaisePropertyChanged(nameof(MotorIOOutputValue));
                }
            }
        }

        private MotorIOOutputTagModel motorIOOutputTagModel = new MotorIOOutputTagModel();
        public MotorIOOutputTagModel MotorIOOutputTagModel
        {
            get { return motorIOOutputTagModel; }
            set
            {
                if (motorIOOutputTagModel != value)
                {
                    motorIOOutputTagModel = value;
                    RaisePropertyChanged(nameof(MotorIOOutputTagModel));
                }
            }
        }

        [XmlIgnore]
        public ICommand MotorIOOutputCommand { get; set; }
    }
}
