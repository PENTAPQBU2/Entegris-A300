using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionStatusTag = string.Empty;
        public string AxisTeachingPositionStatusTag
        {
            get { return axisTeachingPositionStatusTag; }
            set
            {
                if (axisTeachingPositionStatusTag != value)
                {
                    axisTeachingPositionStatusTag = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionStatusTag));
                }
            }
        }
        #endregion
    }
}