using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.OEE;
using HMI_Master_KX7500.Service.Language;
using NLog;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HMI_Master_KX7500.ModulesView.Language
{
    public class LanguageEditorViewModel : ViewModelBase
    {
        #region PrivateFields

        private List<LanguageModel> language = new List<LanguageModel>();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion PrivateFields

        #region PublicCommandProperties

        public ICommand LanguageEditorViewClosingCommand { get; set; }

        public ICommand AddLanguageCommand { get; set; }

        public ICommand DeleteLanguageCommand { get; set; }

        public ICommand SaveLanguageCommand { get; set; }

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

        #endregion PublicProperties

        #region Constructor

        public LanguageEditorViewModel()
        {
            initializeCommand();
            initializeMessenger();
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeCommand()
        {
            LanguageEditorViewClosingCommand = new RelayCommand<EventArgsModel>(
                    languageEditorViewClosingCommand);
            AddLanguageCommand = new RelayCommand<ComboBox>(addLanguageCommand);
            DeleteLanguageCommand = new RelayCommand<LanguageModel>(deleteLanguageCommand);
            SaveLanguageCommand = new RelayCommand<Window>(saveLanguageCommand);
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<LanguageInitializeModel>(this, initializeLanguage);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }

        #endregion PrivateInitializeMethods

        #region PrivateCommandMethods

        private void languageEditorViewClosingCommand(EventArgsModel eventArgsModel)
        {
            CancelEventArgs cancelEventArgs = eventArgsModel.EventArgs as CancelEventArgs;
            cancelEventArgs.Cancel = true;

            Window window = eventArgsModel.Parameter as Window;
            window.Hide();
        }

        private void addLanguageCommand(ComboBox comboBox)
        {
            if(MessageBox.Show($"Are you sure you want to add {comboBox.Text}?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                if(language.Exists(x => x.LanguageKey == comboBox.Text))
                {
                    MessageBox.Show($"{comboBox.Text} exists.");
                }
                else
                {
                    language.Add(new LanguageModel { LanguageKey = comboBox.Text });
                    updateLanguageView();
                    comboBox.Text = string.Empty;
                }
            }
        }

        private void deleteLanguageCommand(LanguageModel languageModel)
        {
            if(MessageBox.Show($"Are you sure you want to delete {languageModel.LanguageKey}?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                language.Remove(languageModel);
                updateLanguageView();
            }
        }

        private void saveLanguageCommand(Window window)
        {
            if(MessageBox.Show($"Are you sure you want to save.", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                if(LanguageService.SerializeLanguage(language))
                {
                    window.Hide();
                }
            }
        }

        #endregion PrivateCommandMethods

        #region PrivateMessengerMethods

        private void initializeLanguage(LanguageInitializeModel languageInitializeModel)
        {
            language = LanguageService.DeserializeLanguage();

            updateLanguageView();
        }

        private void updateLanguage(LanguageModel languageModel = null)
        {
            language.ForEach(x =>
            {
                x.LanguageName = x.LanguageKey;
            });
        }

        #endregion PrivateMessengerMethods

        #region PrivateMethods

        private void updateLanguageView()
        {
            updateLanguage();

            Language = null;
            Language = CollectionViewSource.GetDefaultView(language);
            Language.SortDescriptions.Add(new SortDescription(nameof(LanguageModel.LanguageKey),
                ListSortDirection.Ascending));
        }

        #endregion PrivateMethods
    }
}