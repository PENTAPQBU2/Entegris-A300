using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupTextModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand sendEngineeringGroupTextCommand { get; set; }
        #endregion
        private string engineeringGroupTextKey = string.Empty;
        public string EngineeringGroupTextKey
        {
            get { return engineeringGroupTextKey; }
            set
            {
                if(engineeringGroupTextKey != value)
                {
                    engineeringGroupTextKey = value;
                    RaisePropertyChanged(nameof(EngineeringGroupTextKey));
                }
            }
        }

        private string engineeringGroupTextName = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupTextName
        {
            get { return engineeringGroupTextName; }
            set
            {
                if (engineeringGroupTextName != value)
                {
                    engineeringGroupTextName = value;
                    RaisePropertyChanged(nameof(EngineeringGroupTextName));
                }
            }
        }

        private EngineeringGroupTextTagModel engineeringGroupTextTagModel = new EngineeringGroupTextTagModel();
        public EngineeringGroupTextTagModel EngineeringGroupTextTagModel
        {
            get { return engineeringGroupTextTagModel; }
            set
            {
                if(engineeringGroupTextTagModel != value)
                {
                    engineeringGroupTextTagModel = value;
                    RaisePropertyChanged(nameof(EngineeringGroupTextTagModel));
                }
            }
        }

        private string engineeringGroupTextValue = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupTextValue
        {
            get { return engineeringGroupTextValue; }
            set
            {
                if(engineeringGroupTextValue != value)
                {
                    engineeringGroupTextValue = value;
                    RaisePropertyChanged(nameof(engineeringGroupTextValue));
                }
            }
        }
    }
}
