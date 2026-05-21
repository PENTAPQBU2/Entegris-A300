using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Recipe.Parameter.Numeric
{
    public class RecipeParameterNumericModel : ViewModelBase
    {
        #region PublicProperties
        private string recipeParameterNumericKey = string.Empty;
        public string RecipeParameterNumericKey
        {
            get { return recipeParameterNumericKey; }
            set
            {
                if (recipeParameterNumericKey != value)
                {
                    recipeParameterNumericKey = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericKey));
                }
            }
        }

        private string recipeParameterNumericName = string.Empty;
        [XmlIgnore]
        public string RecipeParameterNumericName
        {
            get { return recipeParameterNumericName; }
            set
            {
                if (recipeParameterNumericName != value)
                {
                    recipeParameterNumericName = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericName));
                }
            }
        }

        private double recipeParameterNumericMaximum = 0;
        public double RecipeParameterNumericMaximum
        {
            get { return recipeParameterNumericMaximum; }
            set
            {
                if (recipeParameterNumericMaximum != value)
                {
                    recipeParameterNumericMaximum = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericMaximum));
                }
            }
        }

        private double recipeParameterNumericMinimum = 0;
        public double RecipeParameterNumericMinimum
        {
            get { return recipeParameterNumericMinimum; }
            set
            {
                if (recipeParameterNumericMinimum != value)
                {
                    recipeParameterNumericMinimum = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericMinimum));
                }
            }
        }

        private double recipeParameterNumericValue = 0;
        [XmlIgnore]
        public double RecipeParameterNumericValue
        {
            get { return recipeParameterNumericValue; }
            set
            {
                //value = Convert.ToInt32(Math.Round(Convert.ToDouble(value), recipeParameterDecimalPlace));

                if (recipeParameterNumericValue != value)
                {
                    recipeParameterNumericValue = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericValue));
                }
            }
        }

        private RecipeParameterNumericTagModel recipeParameterNumericTagModel = new RecipeParameterNumericTagModel();
        public RecipeParameterNumericTagModel RecipeParameterNumericTagModel
        {
            get { return recipeParameterNumericTagModel; }
            set
            {
                if (recipeParameterNumericTagModel != value)
                {
                    recipeParameterNumericTagModel = value;
                    RaisePropertyChanged(nameof(RecipeParameterNumericTagModel));
                }
            }
        }

        private int recipeParameterDecimalPlace = 0;
        public int RecipeParameterDecimalPlace
        {
            get { return recipeParameterDecimalPlace; }
            set
            {
                if (recipeParameterDecimalPlace != value)
                {
                    recipeParameterDecimalPlace = value;

                    SettingGeneralGroupDecimalPlacesString = "0";
                    for (int i = 0; i < recipeParameterDecimalPlace; i++)
                    {
                        if (i == 0)
                            SettingGeneralGroupDecimalPlacesString += ".";
                        SettingGeneralGroupDecimalPlacesString += "0";
                    }

                    RaisePropertyChanged(nameof(RecipeParameterDecimalPlace));
                }
            }
        }

        private string settingGeneralGroupDecimalPlacesString = "0";
        public string SettingGeneralGroupDecimalPlacesString
        {
            get { return settingGeneralGroupDecimalPlacesString; }
            set
            {
                if (settingGeneralGroupDecimalPlacesString != value)
                {
                    settingGeneralGroupDecimalPlacesString = value;
                    RaisePropertyChanged(nameof(SettingGeneralGroupDecimalPlacesString));
                }
            }
        }
        #endregion
    }
}
