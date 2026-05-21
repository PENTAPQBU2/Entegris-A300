using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.OEE.Data.Time
{
    public class OEETimeModel : ViewModelBase
    {
        #region PublicProperties

        private string oEETimeKey = string.Empty;

        public string OEETimeKey
        {
            get { return oEETimeKey; }
            set
            {
                if(oEETimeKey != value)
                {
                    oEETimeKey = value;
                    RaisePropertyChanged(nameof(OEETimeKey));
                }
            }
        }

        private string oEETimeName = string.Empty;

        [XmlIgnore]
        public string OEETimeName
        {
            get { return oEETimeName; }
            set
            {
                if(oEETimeName != value)
                {
                    oEETimeName = value;
                    RaisePropertyChanged(nameof(OEETimeName));
                }
            }
        }

        private string oEETime = string.Empty;

        [XmlIgnore]
        public string OEETime
        {
            get { return oEETime; }
            set
            {
                if(oEETime != value)
                {
                    oEETime = value;
                    RaisePropertyChanged(nameof(OEETime));
                }
            }
        }

        private OEETimeTagModel oEETimeTagModel = new OEETimeTagModel();

        public OEETimeTagModel OEETimeTagModel
        {
            get { return oEETimeTagModel; }
            set
            {
                if(oEETimeTagModel != value)
                {
                    oEETimeTagModel = value;
                    RaisePropertyChanged(nameof(OEETimeTagModel));
                }
            }
        }

        #endregion PublicProperties
    }
}