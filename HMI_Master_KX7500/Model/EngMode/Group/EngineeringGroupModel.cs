using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class EngineeringGroupModel : ViewModelBase
    {
        private List<EngineeringGroupToggleModel> engineeringGroupToggleList = new List<EngineeringGroupToggleModel>();
        public List<EngineeringGroupToggleModel> EngineeringGroupToggleList
        {
            get { return engineeringGroupToggleList; }
            set
            {
                if(engineeringGroupToggleList != value)
                {
                    engineeringGroupToggleList = value;
                    RaisePropertyChanged(nameof(EngineeringGroupToggleList));
                }
            }
        }

        private List<EngineeringGroupNumericModel> engineeringGroupNumericList = new List<EngineeringGroupNumericModel>();
        public List<EngineeringGroupNumericModel> EngineeringGroupNumericList
        {
            get { return engineeringGroupNumericList; }
            set
            {
                if (engineeringGroupNumericList != value)
                {
                    engineeringGroupNumericList = value;
                    RaisePropertyChanged(nameof(EngineeringGroupNumericList));
                }
            }
        }

        private List<EngineeringGroupReadOnlyModel> engineeringGroupReadOnlyList = new List<EngineeringGroupReadOnlyModel>();
        public List<EngineeringGroupReadOnlyModel> EngineeringGroupReadOnlyList
        {
            get { return engineeringGroupReadOnlyList; }
            set
            {
                if (engineeringGroupReadOnlyList != value)
                {
                    engineeringGroupReadOnlyList = value;
                    RaisePropertyChanged(nameof(EngineeringGroupReadOnlyList));
                }
            }
        }

        private List<EngineeringGroupTextModel> engineeringGroupTextList = new List<EngineeringGroupTextModel>();
        public List<EngineeringGroupTextModel> EngineeringGroupTextList
        {
            get { return engineeringGroupTextList; }
            set
            {
                if (engineeringGroupTextList != value)
                {
                    engineeringGroupTextList = value;
                    RaisePropertyChanged(nameof(EngineeringGroupTextList));
                }
            }
        }

        private string engineeringGroupKey = string.Empty;
        public string EngineeringGroupKey
        {
            get { return engineeringGroupKey; }
            set
            {
                if(engineeringGroupKey != value)
                {
                    engineeringGroupKey = value;
                    RaisePropertyChanged(nameof(EngineeringGroupKey));
                }
            }
        }

        private string engineeringGroupName = string.Empty;
        [XmlIgnore]
        public string EngineeringGroupName
        {
            get { return engineeringGroupName; }
            set
            {
                if (engineeringGroupName != value)
                {
                    engineeringGroupName = value;
                    RaisePropertyChanged(nameof(EngineeringGroupName));
                }
            }
        }

        private bool isAllowedAccess = true;
        [XmlIgnore]
        public bool IsAllowedAccess
        {
            get { return isAllowedAccess; }
            set
            {
                if (isAllowedAccess != value)
                {
                    isAllowedAccess = value;
                    RaisePropertyChanged(nameof(IsAllowedAccess));
                }
            }
        }

        private bool isAccessControlled = false;
        public bool IsAccessControlled
        {
            get { return isAccessControlled; }
            set
            {
                if (isAccessControlled != value)
                {
                    isAccessControlled = value;
                    RaisePropertyChanged(nameof(IsAccessControlled));
                }
            }
        }
    }
}
