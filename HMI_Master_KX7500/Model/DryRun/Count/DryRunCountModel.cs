using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.DryRun.Count
{
    public class DryRunCountModel : ViewModelBase
    {
        #region PublicProperties
        private string dryRunCountKey = string.Empty;
        public string DryRunCountKey
        {
            get { return dryRunCountKey; }
            set
            {
                if (dryRunCountKey != value)
                {
                    dryRunCountKey = value;
                    RaisePropertyChanged(nameof(DryRunCountKey));
                }
            }
        }

        private string dryRunCountName = string.Empty;
        [XmlIgnore]
        public string DryRunCountName
        {
            get { return dryRunCountName; }
            set
            {
                if (dryRunCountName != value)
                {
                    dryRunCountName = value;
                    RaisePropertyChanged(nameof(DryRunCountName));
                }
            }
        }

        private string dryRunCount = string.Empty;
        [XmlIgnore]
        public string DryRunCount
        {
            get { return dryRunCount; }
            set
            {
                if (dryRunCount != value)
                {
                    dryRunCount = value;
                    RaisePropertyChanged(nameof(DryRunCount));
                }
            }
        }

        private DryRunCountTagModel dryRunCountTagModel = new DryRunCountTagModel();
        public DryRunCountTagModel DryRunCountTagModel
        {
            get { return dryRunCountTagModel; }
            set
            {
                if (dryRunCountTagModel != value)
                {
                    dryRunCountTagModel = value;
                    RaisePropertyChanged(nameof(DryRunCountTagModel));
                }
            }
        }
        #endregion
    }
}
