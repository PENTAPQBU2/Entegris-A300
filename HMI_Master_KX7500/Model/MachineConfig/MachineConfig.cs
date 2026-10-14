using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.MachineConfig
{
    public class MachineConfigModel : ViewModelBase
    {
        //private bool debugMode = false;
        //public bool DebugMode

        private bool bypassSignIn = false;

        public bool BypassSignIn
        {
            get { return bypassSignIn; }
            set
            {
                if(bypassSignIn != value)
                {
                    bypassSignIn = value;
                    RaisePropertyChanged(nameof(BypassSignIn));
                }
            }
        }

        private bool initializeKX7500 = true;

        public bool InitializeKX7500
        {
            get { return initializeKX7500; }
            set
            {
                if(initializeKX7500 != value)
                {
                    initializeKX7500 = value;
                    RaisePropertyChanged(nameof(InitializeKX7500));
                }
            }
        }

        //Offline mode: no PLC, COM port or TCP server needed. PLC reads/writes go to an in-memory simulated PLC.
        //For simulation on an office PC only. Must be false on the machine.
        private bool offlineMode = false;

        public bool OfflineMode
        {
            get { return offlineMode; }
            set
            {
                if(offlineMode != value)
                {
                    offlineMode = value;
                    RaisePropertyChanged(nameof(OfflineMode));
                }
            }
        }

        private string plcIPAddress = "192.168.0.10";

        public string PLCIPAddress
        {
            get { return plcIPAddress; }
            set
            {
                if(plcIPAddress != value)
                {
                    plcIPAddress = value;
                    RaisePropertyChanged(nameof(PLCIPAddress));
                }
            }
        }

        private string plcPort = "8500";

        public string PLCPort
        {
            get { return plcPort; }
            set
            {
                if(plcPort != value)
                {
                    plcPort = value;
                    RaisePropertyChanged(nameof(PLCPort));
                }
            }
        }

        //Value passed to KV DataBuilder DBCommManager.KVSimulator (was always true before V1.4.0.0)
        private bool useKVStudioSimulator = true;

        public bool UseKVStudioSimulator
        {
            get { return useKVStudioSimulator; }
            set
            {
                if(useKVStudioSimulator != value)
                {
                    useKVStudioSimulator = value;
                    RaisePropertyChanged(nameof(UseKVStudioSimulator));
                }
            }
        }

        //Folder for FOUP CSV (torque) log. Empty = D:\HMI\FOUP CSV\ (Release) or D:\HMI_A300\FOUP CSV\ (Debug),
        //falling back to "<HMI folder>\FOUP CSV\" when that drive does not exist.
        private string foupCsvFolder = string.Empty;

        public string FoupCsvFolder
        {
            get { return foupCsvFolder; }
            set
            {
                if(foupCsvFolder != value)
                {
                    foupCsvFolder = value;
                    RaisePropertyChanged(nameof(FoupCsvFolder));
                }
            }
        }
    }
}