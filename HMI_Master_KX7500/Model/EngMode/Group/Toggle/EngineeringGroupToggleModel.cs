using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupToggleModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand onEngineeringGroupToggleCommand { get; set; }
        [XmlIgnore]
        public ICommand offEngineeringGroupToggleCommand { get; set; }
        #endregion
        private string engineeringGroupToggleKey = string.Empty;
        public string EngineeringGroupToggleKey
        {
            get { return engineeringGroupToggleKey; }
            set
            {
                if(engineeringGroupToggleKey != value)
                {
                    engineeringGroupToggleKey = value;
                    RaisePropertyChanged(nameof(EngineeringGroupToggleKey));
                }
            }
        }

        private string engineeringGroupToggleName = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupToggleName
        {
            get { return engineeringGroupToggleName; }
            set
            {
                if (engineeringGroupToggleName != value)
                {
                    engineeringGroupToggleName = value;
                    RaisePropertyChanged(nameof(EngineeringGroupToggleName));
                }
            }
        }

        private EngineeringGroupToggleTagModel engineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel();
        public EngineeringGroupToggleTagModel EngineeringGroupToggleTagModel
        {
            get { return engineeringGroupToggleTagModel; }
            set
            {
                if(engineeringGroupToggleTagModel != value)
                {
                    engineeringGroupToggleTagModel = value;
                    RaisePropertyChanged(nameof(EngineeringGroupToggleTagModel));
                }
            }
        }

        private bool engineeringGroupToggleValue = false;
        [XmlIgnore]
        public bool EngineeringGroupToggleValue
        {
            get { return engineeringGroupToggleValue; }
            set
            {
                if(engineeringGroupToggleValue != value)
                {
                    engineeringGroupToggleValue = value;
                    RaisePropertyChanged(nameof(engineeringGroupToggleValue));
                }
            }
        }
    }
}
