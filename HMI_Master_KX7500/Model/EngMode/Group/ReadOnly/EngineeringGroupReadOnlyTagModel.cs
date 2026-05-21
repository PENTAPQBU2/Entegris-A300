using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupReadOnlyTagModel : ViewModelBase
    {
        private string engineeringGroupReadOnlyTag = string.Empty;
        public string EngineeringGroupReadOnlyTag
        {
            get { return engineeringGroupReadOnlyTag; }
            set
            {
                if (engineeringGroupReadOnlyTag != value)
                {
                    engineeringGroupReadOnlyTag = value;
                    RaisePropertyChanged(nameof(EngineeringGroupReadOnlyTag));
                }
            }
        }
    }
}
