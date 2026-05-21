using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace HMI_Master_KX7500.Model.Language.Translation
{
    public class LanguageTranslationEditModel : ViewModelBase
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

        private ObservableCollection<LanguageTranslationModel> languageTranslationValueList = new ObservableCollection<LanguageTranslationModel>();

        public ObservableCollection<LanguageTranslationModel> LanguageTranslationValueList
        {
            get { return languageTranslationValueList; }
            set
            {
                if(languageTranslationValueList != value)
                {
                    languageTranslationValueList = value;
                    RaisePropertyChanged(nameof(LanguageTranslationValueList));
                }
            }
        }

        #endregion PublicProperties
    }
}