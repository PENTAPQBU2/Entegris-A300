using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.OEE.HourlyUPH
{
    public class OEEHourlyUphTagModel : ViewModelBase
    {
        #region PublicProperties

        private string oEEHourlyUphTag = string.Empty;

        public string OEEHourlyUphTag
        {
            get { return oEEHourlyUphTag; }
            set
            {
                if(oEEHourlyUphTag != value)
                {
                    oEEHourlyUphTag = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphTag));
                }
            }
        }

        private string oEEHourlyUphTagType = string.Empty;

        public string OEEHourlyUphTagType
        {
            get { return oEEHourlyUphTagType; }
            set
            {
                if(oEEHourlyUphTagType != value)
                {
                    oEEHourlyUphTagType = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphTagType));
                }
            }
        }

        #endregion PublicProperties
    }
}