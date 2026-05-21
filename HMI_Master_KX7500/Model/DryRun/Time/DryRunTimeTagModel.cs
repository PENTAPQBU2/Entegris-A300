using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Model.DryRun.Time
{
    public class DryRunTimeTagModel : ViewModelBase
    {
        #region PublicProperties
        private string dryRunTimeTag = string.Empty;
        public string DryRunTimeTag
        {
            get { return dryRunTimeTag; }
            set
            {
                if (dryRunTimeTag != value)
                {
                    dryRunTimeTag = value;
                    RaisePropertyChanged(nameof(DryRunTimeTag));
                }
            }
        }
        #endregion
    }
}
