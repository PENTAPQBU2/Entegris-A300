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
    }
}