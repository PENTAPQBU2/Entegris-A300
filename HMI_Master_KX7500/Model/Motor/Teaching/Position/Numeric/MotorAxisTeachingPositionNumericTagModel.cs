using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionNumericTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisTeachingPositionNumericTag = string.Empty;
        public string AxisTeachingPositionNumericTag
        {
            get { return axisTeachingPositionNumericTag; }
            set
            {
                if (axisTeachingPositionNumericTag != value)
                {
                    axisTeachingPositionNumericTag = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionNumericTag));
                }
            }
        }
        #endregion
    }
}