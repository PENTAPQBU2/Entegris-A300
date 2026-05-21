using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.OEE.Data.Time
{
    public class OEETimeTagModel : ViewModelBase
    {
        #region PublicProperties

        private string oEETimeTag = string.Empty;

        public string OEETimeTag
        {
            get { return oEETimeTag; }
            set
            {
                if(oEETimeTag != value)
                {
                    oEETimeTag = value;
                    RaisePropertyChanged(nameof(OEETimeTag));
                }
            }
        }

        private string oEETimeTagType = string.Empty;

        public string OEETimeTagType
        {
            get { return oEETimeTagType; }
            set
            {
                if(oEETimeTagType != value)
                {
                    oEETimeTagType = value;
                    RaisePropertyChanged(nameof(OEETimeTagType));
                }
            }
        }

        #endregion PublicProperties
    }
}