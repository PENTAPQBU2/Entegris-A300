using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Main
{
    public class MainModel : ViewModelBase
    {
        #region PublicProperties

        private string mainTag { get; set; }

        public string MainTag
        {
            get { return mainTag; }
            set
            {
                if(mainTag != value)
                {
                    mainTag = value;
                    RaisePropertyChanged(nameof(MainTag));
                }
            }
        }

        private string mainKey = "Spare";

        public string MainKey
        {
            get { return mainKey; }
            set
            {
                if(mainKey != value)
                {
                    mainKey = value;
                    RaisePropertyChanged(nameof(MainKey));
                }
            }
        }

        private string mainName = "";

        public string MainName
        {
            get { return mainName; }
            set
            {
                if(mainName != value)
                {
                    mainName = value;
                    RaisePropertyChanged(nameof(MainName));
                }
            }
        }

        private object mainValue = null;

        public object MainValue
        {
            get { return mainValue; }
            set
            {
                if(mainValue != value)
                {
                    mainValue = value;
                    RaisePropertyChanged(nameof(MainValue));
                }
            }
        }

        private object moduleInitStatus = null;

        public object ModuleInitStatus
        {
            get { return moduleInitStatus; }
            set
            {
                if(moduleInitStatus != value)
                {
                    moduleInitStatus = value;
                    RaisePropertyChanged(nameof(ModuleInitStatus));
                }
            }
        }

        private object modulefontColor = null;

        public object ModulefontColor
        {
            get { return modulefontColor; }
            set
            {
                if(modulefontColor != value)
                {
                    modulefontColor = value;
                    RaisePropertyChanged(nameof(ModulefontColor));
                }
            }
        }

        private class UIset
        {
        }

        #endregion PublicProperties
    }
}