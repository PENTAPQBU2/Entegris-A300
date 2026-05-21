using HMI_Master_KX7500.ModulesView.Language;
using System.Windows;

namespace HMI_Master_KX7500.Modules.Language
{
    /// <summary>
    /// Interaction logic for LanguageEditorView.xaml
    /// </summary>
    public partial class LanguageEditorView : Window
    {
        public LanguageEditorView()
        {
            InitializeComponent();
            DataContext = new LanguageEditorViewModel();
        }
    }
}