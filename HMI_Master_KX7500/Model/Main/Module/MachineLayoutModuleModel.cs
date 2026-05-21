using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.Main.Module.Indicator;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Main.Module
{
    public class MachineLayoutModuleModel : ViewModelBase
    {
        #region PublicProperties

        private string machineLayoutModuleKey = string.Empty;

        public string MachineLayoutModuleKey
        {
            get { return machineLayoutModuleKey; }
            set
            {
                if(machineLayoutModuleKey != value)
                {
                    machineLayoutModuleKey = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleKey));
                }
            }
        }

        private string machineLayoutModuleName = string.Empty;

        [XmlIgnore]
        public string MachineLayoutModuleName
        {
            get { return machineLayoutModuleName; }
            set
            {
                if(machineLayoutModuleName != value)
                {
                    machineLayoutModuleName = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleName));
                }
            }
        }

        private string machineLayoutModuleImage = string.Empty;

        public string MachineLayoutModuleImage
        {
            get { return machineLayoutModuleImage; }
            set
            {
                if(machineLayoutModuleImage != value)
                {
                    machineLayoutModuleImage = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleImage));
                }
            }
        }

        private List<MachineLayoutModuleStatusIndicatorModel> machineLayoutModuleStatusIndicatorList = new List<MachineLayoutModuleStatusIndicatorModel>();

        public List<MachineLayoutModuleStatusIndicatorModel> MachineLayoutModuleStatusIndicatorList
        {
            get { return machineLayoutModuleStatusIndicatorList; }
            set
            {
                if(machineLayoutModuleStatusIndicatorList != value)
                {
                    machineLayoutModuleStatusIndicatorList = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorList));
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorListView));
                }
            }
        }

        private ICollectionView machineLayoutModuleStatusIndicatorListView = null;

        [XmlIgnore]
        public ICollectionView MachineLayoutModuleStatusIndicatorListView
        {
            get
            {
                machineLayoutModuleStatusIndicatorListView = null;
                machineLayoutModuleStatusIndicatorListView = CollectionViewSource.GetDefaultView(MachineLayoutModuleStatusIndicatorList);
                return machineLayoutModuleStatusIndicatorListView;
            }
        }

        #endregion PublicProperties
    }
}