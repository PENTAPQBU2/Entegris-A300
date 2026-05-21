using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisPositionNumericStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisPositionNumericStatusTag = string.Empty;
        public string AxisPositionNumericStatusTag
        {
            get { return axisPositionNumericStatusTag; }
            set
            {
                if (axisPositionNumericStatusTag != value)
                {
                    axisPositionNumericStatusTag = value;
                    RaisePropertyChanged(nameof(AxisPositionNumericStatusTag));
                }
            }
        }
        #endregion
    }
}