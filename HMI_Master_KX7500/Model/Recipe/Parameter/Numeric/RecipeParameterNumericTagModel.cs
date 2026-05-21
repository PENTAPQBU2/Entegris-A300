using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Recipe.Parameter.Numeric
{
    public class RecipeParameterNumericTagModel : ViewModelBase
    {
        #region PublicProperties
        private string recipeParameterNumericTag = string.Empty;
        public string RecipeParameterNumericTag
        {
            get { return recipeParameterNumericTag; }
            set
            {
                if (recipeParameterNumericTag != value)
                {
                    recipeParameterNumericTag = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericTag));
                }
            }
        }
        #endregion
    }
}
