using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Frame
{
    public class MachineSpeedModel : ViewModelBase
    {
        private string machineSpeedNode = string.Empty;

        public string MachineSpeedNode
        {
            get { return machineSpeedNode; }
            set
            {
                if(machineSpeedNode != value)
                {
                    machineSpeedNode = value;
                    RaisePropertyChanged(nameof(MachineSpeedNode));
                }
            }
        }

        private string machineSpeedType = string.Empty;

        public string MachineSpeedType
        {
            get { return machineSpeedType; }
            set
            {
                if(machineSpeedType != value)
                {
                    machineSpeedType = value;
                    RaisePropertyChanged(nameof(MachineSpeedType));
                }
            }
        }
    }
}