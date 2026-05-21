using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogIntervalTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisManualJogIntervalTag = string.Empty;
        public string AxisManualJogIntervalTag
        {
            get { return axisManualJogIntervalTag; }
            set
            {
                if (axisManualJogIntervalTag != value)
                {
                    axisManualJogIntervalTag = value;
                    RaisePropertyChanged(nameof(AxisManualJogIntervalTag));
                }
            }
        }
        #endregion
    }
}