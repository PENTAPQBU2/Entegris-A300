using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupReadOnlyModel : ViewModelBase
    {
        #region PublicProperties
        private string settingsGeneralGroupReadOnlyKey = string.Empty;
        public string SettingsGeneralGroupReadOnlyKey
        {
            get { return settingsGeneralGroupReadOnlyKey; }
            set
            {
                if (settingsGeneralGroupReadOnlyKey != value)
                {
                    settingsGeneralGroupReadOnlyKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyKey));
                }
            }
        }

        private string settingsGeneralGroupReadOnlyName = string.Empty;
        [XmlIgnore]
        public string SettingsGeneralGroupReadOnlyName
        {
            get { return settingsGeneralGroupReadOnlyName; }
            set
            {
                if (settingsGeneralGroupReadOnlyName != value)
                {
                    settingsGeneralGroupReadOnlyName = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyName));
                }
            }
        }

        private string settingsGeneralGroupReadOnlyOffIcon = string.Empty;
        public string SettingsGeneralGroupReadOnlyOffIcon
        {
            get { return settingsGeneralGroupReadOnlyOffIcon; }
            set
            {
                if (settingsGeneralGroupReadOnlyOffIcon != value)
                {
                    settingsGeneralGroupReadOnlyOffIcon = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyOffIcon));
                }
            }
        }

        private string settingsGeneralGroupReadOnlyOffLabelKey = string.Empty;
        public string SettingsGeneralGroupReadOnlyOffLabelKey
        {
            get { return settingsGeneralGroupReadOnlyOffLabelKey; }
            set
            {
                if (settingsGeneralGroupReadOnlyOffLabelKey != value)
                {
                    settingsGeneralGroupReadOnlyOffLabelKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyOffLabelKey));
                }
            }
        }

        private string settingsGeneralGroupReadOnlyOffLabelName = string.Empty;
        [XmlIgnore]
        public string SettingsGeneralGroupReadOnlyOffLabelName
        {
            get { return settingsGeneralGroupReadOnlyOffLabelName; }
            set
            {
                if (settingsGeneralGroupReadOnlyOffLabelName != value)
                {
                    settingsGeneralGroupReadOnlyOffLabelName = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyOffLabelName));
                }
            }
        }

        private string settingsGeneralGroupReadOnlyOnIcon = string.Empty;
        public string SettingsGeneralGroupReadOnlyOnIcon
        {
            get { return settingsGeneralGroupReadOnlyOnIcon; }
            set
            {
                if (settingsGeneralGroupReadOnlyOnIcon != value)
                {
                    settingsGeneralGroupReadOnlyOnIcon = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyOnIcon));
                }
            }
        }

        private string settingsGeneralGroupReadOnlyOnLabelKey = string.Empty;
        public string SettingsGeneralGroupReadOnlyOnLabelKey
        {
            get { return settingsGeneralGroupReadOnlyOnLabelKey; }
            set
            {
                if (settingsGeneralGroupReadOnlyOnLabelKey != value)
                {
                    settingsGeneralGroupReadOnlyOnLabelKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyOnLabelKey));
                }
            }
        }

        //private string settingsGeneralGroupReadOnly = string.Empty;
        private string settingsGeneralGroupReadOnlyValue = "False";
        [XmlIgnore]
        public string SettingsGeneralGroupReadOnlyValue
        {
            get { return settingsGeneralGroupReadOnlyValue; }
            set
            {
                if (settingsGeneralGroupReadOnlyValue != value)
                {
                    settingsGeneralGroupReadOnlyValue = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyValue));
                }
            }
        }

        private SettingsGeneralGroupReadOnlyTagModel settingsGeneralGroupReadOnlyTagModel = new SettingsGeneralGroupReadOnlyTagModel();
        public SettingsGeneralGroupReadOnlyTagModel SettingsGeneralGroupReadOnlyTagModel
        {
            get { return settingsGeneralGroupReadOnlyTagModel; }
            set
            {
                if (settingsGeneralGroupReadOnlyTagModel != value)
                {
                    settingsGeneralGroupReadOnlyTagModel = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyTagModel));
                }
            }
        }
        #endregion
    }
}