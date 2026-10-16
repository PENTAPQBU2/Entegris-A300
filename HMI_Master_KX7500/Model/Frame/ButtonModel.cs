using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Frame
{
    public class ButtonModel : ViewModelBase
    {
        private string buttonKey = string.Empty;

        public string ButtonKey
        {
            get { return buttonKey; }
            set
            {
                if(buttonKey != value)
                {
                    buttonKey = value;
                    RaisePropertyChanged(nameof(ButtonKey));
                }
            }
        }

        private string buttonName = string.Empty;

        [XmlIgnore]
        public string ButtonName
        {
            get { return buttonName; }
            set
            {
                if(buttonName != value)
                {
                    buttonName = value;
                    RaisePropertyChanged(nameof(ButtonName));
                }
            }
        }

        private string buttonNode = string.Empty;

        public string ButtonNode
        {
            get { return buttonNode; }
            set
            {
                if(buttonNode != value)
                {
                    buttonNode = value;
                    RaisePropertyChanged(nameof(ButtonNode));
                }
            }
        }

        private string buttonNodeType = string.Empty;

        public string ButtonNodeType
        {
            get { return buttonNodeType; }
            set
            {
                if(buttonNodeType != value)
                {
                    buttonNodeType = value;
                    RaisePropertyChanged(nameof(ButtonNodeType));
                }
            }
        }

        private string buttonImage = string.Empty;

        public string ButtonImage
        {
            get { return buttonImage; }
            set
            {
                if(buttonKey != value)
                {
                    buttonImage = value;
                    RaisePropertyChanged(nameof(ButtonImage));
                }
            }
        }

        private bool buttonIsEnabled = true;

        // Initialize button: greyed out while Machine_Sys_DB_StartInit is ON (MT_005)
        [XmlIgnore]
        public bool ButtonIsEnabled
        {
            get { return buttonIsEnabled; }
            set
            {
                if(buttonIsEnabled != value)
                {
                    buttonIsEnabled = value;
                    RaisePropertyChanged(nameof(ButtonIsEnabled));
                }
            }
        }
    }
}