using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupReadOnlyModel : ViewModelBase
    {
        private string engineeringGroupReadOnlyKey = string.Empty;
        public string EngineeringGroupReadOnlyKey
        {
            get { return engineeringGroupReadOnlyKey; }
            set
            {
                if (engineeringGroupReadOnlyKey != value)
                {
                    engineeringGroupReadOnlyKey = value;
                    RaisePropertyChanged(nameof(EngineeringGroupReadOnlyKey));
                }
            }
        }

        private string engineeringGroupReadOnlyName = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupReadOnlyName
        {
            get { return engineeringGroupReadOnlyName; }
            set
            {
                if (engineeringGroupReadOnlyName != value)
                {
                    engineeringGroupReadOnlyName = value;
                    RaisePropertyChanged(nameof(EngineeringGroupReadOnlyName));
                }
            }
        }

        private EngineeringGroupReadOnlyTagModel engineeringGroupReadOnlyTagModel = new EngineeringGroupReadOnlyTagModel();
        public EngineeringGroupReadOnlyTagModel EngineeringGroupReadOnlyTagModel
        {
            get { return engineeringGroupReadOnlyTagModel; }
            set
            {
                if (engineeringGroupReadOnlyTagModel != value)
                {
                    engineeringGroupReadOnlyTagModel = value;
                    RaisePropertyChanged(nameof(EngineeringGroupReadOnlyTagModel));
                }
            }
        }

        private string engineeringGroupReadOnlyValue = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupReadOnlyValue
        {
            get { return engineeringGroupReadOnlyValue; }
            set
            {
                if (engineeringGroupReadOnlyValue != value)
                {
                    engineeringGroupReadOnlyValue = value;
                    RaisePropertyChanged(nameof(engineeringGroupReadOnlyValue));
                }
            }
        }
    }
}
