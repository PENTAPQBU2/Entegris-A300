using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.OEE.Data;
using HMI_Master_KX7500.Model.OEE.HourlyUPH;
using System.Collections.ObjectModel;

namespace HMI_Master_KX7500.Model.OEE
{
    public class OEEModel : ViewModelBase
    {
        #region PublicProperties

        private ObservableCollection<OEEHourlyUphModel> oEEHourlyUphList = new ObservableCollection<OEEHourlyUphModel>();

        public ObservableCollection<OEEHourlyUphModel> OEEHourlyUphList
        {
            get { return oEEHourlyUphList; }
            set
            {
                if(oEEHourlyUphList != value)
                {
                    oEEHourlyUphList = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphList));
                }
            }
        }

        private ObservableCollection<OEEDataModel> oEEDataList = new ObservableCollection<OEEDataModel>();

        public ObservableCollection<OEEDataModel> OEEDataList
        {
            get { return oEEDataList; }
            set
            {
                if(oEEDataList != value)
                {
                    oEEDataList = value;
                    RaisePropertyChanged(nameof(OEEDataList));
                }
            }
        }

        #endregion PublicProperties
    }
}