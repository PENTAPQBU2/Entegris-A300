using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.OEE.HourlyUPH
{
    public class OEEHourlyUphModel : ViewModelBase
    {
        #region PublicProperties

        private string oEEHourlyUphKey = string.Empty;

        public string OEEHourlyUphKey
        {
            get { return oEEHourlyUphKey; }
            set
            {
                if(oEEHourlyUphKey != value)
                {
                    oEEHourlyUphKey = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphKey));
                }
            }
        }

        private string oEEHourlyUphName = string.Empty;

        [XmlIgnore]
        public string OEEHourlyUphName
        {
            get { return oEEHourlyUphName; }
            set
            {
                if(oEEHourlyUphName != value)
                {
                    oEEHourlyUphName = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphName));
                }
            }
        }

        private double oEEHourlyUph = 0;

        [XmlIgnore]
        public double OEEHourlyUph
        {
            get { return oEEHourlyUph; }
            set
            {
                if(oEEHourlyUph != value)
                {
                    oEEHourlyUph = value;
                    RaisePropertyChanged(nameof(OEEHourlyUph));
                    Messenger.Default.Send(OEEUpdate.Uph);
                }
            }
        }

        private OEEHourlyUphTagModel oEEHourlyUphTagModel = new OEEHourlyUphTagModel();

        public OEEHourlyUphTagModel OEEHourlyUphTagModel
        {
            get { return oEEHourlyUphTagModel; }
            set
            {
                if(oEEHourlyUphTagModel != value)
                {
                    oEEHourlyUphTagModel = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphTagModel));
                }
            }
        }

        #endregion PublicProperties
    }
}