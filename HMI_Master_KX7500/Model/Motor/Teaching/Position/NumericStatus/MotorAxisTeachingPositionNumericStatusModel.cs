using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionNumericStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionNumericStatusKey = string.Empty;
        public string AxisTeachingPositionNumericStatusKey
        {
            get { return axisTeachingPositionNumericStatusKey; }
            set
            {
                if (axisTeachingPositionNumericStatusKey != value)
                {
                    axisTeachingPositionNumericStatusKey = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusKey));
                }
            }
        }

        private string axisTeachingPositionNumericStatusName = string.Empty;
        [XmlIgnore]
        public string AxisTeachingPositionNumericStatusName
        {
            get { return axisTeachingPositionNumericStatusName; }
            set
            {
                if (axisTeachingPositionNumericStatusName != value)
                {
                    axisTeachingPositionNumericStatusName = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusName));
                }
            }
        }

        private int axisTeachingPositionNumericStatusDecimalPlaces = 0;
        public int AxisTeachingPositionNumericStatusDecimalPlaces
        {
            get { return axisTeachingPositionNumericStatusDecimalPlaces; }
            set
            {
                if (axisTeachingPositionNumericStatusDecimalPlaces != value)
                {
                    axisTeachingPositionNumericStatusDecimalPlaces = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusDecimalPlaces));
                }
            }
        }

        private double axisTeachingPositionNumericStatus = 0;
        [XmlIgnore]
        public double AxisTeachingPositionNumericStatus
        {
            get { return axisTeachingPositionNumericStatus; }
            set
            {
                value = Math.Round(value, AxisTeachingPositionNumericStatusDecimalPlaces);

                if (axisTeachingPositionNumericStatus != value)
                {
                    axisTeachingPositionNumericStatus = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatus));
                }
            }
        }

        private MotorAxisTeachingPositionNumericStatusTagModel axisTeachingPositionNumericStatusTagModel = new MotorAxisTeachingPositionNumericStatusTagModel();
        public MotorAxisTeachingPositionNumericStatusTagModel AxisTeachingPositionNumericStatusTagModel
        {
            get { return axisTeachingPositionNumericStatusTagModel; }
            set
            {
                if (axisTeachingPositionNumericStatusTagModel != value)
                {
                    axisTeachingPositionNumericStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusTagModel));
                }
            }
        }
        #endregion
    }
}