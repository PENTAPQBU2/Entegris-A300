using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisGeneralActionModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand AxisGeneralActionCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisGeneralActionKey = string.Empty;
        public string AxisGeneralActionKey
        {
            get { return axisGeneralActionKey; }
            set
            {
                if (axisGeneralActionKey != value)
                {
                    axisGeneralActionKey = value;
                    RaisePropertyChanged(nameof(AxisGeneralActionKey));
                }
            }
        }

        private string axisGeneralActionName = string.Empty;
        [XmlIgnore]
        public string AxisGeneralActionName
        {
            get { return axisGeneralActionName; }
            set
            {
                if (axisGeneralActionName != value)
                {
                    axisGeneralActionName = value;
                    RaisePropertyChanged(nameof(AxisGeneralActionName));
                }
            }
        }

        private string axisGeneralActionIcon = string.Empty;
        public string AxisGeneralActionIcon
        {
            get { return axisGeneralActionIcon; }
            set
            {
                if (axisGeneralActionIcon != value)
                {
                    axisGeneralActionIcon = value;
                    RaisePropertyChanged(nameof(AxisGeneralActionIcon));
                }
            }
        }

        private MotorAxisGeneralActionTagModel axisGeneralActionTagModel = new MotorAxisGeneralActionTagModel();
        public MotorAxisGeneralActionTagModel AxisGeneralActionTagModel
        {
            get { return axisGeneralActionTagModel; }
            set
            {
                if (axisGeneralActionTagModel != value)
                {
                    axisGeneralActionTagModel = value;
                    RaisePropertyChanged(nameof(AxisGeneralActionTagModel));
                }
            }
        }
        #endregion
    }
}