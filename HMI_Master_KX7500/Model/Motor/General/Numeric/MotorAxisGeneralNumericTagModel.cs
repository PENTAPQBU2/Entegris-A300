using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralNumericTagModel : ViewModelBase
    {
        #region PublicProperties
        private string axisGeneralNumericTag = string.Empty;
        public string AxisGeneralNumericTag
        {
            get { return axisGeneralNumericTag; }
            set
            {
                if (axisGeneralNumericTag != value)
                {
                    axisGeneralNumericTag = value;
                    RaisePropertyChanged(nameof(AxisGeneralNumericTag));
                }
            }
        }
        #endregion
    }
}