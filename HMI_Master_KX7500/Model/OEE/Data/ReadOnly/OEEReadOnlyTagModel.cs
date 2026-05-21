using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.OEE.Data.ReadOnly
{
    public class OEEReadOnlyTagModel : ViewModelBase
    {
        #region PublicProperties

        private string oEEReadOnlyTag = string.Empty;

        public string OEEReadOnlyTag
        {
            get { return oEEReadOnlyTag; }
            set
            {
                if(oEEReadOnlyTag != value)
                {
                    oEEReadOnlyTag = value;
                    RaisePropertyChanged(nameof(OEEReadOnlyTag));
                }
            }
        }

        private string oEEReadOnlyTagType = string.Empty;

        public string OEEReadOnlyTagType
        {
            get { return oEEReadOnlyTagType; }
            set
            {
                if(oEEReadOnlyTagType != value)
                {
                    oEEReadOnlyTagType = value;
                    RaisePropertyChanged(nameof(OEEReadOnlyTagType));
                }
            }
        }

        #endregion PublicProperties
    }
}