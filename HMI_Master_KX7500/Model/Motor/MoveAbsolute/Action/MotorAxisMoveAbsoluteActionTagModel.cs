using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteActionTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisMoveAbsoluteActionTag = string.Empty;
        public string AxisMoveAbsoluteActionTag
        {
            get { return axisMoveAbsoluteActionTag; }
            set
            {
                if (axisMoveAbsoluteActionTag != value)
                {
                    axisMoveAbsoluteActionTag = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionTag));
                }
            }
        }
        #endregion
    }
}