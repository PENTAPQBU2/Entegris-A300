using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Recipe.Parameter.Boolean
{
    public class RecipeParameterBooleanModel : ViewModelBase
    {
        #region PublicProperties
        private string recipeParameterBooleanKey = string.Empty;
        public string RecipeParameterBooleanKey
        {
            get { return recipeParameterBooleanKey; }
            set
            {
                if (recipeParameterBooleanKey != value)
                {
                    recipeParameterBooleanKey = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanKey));
                }
            }
        }

        private string recipeParameterBooleanName = string.Empty;
        [XmlIgnore]
        public string RecipeParameterBooleanName
        {
            get { return recipeParameterBooleanName; }
            set
            {
                if (recipeParameterBooleanName != value)
                {
                    recipeParameterBooleanName = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanName));
                }
            }
        }

        private string recipeParameterBooleanOffIcon = string.Empty;
        public string RecipeParameterBooleanOffIcon
        {
            get { return recipeParameterBooleanOffIcon; }
            set
            {
                if (recipeParameterBooleanOffIcon != value)
                {
                    recipeParameterBooleanOffIcon = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanOffIcon));
                }
            }
        }

        private string recipeParameterBooleanOffLabelKey = string.Empty;
        public string RecipeParameterBooleanOffLabelKey
        {
            get { return recipeParameterBooleanOffLabelKey; }
            set
            {
                if (recipeParameterBooleanOffLabelKey != value)
                {
                    recipeParameterBooleanOffLabelKey = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanOffLabelKey));
                }
            }
        }

        private string recipeParameterBooleanOffLabelName = string.Empty;
        [XmlIgnore]
        public string RecipeParameterBooleanOffLabelName
        {
            get { return recipeParameterBooleanOffLabelName; }
            set
            {
                if (recipeParameterBooleanOffLabelName != value)
                {
                    recipeParameterBooleanOffLabelName = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanOffLabelName));
                }
            }
        }

        private string recipeParameterBooleanOnIcon = string.Empty;
        public string RecipeParameterBooleanOnIcon
        {
            get { return recipeParameterBooleanOnIcon; }
            set
            {
                if (recipeParameterBooleanOnIcon != value)
                {
                    recipeParameterBooleanOnIcon = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanOnIcon));
                }
            }
        }

        private string recipeParameterBooleanOnLabelKey = string.Empty;
        public string RecipeParameterBooleanOnLabelKey
        {
            get { return recipeParameterBooleanOnLabelKey; }
            set
            {
                if (recipeParameterBooleanOnLabelKey != value)
                {
                    recipeParameterBooleanOnLabelKey = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanOnLabelKey));
                }
            }
        }

        //private string recipeParameterBoolean = string.Empty;
        private string recipeParameterBooleanValue = "False";
        [XmlIgnore]
        public string RecipeParameterBooleanValue
        {
            get { return recipeParameterBooleanValue; }
            set
            {
                if (recipeParameterBooleanValue != value)
                {
                    recipeParameterBooleanValue = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanValue));
                }
            }
        }

        private RecipeParameterBooleanTagModel recipeParameterBooleanTagModel = new RecipeParameterBooleanTagModel();
        public RecipeParameterBooleanTagModel RecipeParameterBooleanTagModel
        {
            get { return recipeParameterBooleanTagModel; }
            set
            {
                if (recipeParameterBooleanTagModel != value)
                {
                    recipeParameterBooleanTagModel = value;
                    RaisePropertyChanged(nameof(RecipeParameterBooleanTagModel));
                }
            }
        }
        #endregion
    }
}
