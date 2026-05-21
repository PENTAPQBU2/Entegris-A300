using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Models
{
    public class ComponentLifeCycleTagModel : ViewModelBase
    {
        #region PublicProperties
        private string componentLifeCycleEnableTag = string.Empty;
        public string ComponentLifeCycleEnableTag
        {
            get { return componentLifeCycleEnableTag; }
            set
            {
                if (componentLifeCycleEnableTag != value)
                {
                    componentLifeCycleEnableTag = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleEnableTag));
                }
            }
        }

        private string componentLifeCycleNameTag = string.Empty;
        public string ComponentLifeCycleNameTag
        {
            get { return componentLifeCycleNameTag; }
            set
            {
                if (componentLifeCycleNameTag != value)
                {
                    componentLifeCycleNameTag = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleNameTag));
                }
            }
        }

        private string componentLifeCycleCountTag = string.Empty;
        public string ComponentLifeCycleCountTag
        {
            get { return componentLifeCycleCountTag; }
            set
            {
                if (componentLifeCycleCountTag != value)
                {
                    componentLifeCycleCountTag = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleCountTag));
                }
            }
        }

        private string componentLifeCycleCountResetTag = string.Empty;
        public string ComponentLifeCycleCountResetTag
        {
            get { return componentLifeCycleCountResetTag; }
            set
            {
                if (componentLifeCycleCountResetTag != value)
                {
                    componentLifeCycleCountResetTag = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleCountResetTag));
                }
            }
        }

        private string componentLifeCycleLimitTag = string.Empty;
        public string ComponentLifeCycleLimitTag
        {
            get { return componentLifeCycleLimitTag; }
            set
            {
                if (componentLifeCycleLimitTag != value)
                {
                    componentLifeCycleLimitTag = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleLimitTag));
                }
            }
        }
        #endregion
    }
}