using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralActionTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisGeneralActionTag = string.Empty;
        public string AxisGeneralActionTag
        {
            get { return axisGeneralActionTag; }
            set
            {
                if (axisGeneralActionTag != value)
                {
                    axisGeneralActionTag = value;
                    RaisePropertyChanged(nameof(AxisGeneralActionTag));
                }
            }
        }
        #endregion
    }
}