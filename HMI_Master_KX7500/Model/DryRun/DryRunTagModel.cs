using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Model.DryRun
{
    public class DryRunTagModel : ViewModelBase
    {
        #region PublicProperties
        private string dryRunEnableTag = string.Empty;
        public string DryRunEnableTag
        {
            get { return dryRunEnableTag; }
            set
            {
                if (dryRunEnableTag != value)
                {
                    dryRunEnableTag = value;
                    RaisePropertyChanged(nameof(DryRunEnableTag));
                }
            }
        }

        private string dryRunEnableTagType = string.Empty;

        public string DryRunEnableTagType
        {
            get { return dryRunEnableTagType; }
            set
            {
                if (dryRunEnableTagType != value)
                {
                    dryRunEnableTagType = value;
                    RaisePropertyChanged(nameof(DryRunEnableTagType));
                }
            }
        }

        private string dryRunNameTag = string.Empty;
        public string DryRunNameTag
        {
            get { return dryRunNameTag; }
            set
            {
                if (dryRunNameTag != value)
                {
                    dryRunNameTag = value;
                    RaisePropertyChanged(nameof(DryRunNameTag));
                }
            }
        }

        private string dryRunNameTagType = string.Empty;

        public string DryRunNameTagType
        {
            get { return dryRunNameTagType; }
            set
            {
                if (dryRunNameTagType != value)
                {
                    dryRunNameTagType = value;
                    RaisePropertyChanged(nameof(DryRunNameTagType));
                }
            }
        }

        private string dryRunStartTag = string.Empty;
        public string DryRunStartTag
        {
            get { return dryRunStartTag; }
            set
            {
                if (dryRunStartTag != value)
                {
                    dryRunStartTag = value;
                    RaisePropertyChanged(nameof(DryRunStartTag));
                }
            }
        }

        private string dryRunStartTagType = string.Empty;

        public string DryRunStartTagType
        {
            get { return dryRunStartTagType; }
            set
            {
                if (dryRunStartTagType != value)
                {
                    dryRunStartTagType = value;
                    RaisePropertyChanged(nameof(DryRunStartTagType));
                }
            }
        }
        #endregion
    }
}
