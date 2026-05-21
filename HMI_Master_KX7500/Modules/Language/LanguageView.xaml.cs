using HMI_Master_KX7500.ModulesView.Language;
using System.Windows.Controls;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for LanguageView.xaml
    /// </summary>
    public partial class LanguageView : UserControl
    {
        public LanguageView()
        {
            InitializeComponent();
            this.DataContext = new Language_OPCUA_ViewModel();
        }
    }
}