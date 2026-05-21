using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.MachineStatus
{
    public class MachineStatusModel : ViewModelBase
    {
        private string machineStatusNode = string.Empty;

        public string MachineStatusNode
        {
            get { return machineStatusNode; }
            set
            {
                if(machineStatusNode != value)
                {
                    machineStatusNode = value;
                    RaisePropertyChanged(nameof(MachineStatusNode));
                }
            }
        }

        private string machineStatusType = string.Empty;

        public string MachineStatusType
        {
            get { return machineStatusType; }
            set
            {
                if(machineStatusType != value)
                {
                    machineStatusType = value;
                    RaisePropertyChanged(nameof(MachineStatusType));
                }
            }
        }
    }
}