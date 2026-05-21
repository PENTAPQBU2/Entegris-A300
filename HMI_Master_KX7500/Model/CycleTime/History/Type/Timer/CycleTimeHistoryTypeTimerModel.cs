using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeHistoryTypeTimerModel : ViewModelBase
    {
        #region PublicProperties
        private double cycleTimeHistoryTypeTimer = 0;
        [XmlIgnore]
        public double CycleTimeHistoryTypeTimer
        {
            get { return cycleTimeHistoryTypeTimer; }
            set
            {
                value = Math.Round(Convert.ToDouble(value), 1);

                if (cycleTimeHistoryTypeTimer != value)
                {
                    cycleTimeHistoryTypeTimer = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeTimer));
                    //Messenger.Default.Send(CycleTimeUpdate.Timer);
                    Messenger.Default.Send(this);
                }
            }
        }

        private CycleTimeHistoryTypeTimerTagModel cycleTimeHistoryTypeTimerTagModel = new CycleTimeHistoryTypeTimerTagModel();
        public CycleTimeHistoryTypeTimerTagModel CycleTimeHistoryTypeTimerTagModel
        {
            get { return cycleTimeHistoryTypeTimerTagModel; }
            set
            {
                if (cycleTimeHistoryTypeTimerTagModel != value)
                {
                    cycleTimeHistoryTypeTimerTagModel = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeTimerTagModel));
                }
            }
        }
        #endregion
    }
}