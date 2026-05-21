using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Frame
{
    public class MachineModeModel : ViewModelBase
    {
        private string machineModeNode = string.Empty;

        public string MachineModeNode
        {
            get { return machineModeNode; }
            set
            {
                if(machineModeNode != value)
                {
                    machineModeNode = value;
                    RaisePropertyChanged(nameof(MachineModeNode));
                }
            }
        }

        private string machineModeType = string.Empty;

        public string MachineModeType
        {
            get { return machineModeType; }
            set
            {
                if(machineModeType != value)
                {
                    machineModeType = value;
                    RaisePropertyChanged(nameof(MachineModeType));
                }
            }
        }
    }
}