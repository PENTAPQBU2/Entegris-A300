using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorIOOutputTagModel : ViewModelBase
    {
        private string motorIOOutputTag = string.Empty;
        public string MotorIOOutputTag
        {
            get { return motorIOOutputTag; }
            set
            {
                if (motorIOOutputTag != value)
                {
                    motorIOOutputTag = value;
                    RaisePropertyChanged(nameof(MotorIOOutputTag));
                }
            }
        }

        private string motorIOOutputViewTag = string.Empty;
        public string MotorIOOutputViewTag
        {
            get { return motorIOOutputViewTag; }
            set
            {
                if (motorIOOutputViewTag != value)
                {
                    motorIOOutputViewTag = value;
                    RaisePropertyChanged(nameof(MotorIOOutputViewTag));
                }
            }
        }
    }
}
