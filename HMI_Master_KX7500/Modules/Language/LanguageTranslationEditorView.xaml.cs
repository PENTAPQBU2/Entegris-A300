using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.ModulesView.Language;
using System.Windows;

namespace HMI_Master_KX7500.Modules.Language
{
    /// <summary>
    /// Interaction logic for LanguageTranslationEditorView.xaml
    /// </summary>
    public partial class LanguageTranslationEditorView : Window
    {
        public LanguageTranslationEditorView()
        {
            InitializeComponent();
            DataContext = new LanguageTranslationEditorViewModel();
        }

        private void LanguageTranslationEditorWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<LanguageUpdate>(LanguageUpdate.Update);
        }
    }
}