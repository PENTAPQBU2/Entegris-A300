using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupNumericModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SetEngineeringGroupNumericCommand { get; set; }
        #endregion

        #region PublicProperties
        private string engineeringGroupNumericKey = string.Empty;
        public string EngineeringGroupNumericKey
        {
            get { return engineeringGroupNumericKey; }
            set
            {
                if (engineeringGroupNumericKey != value)
                {
                    engineeringGroupNumericKey = value;
                    RaisePropertyChanged(nameof(EngineeringGroupNumericKey));
                }
            }
        }

        private string engineeringGroupNumericName = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupNumericName
        {
            get { return engineeringGroupNumericName; }
            set
            {
                if (engineeringGroupNumericName != value)
                {
                    engineeringGroupNumericName = value;
                    RaisePropertyChanged(nameof(EngineeringGroupNumericName));
                }
            }
        }

        private EngineeringGroupNumericTagModel engineeringGroupNumericTagModel = new EngineeringGroupNumericTagModel();
        public EngineeringGroupNumericTagModel EngineeringGroupNumericTagModel
        {
            get { return engineeringGroupNumericTagModel; }
            set
            {
                if (engineeringGroupNumericTagModel != value)
                {
                    engineeringGroupNumericTagModel = value;
                    RaisePropertyChanged(nameof(EngineeringGroupNumericTagModel));
                }
            }
        }

        private double engineeringGroupNumericValue = 0;
        [XmlIgnore]
        public double EngineeringGroupNumericValue
        {
            get { return engineeringGroupNumericValue; }
            set
            {
                value = Math.Round(value, EngineeringGroupDecimalPlace);

                if (engineeringGroupNumericValue != value)
                {
                    engineeringGroupNumericValue = value;
                    RaisePropertyChanged(nameof(EngineeringGroupNumericValue));
                }
            }
        }

        private int engineeringGroupDecimalPlace = 0;
        public int EngineeringGroupDecimalPlace
        {
            get { return engineeringGroupDecimalPlace; }
            set
            {
                if (engineeringGroupDecimalPlace != value)
                {
                    engineeringGroupDecimalPlace = value;
                    RaisePropertyChanged(nameof(EngineeringGroupDecimalPlace));
                }
            }
        }
        #endregion
    }
}