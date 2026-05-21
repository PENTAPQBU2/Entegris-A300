using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteNumericModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand SetAxisMoveAbsoluteNumericCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisMoveAbsoluteNumericKey = string.Empty;
        public string AxisMoveAbsoluteNumericKey
        {
            get { return axisMoveAbsoluteNumericKey; }
            set
            {
                if (axisMoveAbsoluteNumericKey != value)
                {
                    axisMoveAbsoluteNumericKey = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericKey));
                }
            }
        }

        private string axisMoveAbsoluteNumericName = string.Empty;
        [XmlIgnore]
        public string AxisMoveAbsoluteNumericName
        {
            get { return axisMoveAbsoluteNumericName; }
            set
            {
                if (axisMoveAbsoluteNumericName != value)
                {
                    axisMoveAbsoluteNumericName = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericName));
                }
            }
        }

        private int axisMoveAbsoluteNumericDecimalPlaces = 0;
        public int AxisMoveAbsoluteNumericDecimalPlaces
        {
            get { return axisMoveAbsoluteNumericDecimalPlaces; }
            set
            {
                if (axisMoveAbsoluteNumericDecimalPlaces != value)
                {
                    axisMoveAbsoluteNumericDecimalPlaces = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericDecimalPlaces));
                }
            }
        }

        private double axisMoveAbsoluteNumeric = 0;
        [XmlIgnore]
        public double AxisMoveAbsoluteNumeric
        {
            get { return axisMoveAbsoluteNumeric; }
            set
            {
                value = Math.Round(value, AxisMoveAbsoluteNumericDecimalPlaces);

                if (axisMoveAbsoluteNumeric != value)
                {
                    axisMoveAbsoluteNumeric = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumeric));
                }
            }
        }

        private MotorAxisMoveAbsoluteNumericTagModel axisMoveAbsoluteNumericTagModel = new MotorAxisMoveAbsoluteNumericTagModel();
        public MotorAxisMoveAbsoluteNumericTagModel AxisMoveAbsoluteNumericTagModel
        {
            get { return axisMoveAbsoluteNumericTagModel; }
            set
            {
                if (axisMoveAbsoluteNumericTagModel != value)
                {
                    axisMoveAbsoluteNumericTagModel = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericTagModel));
                }
            }
        }
        #endregion
    }
}