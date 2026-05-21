using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogStatusTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisManualJogStatusTag = string.Empty;
        public string AxisManualJogStatusTag
        {
            get { return axisManualJogStatusTag; }
            set
            {
                if (axisManualJogStatusTag != value)
                {
                    axisManualJogStatusTag = value;
                    RaisePropertyChanged(nameof(AxisManualJogStatusTag));
                }
            }
        }
        #endregion
    }
}