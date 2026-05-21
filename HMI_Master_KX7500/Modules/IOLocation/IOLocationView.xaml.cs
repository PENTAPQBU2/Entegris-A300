using NLog;
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

namespace HMI_Master_KX7500.Modules.IOLocation
{
    /// <summary>
    /// Interaction logic for IOLocationView.xaml
    /// </summary>
    public partial class IOLocationView : UserControl
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor
        public IOLocationView()
        {
            InitializeComponent();
        } 
        #endregion

        #region PrivateEvents
        private void ioLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TabControl_IOLocation.SelectedIndex = Convert.ToInt32(((Button)sender).Tag);
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

    }
}
