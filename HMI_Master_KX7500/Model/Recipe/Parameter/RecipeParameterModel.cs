using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.Recipe.Parameter.Boolean;
using HMI_Master_KX7500.Model.Recipe.Parameter.Numeric;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Recipe.Parameter
{
    public class RecipeParameterModel : ViewModelBase
    {
        #region PublicProperties
        private string recipeParameterKey = string.Empty;
        public string RecipeParameterKey
        {
            get { return recipeParameterKey; }
            set
            {
                if (recipeParameterKey != value)
                {
                    recipeParameterKey = value;
                    RaisePropertyChanged(nameof(RecipeParameterKey));
                }
            }
        }

        private string recipeParameterName = string.Empty;
        [XmlIgnore]
        public string RecipeParameterName
        {
            get { return recipeParameterName; }
            set
            {
                if (recipeParameterName != value)
                {
                    recipeParameterName = value;
                    RaisePropertyChanged(nameof(RecipeParameterName));
                }
            }
        }

        private string recipeParameter = string.Empty;
        public string RecipeParameter
        {
            get { return recipeParameter; }
            set
            {
                if (recipeParameter != value)
                {
                    recipeParameter = value;
                    RaisePropertyChanged(nameof(RecipeParameter));
                }
            }
        }

        private RecipeParameterTagModel recipeParameterTagModel = new RecipeParameterTagModel();
        public RecipeParameterTagModel RecipeParameterTagModel
        {
            get { return recipeParameterTagModel; }
            set
            {
                if (recipeParameterTagModel != value)
                {
                    recipeParameterTagModel = value;
                    RaisePropertyChanged(nameof(RecipeParameterTagModel));
                }
            }
        }

        //private List<RecipeParameterNumericModel> recipeParameterNumericList = new List<RecipeParameterNumericModel>();
        //public List<RecipeParameterNumericModel> RecipeParameterNumericList
        //{
        //    get { return recipeParameterNumericList; }
        //    set
        //    {
        //        if (recipeParameterNumericList != value)
        //        {
        //            recipeParameterNumericList = value;
        //            RaisePropertyChanged(nameof(RecipeParameterNumericList));
        //        }
        //    }
        //}

        //private List<RecipeParameterBooleanModel> recipeParameterBooleanList = new List<RecipeParameterBooleanModel>();
        //public List<RecipeParameterBooleanModel> RecipeParameterBooleanList
        //{
        //    get { return recipeParameterBooleanList; }
        //    set
        //    {
        //        if (recipeParameterBooleanList != value)
        //        {
        //            recipeParameterBooleanList = value;
        //            RaisePropertyChanged(nameof(RecipeParameterBooleanList));
        //        }
        //    }
        //}
        #endregion
    }
}
