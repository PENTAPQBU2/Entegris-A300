using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Modules.Language;
using HMI_Master_KX7500.Service.Language;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace HMI_Master_KX7500.ModulesView.Language
{
    public class Language_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields

        private readonly LanguageService languageService = new LanguageService();
        private readonly LanguageEditorView languageEditorView = new LanguageEditorView();
        private readonly LanguageTranslationEditorView languageTranslationEditorView = new LanguageTranslationEditorView();
        private List<LanguageModel> language = new List<LanguageModel>();

        #endregion PrivateFields

        #region PublicCommandProperties

        public ICommand LanguageSelectionChangedCommand { get; set; }

        public ICommand EditLanguageCommand { get; set; }

        public ICommand EditTranslationCommand { get; set; }

        #endregion PublicCommandProperties

        #region PublicProperties

        private ICollectionView _language = null;

        public ICollectionView Language
        {
            get { return _language; }
            set
            {
                if(_language != value)
                {
                    _language = value;
                    RaisePropertyChanged(nameof(Language));
                }
            }
        }

        private LanguageModel selectedLanguage = new LanguageModel();

        public LanguageModel SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                if(selectedLanguage != value)
                {
                    selectedLanguage = value;
                    RaisePropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private bool allowEditLanguage = false;

        public bool AllowEditLanguage
        {
            get { return allowEditLanguage; }
            set
            {
                if(allowEditLanguage != value)
                {
                    allowEditLanguage = value;
                    RaisePropertyChanged(nameof(AllowEditLanguage));
                }
            }
        }

        private bool allowEditTranslation = false;

        public bool AllowEditTranslation
        {
            get { return allowEditTranslation; }
            set
            {
                if(allowEditTranslation != value)
                {
                    allowEditTranslation = value;
                    RaisePropertyChanged(nameof(AllowEditTranslation));
                }
            }
        }

        #endregion PublicProperties

        #region Constructor

        public Language_OPCUA_ViewModel()
        {
            initializeCommand();
            initializeMessenger();
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeCommand()
        {
            LanguageSelectionChangedCommand = new RelayCommand(languageSelectionChangedCommand);
            EditLanguageCommand = new RelayCommand(editLanguageCommand);
            EditTranslationCommand = new RelayCommand(editTranslationCommand);
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<LanguageInitializeModel>(this, initializeLanguage);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }

        #endregion PrivateInitializeMethods

        #region PrivateCommandMethods

        private void languageSelectionChangedCommand()
        {
            if(SelectedLanguage != null)
            {
                LanguageService.ChangeLanguage(SelectedLanguage);
            }
        }

        private void editLanguageCommand()
        {
            languageEditorView.ShowDialog();
        }

        private void editTranslationCommand()
        {
            languageTranslationEditorView.ShowDialog();
        }

        #endregion PrivateCommandMethods

        #region PrivateMessengerMethods

        private void initializeLanguage(LanguageInitializeModel languageInitializeModel)
        {
            language = LanguageService.DeserializeLanguage();

            Language = null;
            Language = CollectionViewSource.GetDefaultView(language);

            SelectedLanguage = LanguageService.GetDefaultLanguage(language);

            languageSelectionChangedCommand();
        }

        private void updateUserAccount(/*UserAccountModel userAccountModel*/)
        {
            //AllowEditLanguage = UserAccountService.IsUserAccountGroupAccessAllow(
            //        nameof(UserAccountGroupAccess.LanguageEditor));

            //AllowEditTranslation = UserAccountService.IsUserAccountGroupAccessAllow(
            //    nameof(UserAccountGroupAccess.LanguageTranslationEditor));
        }

        private void updateLanguage(LanguageModel languageModel)
        {
            language.ForEach(x =>
            {
                x.LanguageName = x.LanguageKey;
            });
        }

        #endregion PrivateMessengerMethods
    }
}