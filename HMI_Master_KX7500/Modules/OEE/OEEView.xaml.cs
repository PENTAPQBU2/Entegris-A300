using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Model.OEE;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for OEEView.xaml
    /// </summary>
    public partial class OEEView : UserControl
    {
        public OEEView()
        {
            InitializeComponent();

            DataContext = new OEE_ViewModel();
        }
    }
}