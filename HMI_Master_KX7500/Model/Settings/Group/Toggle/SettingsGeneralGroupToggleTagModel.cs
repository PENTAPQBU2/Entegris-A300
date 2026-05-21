using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupToggleTagModel : ViewModelBase
    {
        #region PublicProperties
        private string settingsGeneralGroupToggleTag = string.Empty;
        public string SettingsGeneralGroupToggleTag
        {
            get { return settingsGeneralGroupToggleTag; }
            set
            {
                if (settingsGeneralGroupToggleTag != value)
                {
                    settingsGeneralGroupToggleTag = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleTag));
                }
            }
        }
        #endregion
    }
}