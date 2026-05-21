using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.OEE.Data.Count
{
    public class OEECountTagModel : ViewModelBase
    {
        #region PublicProperties

        private string oEECountTag = string.Empty;

        public string OEECountTag
        {
            get { return oEECountTag; }
            set
            {
                if(oEECountTag != value)
                {
                    oEECountTag = value;
                    RaisePropertyChanged(nameof(OEECountTag));
                }
            }
        }

        private string oEECountTagType = string.Empty;

        public string OEECountTagType
        {
            get { return oEECountTagType; }
            set
            {
                if(oEECountTagType != value)
                {
                    oEECountTagType = value;
                    RaisePropertyChanged(nameof(OEECountTagType));
                }
            }
        }

        #endregion PublicProperties
    }
}