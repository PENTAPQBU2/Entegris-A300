using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Language
{
    public class LanguageModel : ViewModelBase
    {
        #region PublicProperties

        private string languageKey = string.Empty;

        public string LanguageKey
        {
            get { return languageKey; }
            set
            {
                if(languageKey != value)
                {
                    languageKey = value;
                    RaisePropertyChanged(nameof(LanguageKey));
                }
            }
        }

        private string languageName = string.Empty;

        [XmlIgnore]
        public string LanguageName
        {
            get { return languageName; }
            set
            {
                if(languageName != value)
                {
                    languageName = value;
                    RaisePropertyChanged(nameof(LanguageName));
                }
            }
        }

        private bool defaultLanguage = false;

        public bool DefaultLanguage
        {
            get { return defaultLanguage; }
            set
            {
                if(defaultLanguage != value)
                {
                    defaultLanguage = value;
                    RaisePropertyChanged(nameof(DefaultLanguage));
                }
            }
        }

        #endregion PublicProperties
    }
}