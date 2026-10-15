using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.ShellRecipe
{
    /// <summary>
    /// A300 recipe (MT_001): a label for a shell ID.
    /// Recipe 1 = shell ID 1 (e.g. "Black"), recipe 2 = shell ID 2 (e.g. "Transparent") ...
    /// The ID is sent to PLC (PLCTag.Recipe_ID) on New Lot.
    /// </summary>
    public class ShellRecipeModel : ViewModelBase
    {
        #region PublicProperties
        private int recipeID = 0;
        public int RecipeID
        {
            get { return recipeID; }
            set
            {
                if (recipeID != value)
                {
                    recipeID = value;
                    RaisePropertyChanged(nameof(RecipeID));
                    RaisePropertyChanged(nameof(DisplayName));
                }
            }
        }

        private string recipeName = string.Empty;
        public string RecipeName
        {
            get { return recipeName; }
            set
            {
                if (recipeName != value)
                {
                    recipeName = value;
                    RaisePropertyChanged(nameof(RecipeName));
                    RaisePropertyChanged(nameof(DisplayName));
                }
            }
        }

        [XmlIgnore]
        public string DisplayName
        {
            get { return $"{RecipeID} - {RecipeName}"; }
        }
        #endregion
    }
}
