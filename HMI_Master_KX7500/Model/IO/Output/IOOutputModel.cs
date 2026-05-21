using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500
{
    public class IOOutputModel : ViewModelBase
    {
        private string ioOutputKey = string.Empty;
        public string IOOutputKey
        {
            get { return ioOutputKey; }
            set 
            {
                if(ioOutputKey!=value)
                {
                    ioOutputKey = value;
                    RaisePropertyChanged(nameof(IOOutputKey));
                }
            }
        }

        private string ioOutputName = string.Empty;
        [XmlIgnore]
        public string IOOutputName
        {
            get { return ioOutputName; }
            set 
            {
                if(ioOutputName!=value)
                {
                    ioOutputName = value;
                    RaisePropertyChanged(nameof(IOOutputName));
                }
            }
        }

        private string io_OutputModule = string.Empty;
        public string IO_OutputModule
        {
            get { return io_OutputModule; }
            set 
            {
                if(io_OutputModule!=value)
                {
                    io_OutputModule = value;
                    RaisePropertyChanged(nameof(IO_OutputModule));
                }
            }
        }

        private string io_OutputValue = string.Empty;
        [XmlIgnore]
        public string IO_OutputValue
        {
            get { return io_OutputValue; }
            set
            {
                if(io_OutputValue!=value)
                {
                    io_OutputValue = value;
                    RaisePropertyChanged(nameof(IO_OutputValue));
                }
            }
        }

        private IOOutputTagModel ioOutputTagModel = new IOOutputTagModel();
        public IOOutputTagModel IOOutputTagModel
        {
            get { return ioOutputTagModel; }
            set 
            {
                if(ioOutputTagModel!=value)
                {
                    ioOutputTagModel = value;
                    RaisePropertyChanged(nameof(IOOutputTagModel));
                }
            }
        }

        [XmlIgnore]
        public ICommand IOOutputCommand { get; set; }
    }
}
