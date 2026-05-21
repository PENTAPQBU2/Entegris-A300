using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisHomingActionTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisHomingActionTag = string.Empty;
        public string AxisHomingActionTag
        {
            get { return axisHomingActionTag; }
            set
            {
                if (axisHomingActionTag != value)
                {
                    axisHomingActionTag = value;
                    RaisePropertyChanged(nameof(AxisHomingActionTag));
                }
            }
        }
        #endregion
    }
}