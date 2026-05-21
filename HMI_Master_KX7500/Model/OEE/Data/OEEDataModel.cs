using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.OEE.Data.Count;
using HMI_Master_KX7500.Model.OEE.Data.ReadOnly;
using HMI_Master_KX7500.Model.OEE.Data.Time;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.OEE.Data
{
    public class OEEDataModel : ViewModelBase
    {
        #region PublicProperties

        private string oEEDataKey = string.Empty;

        public string OEEDataKey
        {
            get { return oEEDataKey; }
            set
            {
                if(oEEDataKey != value)
                {
                    oEEDataKey = value;
                    RaisePropertyChanged(nameof(OEEDataKey));
                }
            }
        }

        private string oEEDataName = string.Empty;

        [XmlIgnore]
        public string OEEDataName
        {
            get { return oEEDataName; }
            set
            {
                if(oEEDataName != value)
                {
                    oEEDataName = value;
                    RaisePropertyChanged(nameof(OEEDataName));
                }
            }
        }

        private string oEECsvFolderPath = string.Empty;

        public string OEECsvFolderPath
        {
            get { return oEECsvFolderPath; }
            set
            {
                if(oEECsvFolderPath != value)
                {
                    oEECsvFolderPath = value;
                    RaisePropertyChanged(nameof(OEECsvFolderPath));
                }
            }
        }

        private bool oEEEnableMultipleCsv = false;

        public bool OEEEnableMultipleCsv
        {
            get { return oEEEnableMultipleCsv; }
            set
            {
                if(oEEEnableMultipleCsv != value)
                {
                    oEEEnableMultipleCsv = value;
                    RaisePropertyChanged(nameof(OEEEnableMultipleCsv));
                }
            }
        }

        private OEEDataTagModel oEEDataTagModel = new OEEDataTagModel();

        public OEEDataTagModel OEEDataTagModel
        {
            get { return oEEDataTagModel; }
            set
            {
                if(oEEDataTagModel != value)
                {
                    oEEDataTagModel = value;
                    RaisePropertyChanged(nameof(OEEDataTagModel));
                }
            }
        }

        private ObservableCollection<OEEReadOnlyModel> oEEReadOnlyList = new ObservableCollection<OEEReadOnlyModel>();

        public ObservableCollection<OEEReadOnlyModel> OEEReadOnlyList
        {
            get { return oEEReadOnlyList; }
            set
            {
                if(oEEReadOnlyList != value)
                {
                    oEEReadOnlyList = value;
                    RaisePropertyChanged(nameof(OEEReadOnlyList));
                }
            }
        }

        private ObservableCollection<OEETimeModel> oEETimeList = new ObservableCollection<OEETimeModel>();

        public ObservableCollection<OEETimeModel> OEETimeList
        {
            get { return oEETimeList; }
            set
            {
                if(oEETimeList != value)
                {
                    oEETimeList = value;
                    RaisePropertyChanged(nameof(OEETimeList));
                }
            }
        }

        private ObservableCollection<OEECountModel> oEECountList = new ObservableCollection<OEECountModel>();
        
        public ObservableCollection<OEECountModel> OEECountList
        {
            get { return oEECountList; }
            set
            {
                if(oEECountList != value)
                {
                    oEECountList = value;
                    RaisePropertyChanged(nameof(OEECountList));
                }
            }
        }

        //public void readOEEDataModelTag()
        //{
        //    foreach (var x in OEEDataList)
        //    {
        //        if (x.DataTagType == "DINT")
        //        {
        //            x.OEECount = keyenceDll.Read<int>(x.tag);
        //        }
        //    }
        //}

        #endregion PublicProperties
    }
}