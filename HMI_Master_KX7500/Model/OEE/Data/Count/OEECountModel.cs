using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.OEE.Data.Count
{
    public class OEECountModel : ViewModelBase
    {
        #region PublicProperties

        private string oEECountKey = string.Empty;

        public string OEECountKey
        {
            get { return oEECountKey; }
            set
            {
                if(oEECountKey != value)
                {
                    oEECountKey = value;
                    RaisePropertyChanged(nameof(OEECountKey));
                }
            }
        }

        private string oEECountName = string.Empty;

        [XmlIgnore]
        public string OEECountName
        {
            get { return oEECountName; }
            set
            {
                if(oEECountName != value)
                {
                    oEECountName = value;
                    RaisePropertyChanged(nameof(OEECountName));
                }
            }
        }

        private double oEEMinimumCount = 0;

        public double OEEMinimumCount
        {
            get { return oEEMinimumCount; }
            set
            {
                if(oEEMinimumCount != value)
                {
                    oEEMinimumCount = value;
                    RaisePropertyChanged(nameof(OEEMinimumCount));
                }
            }
        }

        private double oEEMaximumCount = 0;

        public double OEEMaximumCount
        {
            get { return oEEMaximumCount; }
            set
            {
                if(oEEMaximumCount != value)
                {
                    oEEMaximumCount = value;
                    RaisePropertyChanged(nameof(OEEMaximumCount));
                }
            }
        }

        private int oEECountDecimalPlaces = 0;

        public int OEECountDecimalPlaces
        {
            get { return oEECountDecimalPlaces; }
            set
            {
                if(oEECountDecimalPlaces != value)
                {
                    oEECountDecimalPlaces = value;
                    RaisePropertyChanged(nameof(OEECountDecimalPlaces));
                }
            }
        }

        private double oEECount = 0;

        [XmlIgnore]
        public double OEECount
        {
            get { return oEECount; }
            set
            {
                value = Math.Round(value, OEECountDecimalPlaces);

                if(oEECount != value)
                {
                    oEECount = value;
                    RaisePropertyChanged(nameof(OEECount));
                }
            }
        }

        private OEECountTagModel oEECountTagModel = new OEECountTagModel();

        public OEECountTagModel OEECountTagModel
        {
            get { return oEECountTagModel; }
            set
            {
                if(oEECountTagModel != value)
                {
                    oEECountTagModel = value;
                    RaisePropertyChanged(nameof(OEECountTagModel));
                }
            }
        }

        #endregion PublicProperties
    }
}