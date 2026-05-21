using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupTextTagModel : ViewModelBase
    {
        private string engineeringGroupTextTag = string.Empty;
        public string EngineeringGroupTextTag
        {
            get { return engineeringGroupTextTag; }
            set
            {
                if(engineeringGroupTextTag != value)
                {
                    engineeringGroupTextTag = value;
                    RaisePropertyChanged(nameof(EngineeringGroupTextTag));
                }
            }
        }
    }
}
