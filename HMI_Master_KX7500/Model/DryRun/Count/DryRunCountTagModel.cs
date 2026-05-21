using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Model.DryRun.Count
{
    public class DryRunCountTagModel : ViewModelBase
    {
        #region PublicProperties
        private string dryRunCountTag = string.Empty;
        public string DryRunCountTag
        {
            get { return dryRunCountTag; }
            set
            {
                if (dryRunCountTag != value)
                {
                    dryRunCountTag = value;
                    RaisePropertyChanged(nameof(DryRunCountTag));
                }
            }
        }
        #endregion
    }
}
