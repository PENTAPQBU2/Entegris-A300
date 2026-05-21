using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupToggleTagModel : ViewModelBase
    {
        private string engineeringGroupToggleTag = string.Empty;
        public string EngineeringGroupToggleTag
        {
            get { return engineeringGroupToggleTag; }
            set
            {
                if(engineeringGroupToggleTag != value)
                {
                    engineeringGroupToggleTag = value;
                    RaisePropertyChanged(nameof(EngineeringGroupToggleTag));
                }
            }
        }
    }
}
