using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Model.DryRun.Action
{
    public class DryRunActionTagModel : ViewModelBase
    {
        #region PublicProperties
        private string dryRunActionTag = string.Empty;
        public string DryRunActionTag
        {
            get { return dryRunActionTag; }
            set
            {
                if (dryRunActionTag != value)
                {
                    dryRunActionTag = value;
                    RaisePropertyChanged(nameof(DryRunActionTag));
                }
            }
        }
        #endregion
    }
}
