using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.OEE
{
    public class OEEDataTagModel : ViewModelBase
    {
        #region PublicProperties

        private string oEEGenerateCsvTag = string.Empty;

        public string OEEGenerateCsvTag
        {
            get { return oEEGenerateCsvTag; }
            set
            {
                if(oEEGenerateCsvTag != value)
                {
                    oEEGenerateCsvTag = value;
                    RaisePropertyChanged(nameof(OEEGenerateCsvTag));
                }
            }
        }

        private string oEEGenerateCsvTagType = string.Empty;

        public string OEEGenerateCsvTagType
        {
            get { return oEEGenerateCsvTagType; }
            set
            {
                if(oEEGenerateCsvTagType != value)
                {
                    oEEGenerateCsvTagType = value;
                    RaisePropertyChanged(nameof(OEEGenerateCsvTagType));
                }
            }
        }

        #endregion PublicProperties
    }
}