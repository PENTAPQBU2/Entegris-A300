using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupReadOnlyTagModel : ViewModelBase
    {
        #region PublicProperties
        private string settingsGeneralGroupReadOnlyTag = string.Empty;
        public string SettingsGeneralGroupReadOnlyTag
        {
            get { return settingsGeneralGroupReadOnlyTag; }
            set
            {
                if (settingsGeneralGroupReadOnlyTag != value)
                {
                    settingsGeneralGroupReadOnlyTag = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyTag));
                }
            }
        }
        #endregion
    }
}