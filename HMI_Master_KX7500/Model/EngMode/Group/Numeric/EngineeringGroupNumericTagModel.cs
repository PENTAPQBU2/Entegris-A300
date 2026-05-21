using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupNumericTagModel : ViewModelBase
    {
        private string engineeringGroupNumericTag = string.Empty;
        public string EngineeringGroupNumericTag
        {
            get { return engineeringGroupNumericTag; }
            set
            {
                if (engineeringGroupNumericTag != value)
                {
                    engineeringGroupNumericTag = value;
                    RaisePropertyChanged(nameof(EngineeringGroupNumericTag));
                }
            }
        }
    }
}
