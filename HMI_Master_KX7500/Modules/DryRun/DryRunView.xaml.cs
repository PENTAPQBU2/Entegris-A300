using HMI_Master_KX7500.Model.DryRun;
using System.Windows.Controls;

namespace HMI_Master_KX7500.Modules.DryRun
{
    /// <summary>
    /// Interaction logic for DryRunView.xaml
    /// </summary>
    public partial class DryRunView : UserControl
    {
        public DryRunView()
        {
            InitializeComponent();

            DataContext = new DryRunViewModel();
        }
    }
}
