using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisPositionStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisPositionStatusTag = string.Empty;
        public string AxisPositionStatusTag
        {
            get { return axisPositionStatusTag; }
            set
            {
                if (axisPositionStatusTag != value)
                {
                    axisPositionStatusTag = value;
                    RaisePropertyChanged(nameof(AxisPositionStatusTag));
                }
            }
        }
        #endregion
    }
}