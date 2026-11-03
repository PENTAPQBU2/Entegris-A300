using System.Windows;

namespace HMI_Master_KX7500.Modules.Frame
{
    /// <summary>
    /// Confirm pop-up with own button texts (e.g. Proceed / Cancel), used for Hard Reset (MT_006) and Initialize (MT_005)
    /// </summary>
    public partial class ConfirmView : Window
    {
        #region PublicProperties
        // While open the PLC pop-up (PopUpView, also Topmost) does not activate itself over it
        public static bool IsOpen { get; private set; }
        #endregion

        #region Constructor
        public ConfirmView(string title, string message, string yesText, string noText)
        {
            InitializeComponent();
            Title = title;
            TextBlockMessage.Text = message;
            ButtonYes.Content = yesText;
            ButtonNo.Content = noText;
        }
        #endregion

        #region PublicMethods
        // true = yes button pressed, false = no button or window closed
        public static bool Ask(Window owner, string title, string message, string yesText, string noText)
        {
            ConfirmView view = new ConfirmView(title, message, yesText, noText);
            if (owner != null && owner.IsVisible)
            {
                view.Owner = owner;
            }
            IsOpen = true;
            try
            {
                return view.ShowDialog() == true;
            }
            finally { IsOpen = false; }
        }
        #endregion

        #region PrivateEvents
        // Bottom of the screen, below the PLC pop-up (centre), so both stay visible
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            Rect workArea = SystemParameters.WorkArea;
            Left = workArea.Left + (workArea.Width - ActualWidth) / 2;
            Top = workArea.Bottom - ActualHeight - 20;
            Activate();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        #endregion
    }
}
