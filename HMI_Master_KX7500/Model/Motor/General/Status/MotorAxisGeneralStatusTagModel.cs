using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisGeneralStatusTag = string.Empty;
        public string AxisGeneralStatusTag
        {
            get { return axisGeneralStatusTag; }
            set
            {
                if (axisGeneralStatusTag != value)
                {
                    axisGeneralStatusTag = value;
                    RaisePropertyChanged(nameof(AxisGeneralStatusTag));
                }
            }
        }
        #endregion
    }
}