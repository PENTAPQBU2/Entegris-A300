using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisPositionNumericStatusModel : ViewModelBase
    {
        #region PublicProperties
        private string axisPositionNumericStatusKey = string.Empty;
        public string AxisPositionNumericStatusKey
        {
            get { return axisPositionNumericStatusKey; }
            set
            {
                if (axisPositionNumericStatusKey != value)
                {
                    axisPositionNumericStatusKey = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusKey));
                }
            }
        }

        private string axisPositionNumericStatusName = string.Empty;
        [XmlIgnore]
        public string AxisPositionNumericStatusName
        {
            get { return axisPositionNumericStatusName; }
            set
            {
                if (axisPositionNumericStatusName != value)
                {
                    axisPositionNumericStatusName = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusName));
                }
            }
        }

        private int axisPositionNumericStatusDecimalPlaces = 0;
        public int AxisPositionNumericStatusDecimalPlaces
        {
            get { return axisPositionNumericStatusDecimalPlaces; }
            set
            {
                if (axisPositionNumericStatusDecimalPlaces != value)
                {
                    axisPositionNumericStatusDecimalPlaces = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusDecimalPlaces));
                }
            }
        }

        private double axisPositionNumericStatus = 0;
        [XmlIgnore]
        public double AxisPositionNumericStatus
        {
            get { return axisPositionNumericStatus; }
            set
            {
                value = Math.Round(value, AxisPositionNumericStatusDecimalPlaces);

                if (axisPositionNumericStatus != value)
                {
                    axisPositionNumericStatus = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatus));
                }
            }
        }

        private MotorAxisPositionNumericStatusTagModel axisPositionNumericStatusTagModel = new MotorAxisPositionNumericStatusTagModel();
        public MotorAxisPositionNumericStatusTagModel AxisPositionNumericStatusTagModel
        {
            get { return axisPositionNumericStatusTagModel; }
            set
            {
                if (axisPositionNumericStatusTagModel != value)
                {
                    axisPositionNumericStatusTagModel = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusTagModel));
                }
            }
        }
        #endregion
    }
}