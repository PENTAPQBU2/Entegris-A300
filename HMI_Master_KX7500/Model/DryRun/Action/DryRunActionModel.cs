using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.DryRun.Action
{
    public class DryRunActionModel : ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand DryRunActionCommand { get; set; }
        #endregion

        #region PublicProperties
        private string dryRunActionKey = string.Empty;
        public string DryRunActionKey
        {
            get { return dryRunActionKey; }
            set
            {
                if (dryRunActionKey != value)
                {
                    dryRunActionKey = value;
                    RaisePropertyChanged(nameof(DryRunActionKey));
                }
            }
        }

        private string dryRunActionName = string.Empty;
        [XmlIgnore]
        public string DryRunActionName
        {
            get { return dryRunActionName; }
            set
            {
                if (dryRunActionName != value)
                {
                    dryRunActionName = value;
                    RaisePropertyChanged(nameof(DryRunActionName));
                }
            }
        }

        private string dryRunActionIcon = string.Empty;
        public string DryRunActionIcon
        {
            get { return dryRunActionIcon; }
            set
            {
                if (dryRunActionIcon != value)
                {
                    dryRunActionIcon = value;
                    RaisePropertyChanged(nameof(DryRunActionIcon));
                }
            }
        }

        private DryRunActionTagModel dryRunActionTagModel = new DryRunActionTagModel();
        public DryRunActionTagModel DryRunActionTagModel
        {
            get { return dryRunActionTagModel; }
            set
            {
                if (dryRunActionTagModel != value)
                {
                    dryRunActionTagModel = value;
                    RaisePropertyChanged(nameof(DryRunActionTagModel));
                }
            }
        }
        #endregion
    }
}
