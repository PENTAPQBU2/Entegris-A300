using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.Language.Translation;
using HMI_Master_KX7500.Model.OEE;
using HMI_Master_KX7500.Service.Language;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HMI_Master_KX7500.ModulesView.Language
{
    public class LanguageTranslationEditorViewModel : ViewModelBase
    {
        #region PrivateFields

        private List<LanguageModel> language = new List<LanguageModel>();
        private LanguageInitializeModel languageInitializeModel = null;

        #endregion PrivateFields

        #region PublicCommandProperties

        public ICommand LanguageTranslationEditorViewClosingCommand { get; set; }

        public ICommand AddLanguageTranslationKeyCommand { get; set; }

        public ICommand DeleteLanguageTranslationKeyCommand { get; set; }

        public ICommand SaveLanguageTranslationEditListCommand { get; set; }

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

        private ObservableCollection<LanguageTranslationEditModel> _languageTranslationEditList = null;

        public ObservableCollection<LanguageTranslationEditModel> LanguageTranslationEditList
        {
            get { return _languageTranslationEditList; }
            set
            {
                if(_languageTranslationEditList != value)
                {
                    _languageTranslationEditList = value;
                    RaisePropertyChanged(nameof(LanguageTranslationEditList));
                }
            }
        }

        #endregion PublicProperties

        #region Constructor

        public LanguageTranslationEditorViewModel()
        {
            initializeCommand();
            initializeMessenger();
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeCommand()
        {
            LanguageTranslationEditorViewClosingCommand = new RelayCommand<EventArgsModel>(
                    languageTranslationEditorViewClosingCommand);

            AddLanguageTranslationKeyCommand = new RelayCommand<ComboBox>(
                addLanguageTranslationKeyCommand);

            DeleteLanguageTranslationKeyCommand = new RelayCommand<LanguageTranslationEditModel>(
                deleteLanguageTranslationKeyCommand);

            SaveLanguageTranslationEditListCommand = new RelayCommand<Window>(
                saveLanguageTranslationEditListCommand);
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<LanguageInitializeModel>(this, initializeLanguage);
            Messenger.Default.Register<LanguageUpdate>(this, checkLanguage);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }

        #endregion PrivateInitializeMethods

        #region PrivateCommandMethods

        private void languageTranslationEditorViewClosingCommand(EventArgsModel eventArgsModel)
        {
            CancelEventArgs cancelEventArgs = eventArgsModel.EventArgs as CancelEventArgs;
            cancelEventArgs.Cancel = true;

            Window window = eventArgsModel.Parameter as Window;
            window.Hide();
        }

        private void addLanguageTranslationKeyCommand(ComboBox comboBox)
        {
            if(MessageBox.Show($"Are you sure you want to add {comboBox.Text}?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                if(LanguageTranslationEditList.ToList().Exists(x => x.LanguageTranslationKey == comboBox.Text))
                {
                    MessageBox.Show($"{comboBox.Text} exists.");
                }
                else
                {
                    LanguageTranslationEditModel languageTranslationEditModel = new LanguageTranslationEditModel
                    {
                        LanguageTranslationKey = comboBox.Text,
                        LanguageTranslationValueList = new ObservableCollection<LanguageTranslationModel>()
                    };

                    language.ForEach(x =>
                    {
                        languageTranslationEditModel.LanguageTranslationValueList.Add(new LanguageTranslationModel
                        {
                            LanguageTranslationValue = comboBox.Text
                        });
                    });

                    LanguageTranslationEditList.Add(languageTranslationEditModel);
                    updateLanguageTranslationEditList();
                    comboBox.Text = string.Empty;
                }
            }
        }

        private void deleteLanguageTranslationKeyCommand(LanguageTranslationEditModel languageTranslationEditModel)
        {
            if(MessageBox.Show($"Are you sure you want to delete {languageTranslationEditModel.LanguageTranslationKey}?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                LanguageTranslationEditList.Remove(languageTranslationEditModel);
                updateLanguageTranslationEditList();
            }
        }

        private void saveLanguageTranslationEditListCommand(Window window)
        {
            if(MessageBox.Show($"Are you sure you want to save.", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                if(LanguageService.SaveLanguageTranslationEditList(language, LanguageTranslationEditList.ToList()))
                {
                    window.Hide();
                }
            }
        }

        #endregion PrivateCommandMethods

        #region PrivateMessengerMethods

        private void initializeLanguage(LanguageInitializeModel _languageInitializeModel)
        {
            if(languageInitializeModel == null)
            {
                language = LanguageService.DeserializeLanguage();

                Language = null;
                Language = CollectionViewSource.GetDefaultView(language);
                Language.SortDescriptions.Add(new SortDescription(nameof(LanguageModel.LanguageKey),
                    ListSortDirection.Ascending));

                LanguageTranslationEditList = new ObservableCollection<LanguageTranslationEditModel>(LanguageService.GetLanguageTranslationEditList());
                updateLanguageTranslationEditList();
                languageInitializeModel = _languageInitializeModel;
            }
        }

        private void checkLanguage(LanguageUpdate update)
        {
            if(update == LanguageUpdate.Update)
            {
                List<LanguageModel> _language = LanguageService.DeserializeLanguage();
                bool areEquivalent = true;
                if(_language.Count() == language.Count())
                {
                    _language.ForEach(x =>
                    {
                        if(!language.Exists(y => y.LanguageKey == x.LanguageKey))
                        {
                            areEquivalent = false;
                        }
                    });
                }
                else
                {
                    areEquivalent = false;
                }

                if(!areEquivalent)
                {
                    language = _language;

                    Language = null;
                    Language = CollectionViewSource.GetDefaultView(_language);
                    Language.SortDescriptions.Add(new SortDescription(nameof(LanguageModel.LanguageKey),
                        ListSortDirection.Ascending));

                    LanguageTranslationEditList = new ObservableCollection<LanguageTranslationEditModel>(LanguageService.GetLanguageTranslationEditList());
                    updateLanguage(new LanguageModel());
                }
            }
        }

        private void updateLanguage(LanguageModel languageModel)
        {
            language.ForEach(x =>
            {
                x.LanguageName = x.LanguageKey;
            });
        }

        #endregion PrivateMessengerMethods

        #region PrivateMethods

        private void updateLanguageTranslationEditList()
        {
            //MethodHelper.Action(GetType().Name, nameof(updateLanguageTranslationEditList), () =>
            //{
            //    LanguageTranslationEditList = null;
            //    LanguageTranslationEditList = CollectionViewSource.GetDefaultView(languageTranslationEditList);
            //    LanguageTranslationEditList.SortDescriptions.Add(new SortDescription(
            //        nameof(LanguageTranslationEditModel.LanguageTranslationKey), ListSortDirection.Ascending));
            //});
        }

        #endregion PrivateMethods
    }
}