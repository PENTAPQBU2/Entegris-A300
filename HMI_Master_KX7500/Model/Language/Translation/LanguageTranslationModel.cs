using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Language.Translation
{
    public class LanguageTranslationModel : ViewModelBase
    {
        #region PublicProperties

        private string languageTranslationKey = string.Empty;

        public string LanguageTranslationKey
        {
            get { return languageTranslationKey; }
            set
            {
                if(languageTranslationKey != value)
                {
                    languageTranslationKey = value;
                    RaisePropertyChanged(nameof(LanguageTranslationKey));
                }
            }
        }

        private string languageTranslationValue = string.Empty;

        public string LanguageTranslationValue
        {
            get { return languageTranslationValue; }
            set
            {
                if(languageTranslationValue != value)
                {
                    languageTranslationValue = value;
                    RaisePropertyChanged(nameof(LanguageTranslationValue));
                }
            }
        }

        #endregion PublicProperties
    }
}