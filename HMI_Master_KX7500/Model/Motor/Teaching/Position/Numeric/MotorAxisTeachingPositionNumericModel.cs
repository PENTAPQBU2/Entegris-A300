using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionNumericModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SetAxisTeachingPositionNumericCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisTeachingPositionNumericKey = string.Empty;
        public string AxisTeachingPositionNumericKey
        {
            get { return axisTeachingPositionNumericKey; }
            set
            {
                if (axisTeachingPositionNumericKey != value)
                {
                    axisTeachingPositionNumericKey = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericKey));
                }
            }
        }

        private string axisTeachingPositionNumericName = string.Empty;
        [XmlIgnore]
        public string AxisTeachingPositionNumericName
        {
            get { return axisTeachingPositionNumericName; }
            set
            {
                if (axisTeachingPositionNumericName != value)
                {
                    axisTeachingPositionNumericName = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericName));
                }
            }
        }

        private double axisTeachingPositionNumericMaximum = 0;
        public double AxisTeachingPositionNumericMaximum
        {
            get { return axisTeachingPositionNumericMaximum; }
            set
            {
                if (axisTeachingPositionNumericMaximum != value)
                {
                    axisTeachingPositionNumericMaximum = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericMaximum));
                }
            }
        }

        private double axisTeachingPositionNumericMinimum = 0;
        public double AxisTeachingPositionNumericMinimum
        {
            get { return axisTeachingPositionNumericMinimum; }
            set
            {
                if (axisTeachingPositionNumericMinimum != value)
                {
                    axisTeachingPositionNumericMinimum = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericMinimum));
                }
            }
        }

        private int axisTeachingPositionNumericDecimalPlaces = 0;
        public int AxisTeachingPositionNumericDecimalPlaces
        {
            get { return axisTeachingPositionNumericDecimalPlaces; }
            set
            {
                if (axisTeachingPositionNumericDecimalPlaces != value)
                {
                    axisTeachingPositionNumericDecimalPlaces = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericDecimalPlaces));
                }
            }
        }

        private double axisTeachingPositionNumeric = 0;
        [XmlIgnore]
        public double AxisTeachingPositionNumeric
        {
            get { return axisTeachingPositionNumeric; }
            set
            {
                value = Math.Round(value, AxisTeachingPositionNumericDecimalPlaces);

                if (axisTeachingPositionNumeric != value)
                {
                    axisTeachingPositionNumeric = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumeric));
                }
            }
        }

        private MotorAxisTeachingPositionNumericTagModel axisTeachingPositionNumericTagModel = new MotorAxisTeachingPositionNumericTagModel();
        public MotorAxisTeachingPositionNumericTagModel AxisTeachingPositionNumericTagModel
        {
            get { return axisTeachingPositionNumericTagModel; }
            set
            {
                if (axisTeachingPositionNumericTagModel != value)
                {
                    axisTeachingPositionNumericTagModel = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericTagModel));
                }
            }
        }
        #endregion
    }
}