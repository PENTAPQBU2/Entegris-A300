using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Main.Module.Indicator
{
    public class MachineLayoutModuleStatusIndicatorTagModel : ViewModelBase
    {
        #region PublicProperties

        private string machineLayoutModuleStatusIndicatorTag = string.Empty;

        public string MachineLayoutModuleStatusIndicatorTag
        {
            get { return machineLayoutModuleStatusIndicatorTag; }
            set
            {
                if(machineLayoutModuleStatusIndicatorTag != value)
                {
                    machineLayoutModuleStatusIndicatorTag = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorTag));
                }
            }
        }

        #endregion PublicProperties
    }
}