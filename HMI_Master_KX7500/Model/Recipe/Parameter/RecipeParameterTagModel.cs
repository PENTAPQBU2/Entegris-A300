using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Recipe.Parameter
{
    public class RecipeParameterTagModel : ViewModelBase
    {
        #region PublicProperties
        private string recipeParameterTag = string.Empty;
        public string RecipeParameterTag
        {
            get { return recipeParameterTag; }
            set
            {
                if (recipeParameterTag != value)
                {
                    recipeParameterTag = value;
                    RaisePropertyChanged(nameof(RecipeParameterTag));
                }
            }
        }
        #endregion
    }
}