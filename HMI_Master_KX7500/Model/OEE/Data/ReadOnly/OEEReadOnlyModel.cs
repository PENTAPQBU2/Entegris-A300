using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.OEE.Data.ReadOnly
{
    public class OEEReadOnlyModel : ViewModelBase
    {
        #region PublicProperties

        private string oEEReadOnlyKey = string.Empty;

        public string OEEReadOnlyKey
        {
            get { return oEEReadOnlyKey; }
            set
            {
                if(oEEReadOnlyKey != value)
                {
                    oEEReadOnlyKey = value;
                    RaisePropertyChanged(nameof(OEEReadOnlyKey));
                }
            }
        }

        private string oEEReadOnlyName = string.Empty;

        [XmlIgnore]
        public string OEEReadOnlyName
        {
            get { return oEEReadOnlyName; }
            set
            {
                if(oEEReadOnlyName != value)
                {
                    oEEReadOnlyName = value;
                    RaisePropertyChanged(nameof(OEEReadOnlyName));
                }
            }
        }

        private string oEEReadOnly = string.Empty;

        [XmlIgnore]
        public string OEEReadOnly
        {
            get { return oEEReadOnly; }
            set
            {
                if(oEEReadOnly != value)
                {
                    oEEReadOnly = value;
                    RaisePropertyChanged(nameof(OEEReadOnly));
                }
            }
        }

        private OEEReadOnlyTagModel oEEReadOnlyTagModel = new OEEReadOnlyTagModel();

        public OEEReadOnlyTagModel OEEReadOnlyTagModel
        {
            get { return oEEReadOnlyTagModel; }
            set
            {
                if(oEEReadOnlyTagModel != value)
                {
                    oEEReadOnlyTagModel = value;
                    RaisePropertyChanged(nameof(OEEReadOnlyTagModel));
                }
            }
        }

        #endregion PublicProperties
    }
}