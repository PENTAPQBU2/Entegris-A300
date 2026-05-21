using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogNumericModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SetAxisManualJogNumericCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisManualJogNumericKey = string.Empty;
        public string AxisManualJogNumericKey
        {
            get { return axisManualJogNumericKey; }
            set
            {
                if (axisManualJogNumericKey != value)
                {
                    axisManualJogNumericKey = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumericKey));
                }
            }
        }

        private string axisManualJogNumericName = string.Empty;
        [XmlIgnore]
        public string AxisManualJogNumericName
        {
            get { return axisManualJogNumericName; }
            set
            {
                if (axisManualJogNumericName != value)
                {
                    axisManualJogNumericName = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumericName));
                }
            }
        }

        private int axisManualJogNumericDecimalPlaces = 0;
        public int AxisManualJogNumericDecimalPlaces
        {
            get { return axisManualJogNumericDecimalPlaces; }
            set
            {
                if (axisManualJogNumericDecimalPlaces != value)
                {
                    axisManualJogNumericDecimalPlaces = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumericDecimalPlaces));
                }
            }
        }

        private double axisManualJogNumeric = 0;
        [XmlIgnore]
        public double AxisManualJogNumeric
        {
            get { return axisManualJogNumeric; }
            set
            {
                value = Math.Round(value, AxisManualJogNumericDecimalPlaces);

                if (axisManualJogNumeric != value)
                {
                    axisManualJogNumeric = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumeric));
                }
            }
        }

        private MotorAxisManualJogNumericTagModel axisManualJogNumericTagModel = new MotorAxisManualJogNumericTagModel();
        public MotorAxisManualJogNumericTagModel AxisManualJogNumericTagModel
        {
            get { return axisManualJogNumericTagModel; }
            set
            {
                if (axisManualJogNumericTagModel != value)
                {
                    axisManualJogNumericTagModel = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumericTagModel));
                }
            }
        }
        #endregion
    }
}