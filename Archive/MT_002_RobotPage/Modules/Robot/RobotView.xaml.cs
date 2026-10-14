using HMI_Master_KX7500.ModulesView.Robot;
using System.Windows.Controls;
using System.Windows.Input;

namespace HMI_Master_KX7500.Modules.Robot
{
    /// <summary>
    /// Robot page (MT_002). Logic is in RobotViewModel; this file only handles the hold-to-move button.
    /// </summary>
    public partial class RobotView : UserControl
    {
        private bool moving;

        public RobotView()
        {
            InitializeComponent();
        }

        private RobotViewModel viewModel => DataContext as RobotViewModel;

        private void moveButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(viewModel == null || !MoveButton.IsEnabled)
            {
                return;
            }
            moving = true;
            MoveButton.CaptureMouse();
            viewModel.StartMove();
            e.Handled = true;
        }

        private void moveButtonUp(object sender, MouseButtonEventArgs e)
        {
            stopMove();
            MoveButton.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void moveButtonReleased(object sender, MouseEventArgs e)
        {
            stopMove();
        }

        private void stopMove()
        {
            if(!moving)
            {
                return;
            }
            moving = false;
            viewModel?.StopMove();
        }
    }
}
