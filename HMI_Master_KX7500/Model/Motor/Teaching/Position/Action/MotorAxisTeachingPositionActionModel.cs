using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorAxisTeachingPositionActionModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand AxisTeachingPositionActionCommand { get; set; }
        #endregion

        #region PublicProperties
        private string axisTeachingPositionActionKey = string.Empty;
        public string AxisTeachingPositionActionKey
        {
            get { return axisTeachingPositionActionKey; }
            set
            {
                if (axisTeachingPositionActionKey != value)
                {
                    axisTeachingPositionActionKey = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionKey));
                }
            }
        }

        private string axisTeachingPositionActionName = string.Empty;
        [XmlIgnore]
        public string AxisTeachingPositionActionName
        {
            get { return axisTeachingPositionActionName; }
            set
            {
                if (axisTeachingPositionActionName != value)
                {
                    axisTeachingPositionActionName = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionName));
                }
            }
        }

        private string axisTeachingPositionActionIcon = string.Empty;
        public string AxisTeachingPositionActionIcon
        {
            get { return axisTeachingPositionActionIcon; }
            set
            {
                if (axisTeachingPositionActionIcon != value)
                {
                    axisTeachingPositionActionIcon = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionIcon));
                }
            }
        }

        private MotorAxisTeachingPositionActionTagModel axisTeachingPositionActionTagModel = new MotorAxisTeachingPositionActionTagModel();
        public MotorAxisTeachingPositionActionTagModel AxisTeachingPositionActionTagModel
        {
            get { return axisTeachingPositionActionTagModel; }
            set
            {
                if (axisTeachingPositionActionTagModel != value)
                {
                    axisTeachingPositionActionTagModel = value;
                    RaisePropertyChanged(nameof(AxisTeachingPositionActionTagModel));
                }
            }
        }
        #endregion
    }
}