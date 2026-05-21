using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisMoveAbsoluteActionModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand AxisMoveAbsoluteActionCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisMoveAbsoluteActionKey = string.Empty;
        public string AxisMoveAbsoluteActionKey
        {
            get { return axisMoveAbsoluteActionKey; }
            set
            {
                if (axisMoveAbsoluteActionKey != value)
                {
                    axisMoveAbsoluteActionKey = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionKey));
                }
            }
        }

        private string axisMoveAbsoluteActionName = string.Empty;
        [XmlIgnore]
        public string AxisMoveAbsoluteActionName
        {
            get { return axisMoveAbsoluteActionName; }
            set
            {
                if (axisMoveAbsoluteActionName != value)
                {
                    axisMoveAbsoluteActionName = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionName));
                }
            }
        }

        private string axisMoveAbsoluteActionIcon = string.Empty;
        public string AxisMoveAbsoluteActionIcon
        {
            get { return axisMoveAbsoluteActionIcon; }
            set
            {
                if (axisMoveAbsoluteActionIcon != value)
                {
                    axisMoveAbsoluteActionIcon = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionIcon));
                }
            }
        }

        private MotorAxisMoveAbsoluteActionTagModel axisMoveAbsoluteActionTagModel = new MotorAxisMoveAbsoluteActionTagModel();
        public MotorAxisMoveAbsoluteActionTagModel AxisMoveAbsoluteActionTagModel
        {
            get { return axisMoveAbsoluteActionTagModel; }
            set
            {
                if (axisMoveAbsoluteActionTagModel != value)
                {
                    axisMoveAbsoluteActionTagModel = value;
                    RaisePropertyChanged(nameof(AxisMoveAbsoluteActionTagModel));
                }
            }
        }
        #endregion
    }
}