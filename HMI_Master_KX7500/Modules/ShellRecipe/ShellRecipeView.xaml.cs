using HMI_Master_KX7500.ModulesView.ShellRecipe;
using System.Windows;
using System.Windows.Controls;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for ShellRecipeView.xaml (Recipe page, MT_001)
    /// </summary>
    public partial class ShellRecipeView : UserControl
    {
        #region Constructor
        public ShellRecipeView()
        {
            InitializeComponent();
            DataContext = new ShellRecipe_ViewModel();
        }
        #endregion

        #region PrivateEvents
        // reload recipe list each time the page is opened from the menu
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ShellRecipe_ViewModel)?.Refresh();
        }
        #endregion
    }
}
