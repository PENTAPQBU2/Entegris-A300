using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionActionTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionActionTag = string.Empty;
        public string AxisTeachingPositionActionTag
        {
            get { return axisTeachingPositionActionTag; }
            set
            {
                if (axisTeachingPositionActionTag != value)
                {
                    axisTeachingPositionActionTag = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionTag));
                }
            }
        }
        #endregion
    }
}