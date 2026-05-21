using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisHomingStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisHomingStatusTag = string.Empty;
        public string AxisHomingStatusTag
        {
            get { return axisHomingStatusTag; }
            set
            {
                if (axisHomingStatusTag != value)
                {
                    axisHomingStatusTag = value;
                    RaisePropertyChanged(nameof(AxisHomingStatusTag));
                }
            }
        }
        #endregion
    }
}