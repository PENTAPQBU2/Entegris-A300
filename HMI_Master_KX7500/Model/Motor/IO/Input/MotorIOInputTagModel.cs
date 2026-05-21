using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorIOInputTagModel : ViewModelBase
    {
        private string motorIOInputTag = string.Empty;
        public string MotorIOInputTag
        {
            get { return motorIOInputTag; }
            set
            {
                if (motorIOInputTag != value)
                {
                    motorIOInputTag = value;
                    RaisePropertyChanged(nameof(MotorIOInputTag));
                }
            }
        }
    }
}
