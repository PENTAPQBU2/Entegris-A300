using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500
{
    public class IOInputModel : ViewModelBase
    {
        private string ioInputKey = string.Empty;
        public string IOInputKey
        {
            get { return ioInputKey; }
            set 
            {
                if(ioInputKey!=value)
                {
                    ioInputKey = value;
                    RaisePropertyChanged(nameof(IOInputKey));
                }
            }
        }

        private string io_InputName = string.Empty;
        [XmlIgnore]
        public string IO_InputName
        {
            get { return io_InputName; }
            set 
            {
                if(io_InputName!=value)
                {
                    io_InputName = value;
                    RaisePropertyChanged(nameof(IO_InputName));
                }
            }
        }

        private string io_InputValue = string.Empty;
        [XmlIgnore]
        public string IO_InputValue
        {
            get { return io_InputValue; }
            set 
            {
                if(io_InputValue!=value)
                {
                    io_InputValue = value;
                    RaisePropertyChanged(nameof(IO_InputValue));
                }
            }
        }

        private string io_InputModule = string.Empty;
        public string IO_InputModule
        {
            get { return io_InputModule; }
            set
            {
                if(io_InputModule!=value)
                {
                    io_InputModule = value;
                    RaisePropertyChanged(nameof(IO_InputModule));
                }
            }
        }

        private IOInputTagModel ioInputTagModel = new IOInputTagModel();
        public IOInputTagModel IOInputTagModel
        {
            get { return ioInputTagModel; }
            set
            {
                if(ioInputTagModel!=value)
                {
                    ioInputTagModel = value;
                    RaisePropertyChanged(nameof(IOInputTagModel));
                }
            }
        }

    }
}
