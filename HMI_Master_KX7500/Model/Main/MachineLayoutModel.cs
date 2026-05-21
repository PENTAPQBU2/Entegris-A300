using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.Main.Module;
using HMI_Master_KX7500.Model.Main.Status;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Main
{
    public class MachineLayoutModel : ViewModelBase
    {
        #region PublicProperties

        private List<MachineLayoutModuleModel> machineLayoutModuleList = new List<MachineLayoutModuleModel>();

        public List<MachineLayoutModuleModel> MachineLayoutModuleList
        {
            get { return machineLayoutModuleList; }
            set
            {
                if(machineLayoutModuleList != value)
                {
                    machineLayoutModuleList = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleList));
                    RaisePropertyChanged(nameof(MachineLayoutModuleListView));
                }
            }
        }

        private ICollectionView machineLayoutModuleListView = null;

        [XmlIgnore]
        public ICollectionView MachineLayoutModuleListView
        {
            get
            {
                machineLayoutModuleListView = null;
                machineLayoutModuleListView = CollectionViewSource.GetDefaultView(MachineLayoutModuleList);
                return machineLayoutModuleListView;
            }
        }

        private List<MachineLayoutModuleStatusModel> machineLayoutModuleStatusList = new List<MachineLayoutModuleStatusModel>();

        public List<MachineLayoutModuleStatusModel> MachineLayoutModuleStatusList
        {
            get { return machineLayoutModuleStatusList; }
            set
            {
                if(machineLayoutModuleStatusList != value)
                {
                    machineLayoutModuleStatusList = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusList));
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusListView));
                }
            }
        }

        private ICollectionView machineLayoutModuleStatusListView = null;

        [XmlIgnore]
        public ICollectionView MachineLayoutModuleStatusListView
        {
            get
            {
                machineLayoutModuleStatusListView = null;
                machineLayoutModuleStatusListView = CollectionViewSource.GetDefaultView(MachineLayoutModuleStatusList);
                return machineLayoutModuleStatusListView;
            }
        }

        //private List<MachineLayoutToggleModel> machineLayoutToggleList = new List<MachineLayoutToggleModel>();
        //public List<MachineLayoutToggleModel> MachineLayoutToggleList
        //{
        //    get { return machineLayoutToggleList; }
        //    set
        //    {
        //        if (machineLayoutToggleList != value)
        //        {
        //            machineLayoutToggleList = value;
        //            RaisePropertyChanged(nameof(MachineLayoutToggleList));
        //        }
        //    }
        //}

        //private ICollectionView machineLayoutToggleListView = null;
        //[XmlIgnore]
        //public ICollectionView MachineLayoutToggleListView
        //{
        //    get
        //    {
        //        machineLayoutToggleListView = null;
        //        machineLayoutToggleListView = CollectionViewSource.GetDefaultView(MachineLayoutToggleList);
        //        return machineLayoutToggleListView;
        //    }
        //}

        #endregion PublicProperties
    }
}