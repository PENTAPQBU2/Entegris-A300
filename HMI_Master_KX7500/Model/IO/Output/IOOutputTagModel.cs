using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500
{
    public class IOOutputTagModel : ViewModelBase
    {
        private string ioOutputTag = string.Empty;
        public string IOOutputTag
        {
            get { return ioOutputTag; }
            set 
            {
                if(ioOutputTag!=value)
                {
                    ioOutputTag = value;
                    RaisePropertyChanged(nameof(IOOutputTag));
                }
            }
        }
    }
}
