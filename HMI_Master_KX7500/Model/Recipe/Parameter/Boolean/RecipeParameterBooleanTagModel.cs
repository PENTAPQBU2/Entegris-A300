using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Recipe.Parameter.Boolean
{
    public class RecipeParameterBooleanTagModel : ViewModelBase
    {
        #region PublicProperties
        private string recipeParameterBooleanTag = string.Empty;
        public string RecipeParameterBooleanTag
        {
            get { return recipeParameterBooleanTag; }
            set
            {
                if (recipeParameterBooleanTag != value)
                {
                    recipeParameterBooleanTag = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanTag));
                }
            }
        }
        #endregion
    }
}
