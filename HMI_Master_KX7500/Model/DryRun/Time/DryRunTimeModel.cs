using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.DryRun.Time
{
    public class DryRunTimeModel : ViewModelBase
    {
        #region PublicProperties
        private string dryRunTimeKey = string.Empty;
        public string DryRunTimeKey
        {
            get { return dryRunTimeKey; }
            set
            {
                if (dryRunTimeKey != value)
                {
                    dryRunTimeKey = value;
                    RaisePropertyChanged(nameof(DryRunTimeKey));
                }
            }
        }

        private string dryRunTimeName = string.Empty;
        [XmlIgnore]
        public string DryRunTimeName
        {
            get { return dryRunTimeName; }
            set
            {
                if (dryRunTimeName != value)
                {
                    dryRunTimeName = value;
                    RaisePropertyChanged(nameof(DryRunTimeName));
                }
            }
        }

        private string dryRunTime = string.Empty;
        [XmlIgnore]
        public string DryRunTime
        {
            get { return dryRunTime; }
            set
            {
                if (dryRunTime != value)
                {
                    dryRunTime = value;
                    RaisePropertyChanged(nameof(DryRunTime));
                }
            }
        }

        private DryRunTimeTagModel dryRunTimeTagModel = new DryRunTimeTagModel();
        public DryRunTimeTagModel DryRunTimeTagModel
        {
            get { return dryRunTimeTagModel; }
            set
            {
                if (dryRunTimeTagModel != value)
                {
                    dryRunTimeTagModel = value;
                    RaisePropertyChanged(nameof(DryRunTimeTagModel));
                }
            }
        }
        #endregion
    }
}
