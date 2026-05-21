using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class SettingsGeneralModel : ViewModelBase
    {
        #region PublicProperties
        private List<SettingsGeneralGroupModel> settingsGeneralGroupList = new List<SettingsGeneralGroupModel>();
        public List<SettingsGeneralGroupModel> SettingsGeneralGroupList
        {
            get { return settingsGeneralGroupList; }
            set
            {
                if (settingsGeneralGroupList != value)
                {
                    settingsGeneralGroupList = value;
                    RaisePropertyChanged(nameof(SettingsGeneralGroupList));
                    RaisePropertyChanged(nameof(SettingsGeneralGroupListView));
                }
            }
        }

        private ICollectionView settingsGeneralGroupListView = null;
        [XmlIgnore]
        public ICollectionView SettingsGeneralGroupListView
        {
            get
            {
                settingsGeneralGroupListView = null;
                settingsGeneralGroupListView = CollectionViewSource.GetDefaultView(SettingsGeneralGroupList);
                return settingsGeneralGroupListView;
            }
        }
        #endregion
    }
}