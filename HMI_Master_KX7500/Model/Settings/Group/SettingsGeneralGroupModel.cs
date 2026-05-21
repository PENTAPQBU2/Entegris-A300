using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class SettingsGeneralGroupModel : ViewModelBase
    {
        #region PublicProperties
        private string settingsGeneralGroupKey = string.Empty;
        public string SettingsGeneralGroupKey
        {
            get { return settingsGeneralGroupKey; }
            set
            {
                if (settingsGeneralGroupKey != value)
                {
                    settingsGeneralGroupKey = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupKey));
                }
            }
        }

        private string settingsGeneralGroupName = string.Empty;
        [XmlIgnore]
        public string SettingsGeneralGroupName
        {
            get { return settingsGeneralGroupName; }
            set
            {
                if (settingsGeneralGroupName != value)
                {
                    settingsGeneralGroupName = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupName));
                }
            }
        }

        private List<SettingsGeneralGroupNumericModel> settingsGeneralGroupNumericList = new List<SettingsGeneralGroupNumericModel>();
        public List<SettingsGeneralGroupNumericModel> SettingsGeneralGroupNumericList
        {
            get { return settingsGeneralGroupNumericList; }
            set
            {
                if (settingsGeneralGroupNumericList != value)
                {
                    settingsGeneralGroupNumericList = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupNumericList));
                }
            }
        }

        private ICollectionView settingsGeneralGroupNumericListView = null;
        [XmlIgnore]
        public ICollectionView SettingsGeneralGroupNumericListView
        {
            get
            {
                settingsGeneralGroupNumericListView = null;
                settingsGeneralGroupNumericListView = CollectionViewSource.GetDefaultView(SettingsGeneralGroupNumericList);
                return settingsGeneralGroupNumericListView;
            }
        }

        private List<SettingsGeneralGroupToggleModel> settingsGeneralGroupToggleList = new List<SettingsGeneralGroupToggleModel>();
        public List<SettingsGeneralGroupToggleModel> SettingsGeneralGroupToggleList
        {
            get { return settingsGeneralGroupToggleList; }
            set
            {
                if (settingsGeneralGroupToggleList != value)
                {
                    settingsGeneralGroupToggleList = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupToggleList));
                }
            }
        }

        private ICollectionView settingsGeneralGroupToggleListView = null;
        [XmlIgnore]
        public ICollectionView SettingsGeneralGroupToggleListView
        {
            get
            {
                settingsGeneralGroupToggleListView = null;
                settingsGeneralGroupToggleListView = CollectionViewSource.GetDefaultView(SettingsGeneralGroupToggleList);
                return settingsGeneralGroupToggleListView;
            }
        }

        //
        private List<SettingsGeneralGroupReadOnlyModel> settingsGeneralGroupReadOnlyList = new List<SettingsGeneralGroupReadOnlyModel>();
        public List<SettingsGeneralGroupReadOnlyModel> SettingsGeneralGroupReadOnlyList
        {
            get { return settingsGeneralGroupReadOnlyList; }
            set
            {
                if (settingsGeneralGroupReadOnlyList != value)
                {
                    settingsGeneralGroupReadOnlyList = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupReadOnlyList));
                }
            }
        }

        private ICollectionView settingsGeneralGroupReadOnlyListView = null;
        [XmlIgnore]
        public ICollectionView SettingsGeneralGroupReadOnlyListView
        {
            get
            {
                settingsGeneralGroupReadOnlyListView = null;
                settingsGeneralGroupReadOnlyListView = CollectionViewSource.GetDefaultView(SettingsGeneralGroupReadOnlyList);
                return settingsGeneralGroupReadOnlyListView;
            }
        }
        #endregion
    }
}