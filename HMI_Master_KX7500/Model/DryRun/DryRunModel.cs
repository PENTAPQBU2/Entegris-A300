using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Model.DryRun.Action;
using HMI_Master_KX7500.Model.DryRun.Count;
using HMI_Master_KX7500.Model.DryRun.Time;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.DryRun
{
    public class DryRunModel : ViewModelBase
    {
        #region PublicProperties
        private bool dryRunEnable = false;
        [XmlIgnore]
        public bool DryRunEnable
        {
            get { return dryRunEnable; }
            set
            {
                if (dryRunEnable != value)
                {
                    dryRunEnable = value;
                    RaisePropertyChanged(nameof(DryRunEnable));
                    Messenger.Default.Send(DryRunUpdate.Enable);
                }
            }
        }

        private string dryRunKey = string.Empty;
        [XmlIgnore]
        public string DryRunKey
        {
            get { return dryRunKey; }
            set
            {
                if (dryRunKey != value)
                {
                    dryRunKey = value;
                    RaisePropertyChanged(nameof(DryRunKey));
                    Messenger.Default.Send(DryRunUpdate.Name);
                }
            }
        }

        private string dryRunName = string.Empty;
        [XmlIgnore]
        public string DryRunName
        {
            get { return dryRunName; }
            set
            {
                if (dryRunName != value)
                {
                    dryRunName = value;
                    RaisePropertyChanged(nameof(DryRunName));
                }
            }
        }

        private string dryRunStart = string.Empty;
        [XmlIgnore]
        public string DryRunStart
        {
            get { return dryRunStart; }
            set
            {
                if (dryRunStart != value)
                {
                    dryRunStart = value;
                    RaisePropertyChanged(nameof(DryRunStart));
                }
            }
        }

        private DryRunTagModel dryRunTagModel = new DryRunTagModel();
        public DryRunTagModel DryRunTagModel
        {
            get { return dryRunTagModel; }
            set
            {
                if (dryRunTagModel != value)
                {
                    dryRunTagModel = value;
                    RaisePropertyChanged(nameof(DryRunTagModel));
                }
            }
        }

        private ObservableCollection<DryRunTimeModel> dryRunTimeList = new ObservableCollection<DryRunTimeModel>();
        public ObservableCollection<DryRunTimeModel> DryRunTimeList
        {
            get { return dryRunTimeList; }
            set
            {
                if (dryRunTimeList != value)
                {
                    dryRunTimeList = value;
                    RaisePropertyChanged(nameof(DryRunTimeList));
                }
            }
        }

        private ObservableCollection<DryRunCountModel> dryRunCountList = new ObservableCollection<DryRunCountModel>();
        public ObservableCollection<DryRunCountModel> DryRunCountList
        {
            get { return dryRunCountList; }
            set
            {
                if (dryRunCountList != value)
                {
                    dryRunCountList = value;
                    RaisePropertyChanged(nameof(DryRunCountList));
                }
            }
        }

        private ObservableCollection<DryRunActionModel> dryRunActionList = new ObservableCollection<DryRunActionModel>();
        public ObservableCollection<DryRunActionModel> DryRunActionList
        {
            get { return dryRunActionList; }
            set
            {
                if (dryRunActionList != value)
                {
                    dryRunActionList = value;
                    RaisePropertyChanged(nameof(DryRunActionList));
                }
            }
        }
        #endregion
    }
}
