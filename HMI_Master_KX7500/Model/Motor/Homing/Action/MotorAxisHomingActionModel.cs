using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisHomingActionModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand AxisHomingActionCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisHomingActionKey = string.Empty;
        public string AxisHomingActionKey
        {
            get { return axisHomingActionKey; }
            set
            {
                if (axisHomingActionKey != value)
                {
                    axisHomingActionKey = value;
                    RaisePropertyChanged(nameof(AxisHomingActionKey));
                }
            }
        }

        private string axisHomingActionName = string.Empty;
        [XmlIgnore]
        public string AxisHomingActionName
        {
            get { return axisHomingActionName; }
            set
            {
                if (axisHomingActionName != value)
                {
                    axisHomingActionName = value;
                    RaisePropertyChanged(nameof(AxisHomingActionName));
                }
            }
        }

        private string axisHomingActionIcon = string.Empty;
        public string AxisHomingActionIcon
        {
            get { return axisHomingActionIcon; }
            set
            {
                if (axisHomingActionIcon != value)
                {
                    axisHomingActionIcon = value;
                    RaisePropertyChanged(nameof(AxisHomingActionIcon));
                }
            }
        }

        private MotorAxisHomingActionTagModel axisHomingActionTagModel = new MotorAxisHomingActionTagModel();
        public MotorAxisHomingActionTagModel AxisHomingActionTagModel
        {
            get { return axisHomingActionTagModel; }
            set
            {
                if (axisHomingActionTagModel != value)
                {
                    axisHomingActionTagModel = value;
                    RaisePropertyChanged(nameof(AxisHomingActionTagModel));
                }
            }
        }
        #endregion
    }
}