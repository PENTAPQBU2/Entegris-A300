using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionNumericStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionNumericStatusTag = string.Empty;
        public string AxisTeachingPositionNumericStatusTag
        {
            get { return axisTeachingPositionNumericStatusTag; }
            set
            {
                if (axisTeachingPositionNumericStatusTag != value)
                {
                    axisTeachingPositionNumericStatusTag = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericStatusTag));
                }
            }
        }
        #endregion
    }
}