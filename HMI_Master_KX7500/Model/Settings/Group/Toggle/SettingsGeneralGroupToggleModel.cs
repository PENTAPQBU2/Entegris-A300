using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupToggleModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SettingsGeneralGroupToggleCommand { get; set; }

        #endregion

        #region PublicProperties
        private string settingsGeneralGroupToggleKey = string.Empty;
        public string SettingsGeneralGroupToggleKey
        {
            get { return settingsGeneralGroupToggleKey; }
            set
            {
                if (settingsGeneralGroupToggleKey != value)
                {
                    settingsGeneralGroupToggleKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleKey));
                }
            }
        }

        private string settingsGeneralGroupToggleName = string.Empty;
        [XmlIgnore]
        public string SettingsGeneralGroupToggleName
        {
            get { return settingsGeneralGroupToggleName; }
            set
            {
                if (settingsGeneralGroupToggleName != value)
                {
                    settingsGeneralGroupToggleName = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleName));
                }
            }
        }

        private string settingsGeneralGroupToggleOffIcon = string.Empty;
        public string SettingsGeneralGroupToggleOffIcon
        {
            get { return settingsGeneralGroupToggleOffIcon; }
            set
            {
                if (settingsGeneralGroupToggleOffIcon != value)
                {
                    settingsGeneralGroupToggleOffIcon = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleOffIcon));
                }
            }
        }

        private string settingsGeneralGroupToggleOffLabelKey = string.Empty;
        public string SettingsGeneralGroupToggleOffLabelKey
        {
            get { return settingsGeneralGroupToggleOffLabelKey; }
            set
            {
                if (settingsGeneralGroupToggleOffLabelKey != value)
                {
                    settingsGeneralGroupToggleOffLabelKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleOffLabelKey));
                }
            }
        }

        private string settingsGeneralGroupToggleOffLabelName = string.Empty;
        [XmlIgnore]
        public string SettingsGeneralGroupToggleOffLabelName
        {
            get { return settingsGeneralGroupToggleOffLabelName; }
            set
            {
                if (settingsGeneralGroupToggleOffLabelName != value)
                {
                    settingsGeneralGroupToggleOffLabelName = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleOffLabelName));
                }
            }
        }

        private string settingsGeneralGroupToggleOnIcon = string.Empty;
        public string SettingsGeneralGroupToggleOnIcon
        {
            get { return settingsGeneralGroupToggleOnIcon; }
            set
            {
                if (settingsGeneralGroupToggleOnIcon != value)
                {
                    settingsGeneralGroupToggleOnIcon = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleOnIcon));
                }
            }
        }

        private string settingsGeneralGroupToggleOnLabelKey = string.Empty;
        public string SettingsGeneralGroupToggleOnLabelKey
        {
            get { return settingsGeneralGroupToggleOnLabelKey; }
            set
            {
                if (settingsGeneralGroupToggleOnLabelKey != value)
                {
                    settingsGeneralGroupToggleOnLabelKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleOnLabelKey));
                }
            }
        }

        //private string settingsGeneralGroupToggle = string.Empty;
        private string settingsGeneralGroupToggleValue = "False";
        [XmlIgnore]
        public string SettingsGeneralGroupToggleValue
        {
            get { return settingsGeneralGroupToggleValue; }
            set
            {
                if (settingsGeneralGroupToggleValue != value)
                {
                    settingsGeneralGroupToggleValue = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleValue));
                }
            }
        }

        private SettingsGeneralGroupToggleTagModel settingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel();
        public SettingsGeneralGroupToggleTagModel SettingsGeneralGroupToggleTagModel
        {
            get { return settingsGeneralGroupToggleTagModel; }
            set
            {
                if (settingsGeneralGroupToggleTagModel != value)
                {
                    settingsGeneralGroupToggleTagModel = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleTagModel));
                }
            }
        }
        #endregion
    }
}