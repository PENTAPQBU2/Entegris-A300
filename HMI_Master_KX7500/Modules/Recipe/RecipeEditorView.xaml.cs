using HMI_Master_KX7500.ModulesView.Recipe;
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
using System.Windows.Shapes;

namespace HMI_Master_KX7500.Modules.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeEditorView.xaml
    /// </summary>
    public partial class RecipeEditorView : Window
    {
        public RecipeEditorView()
        {
            InitializeComponent();
            DataContext = new RecipeEditorViewModel(this);
        }
    }
}
