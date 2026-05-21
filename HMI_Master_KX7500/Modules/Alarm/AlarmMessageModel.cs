using System.ComponentModel;
using System.Windows.Controls;

namespace HMI_Master_KX7500.Modules.Alarm
{
    public class AlarmMessageModel : UserControl, INotifyPropertyChanged
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if(_message != value)
                {
                    _message = value;
                    NotifyChange(new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private string _action;

        public string Action
        {
            get { return _action; }
            set
            {
                if(_action != value)
                {
                    _action = value;
                    NotifyChange(new PropertyChangedEventArgs(nameof(Action)));
                }
            }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                if(_code != value)
                {
                    _code = value;
                    NotifyChange(new PropertyChangedEventArgs(nameof(Code)));
                }
            }
        }

        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                if(_time != value)
                {
                    _time = value;
                    NotifyChange(new PropertyChangedEventArgs(nameof(Time)));
                }
            }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                if(_type != value)
                {
                    _type = value;
                    NotifyChange(new PropertyChangedEventArgs(nameof(Type)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChange(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}