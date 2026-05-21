using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public enum ComponentLifeCycleUpdate
    {
        Enable,
        Name
    }
    public class ComponentLifeCycleModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand ResetComponentLifeCycleCountCommand { get; set; }

        [XmlIgnore]
        public ICommand SetComponentLifeCycleLimitCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool componentLifeCycleEnable = false;
        [XmlIgnore]
        public bool ComponentLifeCycleEnable
        {
            get { return componentLifeCycleEnable; }
            set
            {
                if (componentLifeCycleEnable != value)
                {
                    componentLifeCycleEnable = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleEnable));
                    Messenger.Default.Send(ComponentLifeCycleUpdate.Enable);
                }
            }
        }

        private string componentLifeCycleKey = string.Empty;
        [XmlIgnore]
        public string ComponentLifeCycleKey
        {
            get { return componentLifeCycleKey; }
            set
            {
                if (componentLifeCycleKey != value)
                {
                    componentLifeCycleKey = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleKey));
                    Messenger.Default.Send(ComponentLifeCycleUpdate.Name);
                }
            }
        }

        private string componentLifeCycleName = string.Empty;
        [XmlIgnore]
        public string ComponentLifeCycleName
        {
            get { return componentLifeCycleName; }
            set
            {
                if (componentLifeCycleName != value)
                {
                    componentLifeCycleName = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleName));
                }
            }
        }

        private string componentLifeCycleCount = string.Empty;
        [XmlIgnore]
        public string ComponentLifeCycleCount
        {
            get { return componentLifeCycleCount; }
            set
            {
                if (componentLifeCycleCount != value)
                {
                    componentLifeCycleCount = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleCount));
                }
            }
        }

        private string componentLifeCycleLimit = string.Empty;
        [XmlIgnore]
        public string ComponentLifeCycleLimit
        {
            get { return componentLifeCycleLimit; }
            set
            {
                if (componentLifeCycleLimit != value)
                {
                    componentLifeCycleLimit = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleLimit));
                }
            }
        }

        private ComponentLifeCycleTagModel componentLifeCycleTagModel = new ComponentLifeCycleTagModel();
        public ComponentLifeCycleTagModel ComponentLifeCycleTagModel
        {
            get { return componentLifeCycleTagModel; }
            set
            {
                if (componentLifeCycleTagModel != value)
                {
                    componentLifeCycleTagModel = value;
                    RaisePropertyChanged(nameof(ComponentLifeCycleTagModel));
                }
            }
        }
        #endregion
    }
}