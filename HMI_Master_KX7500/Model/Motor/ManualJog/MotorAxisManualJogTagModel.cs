using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisManualJogTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisManualJogTag = string.Empty;
        public string AxisManualJogTag
        {
            get { return axisManualJogTag; }
            set
            {
                if (axisManualJogTag != value)
                {
                    axisManualJogTag = value;
                    RaisePropertyChanged(nameof(AxisManualJogTag));
                }
            }
        }
        #endregion
    }
}