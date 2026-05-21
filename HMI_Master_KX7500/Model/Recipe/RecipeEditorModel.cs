using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.Recipe.Parameter;
using System.Collections.Generic;

namespace HMI_Master_KX7500.Model.Recipe
{
    public class RecipeEditorModel : ViewModelBase
    {
        #region PublicProperties
        private List<RecipeParameterModel> recipeParameterList = new List<RecipeParameterModel>();
        public List<RecipeParameterModel> RecipeParameterList
        {
            get { return recipeParameterList; }
            set
            {
                if (recipeParameterList != value)
                {
                    recipeParameterList = value;
                    RaisePropertyChanged(nameof(RecipeParameterList));
                    //RaisePropertyChanged(nameof(RecipeParameterListView));
                }
            }
        }
        #endregion
    }
}