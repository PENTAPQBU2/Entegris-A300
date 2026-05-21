using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeNumericStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string cycleTimeNumericStatusKey = string.Empty;
        public string CycleTimeNumericStatusKey
        {
            get { return cycleTimeNumericStatusKey; }
            set
            {
                if (cycleTimeNumericStatusKey != value)
                {
                    cycleTimeNumericStatusKey = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusKey));
                }
            }
        }

        private string cycleTimeNumericStatusName = string.Empty;
        [XmlIgnore]
        public string CycleTimeNumericStatusName
        {
            get { return cycleTimeNumericStatusName; }
            set
            {
                if (cycleTimeNumericStatusName != value)
                {
                    cycleTimeNumericStatusName = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusName));
                }
            }
        }

        private int cycleTimeNumericStatusDecimalPlaces = 0;
        public int CycleTimeNumericStatusDecimalPlaces
        {
            get { return cycleTimeNumericStatusDecimalPlaces; }
            set
            {
                if (cycleTimeNumericStatusDecimalPlaces != value)
                {
                    cycleTimeNumericStatusDecimalPlaces = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusDecimalPlaces));
                }
            }
        }

        private double cycleTimeNumericStatus = 0;
        [XmlIgnore]
        public double CycleTimeNumericStatus
        {
            get { return cycleTimeNumericStatus; }
            set
            {
                value = Math.Round(value, CycleTimeNumericStatusDecimalPlaces);

                if (cycleTimeNumericStatus != value)
                {
                    cycleTimeNumericStatus = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatus));
                }
            }
        }

        private CycleTimeNumericStatusTagModel cycleTimeNumericStatusTagModel = new CycleTimeNumericStatusTagModel();
        public CycleTimeNumericStatusTagModel CycleTimeNumericStatusTagModel
        {
            get { return cycleTimeNumericStatusTagModel; }
            set
            {
                if (cycleTimeNumericStatusTagModel != value)
                {
                    cycleTimeNumericStatusTagModel = value;
                    RaisePropertyChanged(nameof(CycleTimeNumericStatusTagModel));
                }
            }
        }
        #endregion
    }
}