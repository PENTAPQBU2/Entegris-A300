using HMI_Master_KX7500.Model.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HMI_Master_KX7500.Modules.IO
{
    /// <summary>
    /// Interaction logic for IOView_.xaml
    /// </summary>
    public partial class IOView_ : UserControl
    {
        public IOView_()
        {
            InitializeComponent();

            DataContext = new IO_OPCUA_ViewModel();
        }
    }
}
