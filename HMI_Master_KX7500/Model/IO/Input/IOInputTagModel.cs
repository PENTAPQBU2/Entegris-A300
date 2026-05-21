using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500
{
    public class IOInputTagModel : ViewModelBase
    {
        private string ioInputTag = string.Empty;
        public string IOInputTag
        {
            get { return ioInputTag; }
            set 
            {
                if(ioInputTag!=value)
                {
                    ioInputTag = value;
                    RaisePropertyChanged(nameof(IOInputTag));
                }
            }
        }

    }
}
