using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Alarm
{
    public class AlarmNodeModel : ViewModelBase
    {
        private string errorNodeString = string.Empty;

        public string ErrorNodeString
        {
            get { return errorNodeString; }
            set
            {
                if(errorNodeString != value)
                {
                    errorNodeString = value;
                    RaisePropertyChanged(nameof(ErrorNodeString));
                }
            }
        }

        private string errorNodeTypeString = string.Empty;

        public string ErrorNodeTypeString
        {
            get { return errorNodeTypeString; }
            set
            {
                if(errorNodeTypeString != value)
                {
                    errorNodeTypeString = value;
                    RaisePropertyChanged(nameof(ErrorNodeTypeString));
                }
            }
        }

        private string warningNodeString = string.Empty;

        public string WarningNodeString
        {
            get { return warningNodeString; }
            set
            {
                if(warningNodeString != value)
                {
                    warningNodeString = value;
                    RaisePropertyChanged(nameof(WarningNodeString));
                }
            }
        }

        private string warningNodeTypeString = string.Empty;

        public string WarningNodeTypeString
        {
            get { return warningNodeTypeString; }
            set
            {
                if(warningNodeTypeString != value)
                {
                    warningNodeTypeString = value;
                    RaisePropertyChanged(nameof(WarningNodeTypeString));
                }
            }
        }
    }
}