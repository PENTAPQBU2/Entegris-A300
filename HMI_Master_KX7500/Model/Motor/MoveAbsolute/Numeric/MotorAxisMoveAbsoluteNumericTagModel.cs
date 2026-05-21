using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteNumericTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisMoveAbsoluteNumericTag = string.Empty;
        public string AxisMoveAbsoluteNumericTag
        {
            get { return axisMoveAbsoluteNumericTag; }
            set
            {
                if (axisMoveAbsoluteNumericTag != value)
                {
                    axisMoveAbsoluteNumericTag = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteNumericTag));
                }
            }
        }
        #endregion
    }
}