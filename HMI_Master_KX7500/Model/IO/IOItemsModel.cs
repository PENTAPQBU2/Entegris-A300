using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500
{
    public class IOItemsModel : ViewModelBase
    {
        private string ioModule = string.Empty;
        //[XmlIgnore]
        public string IOModule
        {
            get { return ioModule; }
            set
            {
                if (ioModule != value)
                {
                    ioModule = value;
                    RaisePropertyChanged(nameof(IOModule));
                }
            }
        }

        private List<IOInputModel> ioInputList = new List<IOInputModel>();
        public List<IOInputModel> IOInputList
        {
            get { return ioInputList; }
            set
            {
                if (ioInputList != value)
                {
                    ioInputList = value;
                    RaisePropertyChanged(nameof(IOInputList));
                }
            }
        }

        private List<IOOutputModel> ioOutputList = new List<IOOutputModel>();
        public List<IOOutputModel> IOOutputList
        {
            get { return ioOutputList; }
            set
            {
                if (ioOutputList != value)
                {
                    ioOutputList = value;
                    RaisePropertyChanged(nameof(IOOutputList));
                }
            }
        }
    }
}
