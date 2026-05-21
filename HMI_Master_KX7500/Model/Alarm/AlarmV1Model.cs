using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Alarm
{
    public class AlarmV1Model : ViewModelBase
    {
        private AlarmType alarmType;

        private string alarmModule = "Spare";

        private string alarmCode = "0";

        private string alarmMessageEnglish = "Spare";

        private string alarmActionEnglish = "Please contact administrator";

        private string alarmMessageChinese = "备用";

        private string alarmActionChinese = "请联系管理员";

        public AlarmType AlarmType
        {
            get
            {
                return alarmType;
            }
            set
            {
                if(alarmType != value)
                {
                    alarmType = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmType");
                }
            }
        }

        public string AlarmModule
        {
            get
            {
                return alarmModule;
            }
            set
            {
                if(alarmModule != value)
                {
                    alarmModule = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmModule");
                }
            }
        }

        public string AlarmCode
        {
            get
            {
                return alarmCode;
            }
            set
            {
                if(alarmCode != value)
                {
                    alarmCode = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmCode");
                }
            }
        }

        public string AlarmMessageEnglish
        {
            get
            {
                return alarmMessageEnglish;
            }
            set
            {
                if(alarmMessageEnglish != value)
                {
                    alarmMessageEnglish = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmMessageEnglish");
                }
            }
        }

        public string AlarmActionEnglish
        {
            get
            {
                return alarmActionEnglish;
            }
            set
            {
                if(alarmActionEnglish != value)
                {
                    alarmActionEnglish = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmActionEnglish");
                }
            }
        }

        public string AlarmMessageChinese
        {
            get
            {
                return alarmMessageChinese;
            }
            set
            {
                if(alarmMessageChinese != value)
                {
                    alarmMessageChinese = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmMessageChinese");
                }
            }
        }

        public string AlarmActionChinese
        {
            get
            {
                return alarmActionChinese;
            }
            set
            {
                if(alarmActionChinese != value)
                {
                    alarmActionChinese = value;
                    ((ObservableObject)this).RaisePropertyChanged("AlarmActionChinese");
                }
            }
        }
    }
}