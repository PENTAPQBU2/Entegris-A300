using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupNumericTagModel : ViewModelBase
    {
        #region PublicProperties
        private string settingsGeneralGroupNumericTag = string.Empty;
        public string SettingsGeneralGroupNumericTag
        {
            get { return settingsGeneralGroupNumericTag; }
            set
            {
                if (settingsGeneralGroupNumericTag != value)
                {
                    settingsGeneralGroupNumericTag = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericTag));
                }
            }
        }
        #endregion
    }
}