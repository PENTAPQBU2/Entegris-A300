using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisMoveAbsoluteStatusTag = string.Empty;
        public string AxisMoveAbsoluteStatusTag
        {
            get { return axisMoveAbsoluteStatusTag; }
            set
            {
                if (axisMoveAbsoluteStatusTag != value)
                {
                    axisMoveAbsoluteStatusTag = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteStatusTag));
                }
            }
        }
        #endregion
    }
}