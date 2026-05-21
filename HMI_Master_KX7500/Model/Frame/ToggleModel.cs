using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Frame
{
    public class ToggleModel : ViewModelBase
    {
        private string toggleNode = string.Empty;

        public string ToggleNode
        {
            get { return toggleNode; }
            set
            {
                if(toggleNode != value)
                {
                    toggleNode = value;
                    RaisePropertyChanged(nameof(ToggleNode));
                }
            }
        }

        private string toggleNodeType = string.Empty;

        public string ToggleNodeType
        {
            get { return toggleNodeType; }
            set
            {
                if(toggleNodeType != value)
                {
                    toggleNodeType = value;
                    RaisePropertyChanged(nameof(ToggleNodeType));
                }
            }
        }

        private bool toggleValue;

        [XmlIgnore]
        public bool ToggleValue
        {
            get { return toggleValue; }
            set
            {
                if(toggleValue != value)
                {
                    toggleValue = value;
                    RaisePropertyChanged(nameof(ToggleValue));
                }
            }
        }

        private bool toggleIsEnabled = false;

        [XmlIgnore]
        public bool ToggleIsEnabled
        {
            get { return toggleIsEnabled; }
            set
            {
                if(toggleIsEnabled != value)
                {
                    toggleIsEnabled = value;
                    RaisePropertyChanged(nameof(ToggleIsEnabled));
                }
            }
        }

        private string toggleKey = string.Empty;

        public string ToggleKey
        {
            get { return toggleKey; }
            set
            {
                if(toggleKey != value)
                {
                    toggleKey = value;
                    RaisePropertyChanged(nameof(ToggleKey));
                }
            }
        }

        private string toggleName = string.Empty;

        [XmlIgnore]
        public string ToggleName
        {
            get { return toggleName; }
            set
            {
                if(toggleName != value)
                {
                    toggleName = value;
                    RaisePropertyChanged(nameof(ToggleName));
                }
            }
        }

        private string toggleImage = string.Empty;

        public string ToggleImage
        {
            get { return toggleImage; }
            set
            {
                if(toggleImage != value)
                {
                    toggleImage = value;
                    RaisePropertyChanged(nameof(ToggleImage));
                }
            }
        }
    }
}