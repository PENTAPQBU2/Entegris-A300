using NLog;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for AlarmMessageModule.xaml
    /// </summary>
    public partial class AlarmMessageModule : UserControl, INotifyPropertyChanged
    {
        #region Private Fields

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion Private Fields

        #region Constructor

        public AlarmMessageModule()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #endregion Constructor

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if(_message != value)
                {
                    _message = value;
                    NotifyChanged(new PropertyChangedEventArgs(nameof(Message)));
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
                    NotifyChanged(new PropertyChangedEventArgs(nameof(Action)));
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
                    NotifyChanged(new PropertyChangedEventArgs(nameof(Code)));
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
                    NotifyChanged(new PropertyChangedEventArgs(nameof(Time)));
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
                    NotifyChanged(new PropertyChangedEventArgs(nameof(Type)));
                }
            }
        }

        public event EventHandler Next_Click;

        private void NextItem_Click(object sender, RoutedEventArgs e)
        {
            if(Next_Click != null)
            { Next_Click(this, e); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChanged(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}