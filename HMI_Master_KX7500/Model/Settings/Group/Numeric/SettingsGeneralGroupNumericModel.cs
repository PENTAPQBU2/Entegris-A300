using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupNumericModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SetSettingsGeneralGroupNumericCommand { get; set; }
        #endregion

        #region PublicProperties
        private string settingsGeneralGroupNumericKey = string.Empty;
        public string SettingsGeneralGroupNumericKey
        {
            get { return settingsGeneralGroupNumericKey; }
            set
            {
                if (settingsGeneralGroupNumericKey != value)
                {
                    settingsGeneralGroupNumericKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericKey));
                }
            }
        }

        private string settingsGeneralGroupNumericName = string.Empty;
        [XmlIgnore]
        public string SettingsGeneralGroupNumericName
        {
            get { return settingsGeneralGroupNumericName; }
            set
            {
                if (settingsGeneralGroupNumericName != value)
                {
                    settingsGeneralGroupNumericName = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericName));
                }
            }
        }

        private double settingsGeneralGroupNumericMaximum = 0;
        public double SettingsGeneralGroupNumericMaximum
        {
            get { return settingsGeneralGroupNumericMaximum; }
            set
            {
                if (settingsGeneralGroupNumericMaximum != value)
                {
                    settingsGeneralGroupNumericMaximum = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericMaximum));
                }
            }
        }

        private double settingsGeneralGroupNumericMinimum = 0;
        public double SettingsGeneralGroupNumericMinimum
        {
            get { return settingsGeneralGroupNumericMinimum; }
            set
            {
                if (settingsGeneralGroupNumericMinimum != value)
                {
                    settingsGeneralGroupNumericMinimum = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericMinimum));
                }
            }
        }

        private double settingsGeneralGroupNumericValue = 0;
        [XmlIgnore]
        public double SettingsGeneralGroupNumericValue
        {
            get { return settingsGeneralGroupNumericValue; }
            set
            {
                value = Math.Round(value, settingsGeneralGroupDecimalPlace);
                //value = Convert.ToInt32(Math.Round(Convert.ToDouble(value), settingsGeneralGroupDecimalPlace));

                if (settingsGeneralGroupNumericValue != value)
                {
                    settingsGeneralGroupNumericValue = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericValue));
                }
            }
        }

        private SettingsGeneralGroupNumericTagModel settingsGeneralGroupNumericTagModel = new SettingsGeneralGroupNumericTagModel();
        public SettingsGeneralGroupNumericTagModel SettingsGeneralGroupNumericTagModel
        {
            get { return settingsGeneralGroupNumericTagModel; }
            set
            {
                if (settingsGeneralGroupNumericTagModel != value)
                {
                    settingsGeneralGroupNumericTagModel = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericTagModel));
                }
            }
        }

        private int settingsGeneralGroupDecimalPlace = 0;
        public int SettingsGeneralGroupDecimalPlace
        {
            get { return settingsGeneralGroupDecimalPlace; }
            set
            {
                if (settingsGeneralGroupDecimalPlace != value)
                {
                    settingsGeneralGroupDecimalPlace = value;
                    
                    SettingGeneralGroupDecimalPlacesString = "0";
                    for (int i = 0; i < settingsGeneralGroupDecimalPlace; i++)
                    {
                        if (i == 0)
                            SettingGeneralGroupDecimalPlacesString += ".";
                        SettingGeneralGroupDecimalPlacesString += "0";
                    }
                    
                    RaisePropertyChanged(nameof(SettingsGeneralGroupDecimalPlace));
                }
            }
        }

        private string settingGeneralGroupDecimalPlacesString = "0";
        public string SettingGeneralGroupDecimalPlacesString
        {
            get { return settingGeneralGroupDecimalPlacesString; }
            set
            {
                if (settingGeneralGroupDecimalPlacesString != value)
                {
                    settingGeneralGroupDecimalPlacesString = value;
                    RaisePropertyChanged(nameof(SettingGeneralGroupDecimalPlacesString));
                }
            }
        }
        #endregion
    }
}