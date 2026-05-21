using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Main.Status
{
    public class MachineLayoutModuleStatusModel : ViewModelBase
    {
        #region PublicProperties

        private string machineLayoutModuleStatusKey = string.Empty;

        public string MachineLayoutModuleStatusKey
        {
            get { return machineLayoutModuleStatusKey; }
            set
            {
                if(machineLayoutModuleStatusKey != value)
                {
                    machineLayoutModuleStatusKey = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusKey));
                }
            }
        }

        private string machineLayoutModuleStatusName = string.Empty;

        [XmlIgnore]
        public string MachineLayoutModuleStatusName
        {
            get { return machineLayoutModuleStatusName; }
            set
            {
                if(machineLayoutModuleStatusName != value)
                {
                    machineLayoutModuleStatusName = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusName));
                }
            }
        }

        private string machineLayoutModuleStatusIndex = string.Empty;

        public string MachineLayoutModuleStatusIndex
        {
            get { return machineLayoutModuleStatusIndex; }
            set
            {
                if(machineLayoutModuleStatusIndex != value)
                {
                    machineLayoutModuleStatusIndex = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndex));
                }
            }
        }

        private string machineLayoutModuleStatusColor = string.Empty;

        public string MachineLayoutModuleStatusColor
        {
            get { return machineLayoutModuleStatusColor; }
            set
            {
                if(machineLayoutModuleStatusColor != value)
                {
                    machineLayoutModuleStatusColor = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusColor));
                }
            }
        }

        #endregion PublicProperties
    }
}