using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogNumericTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisManualJogNumericTag = string.Empty;
        public string AxisManualJogNumericTag
        {
            get { return axisManualJogNumericTag; }
            set
            {
                if (axisManualJogNumericTag != value)
                {
                    axisManualJogNumericTag = value;
                    RaisePropertyChanged(nameof(AxisManualJogNumericTag));
                }
            }
        }
        #endregion
    }
}