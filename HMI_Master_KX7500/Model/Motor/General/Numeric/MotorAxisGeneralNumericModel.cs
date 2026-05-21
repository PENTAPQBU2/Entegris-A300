using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralNumericModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SetAxisGeneralNumericCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisGeneralNumericKey = string.Empty;
        public string AxisGeneralNumericKey
        {
            get { return axisGeneralNumericKey; }
            set
            {
                if (axisGeneralNumericKey != value)
                {
                    axisGeneralNumericKey = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericKey));
                }
            }
        }

        private string axisGeneralNumericName = string.Empty;
        [XmlIgnore]
        public string AxisGeneralNumericName
        {
            get { return axisGeneralNumericName; }
            set
            {
                if (axisGeneralNumericName != value)
                {
                    axisGeneralNumericName = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericName));
                }
            }
        }

        private double axisGeneralNumericMaximum = 0;
        public double AxisGeneralNumericMaximum
        {
            get { return axisGeneralNumericMaximum; }
            set
            {
                if (axisGeneralNumericMaximum != value)
                {
                    axisGeneralNumericMaximum = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericMaximum));
                }
            }
        }

        private double axisGeneralNumericMinimum = 0;
        public double AxisGeneralNumericMinimum
        {
            get { return axisGeneralNumericMinimum; }
            set
            {
                if (axisGeneralNumericMinimum != value)
                {
                    axisGeneralNumericMinimum = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericMinimum));
                }
            }
        }

        private int axisGeneralNumericDecimalPlaces = 0;
        public int AxisGeneralNumericDecimalPlaces
        {
            get { return axisGeneralNumericDecimalPlaces; }
            set
            {
                if (axisGeneralNumericDecimalPlaces != value)
                {
                    axisGeneralNumericDecimalPlaces = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericDecimalPlaces));
                }
            }
        }

        private double axisGeneralNumeric = 0;
        [XmlIgnore]
        public double AxisGeneralNumeric
        {
            get { return axisGeneralNumeric; }
            set
            {
                value = Math.Round(value, AxisGeneralNumericDecimalPlaces);

                if (axisGeneralNumeric != value)
                {
                    axisGeneralNumeric = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumeric));
                }
            }
        }

        private MotorAxisGeneralNumericTagModel axisGeneralNumericTagModel = new MotorAxisGeneralNumericTagModel();
        public MotorAxisGeneralNumericTagModel AxisGeneralNumericTagModel
        {
            get { return axisGeneralNumericTagModel; }
            set
            {
                if (axisGeneralNumericTagModel != value)
                {
                    axisGeneralNumericTagModel = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericTagModel));
                }
            }
        }
        #endregion
    }
}