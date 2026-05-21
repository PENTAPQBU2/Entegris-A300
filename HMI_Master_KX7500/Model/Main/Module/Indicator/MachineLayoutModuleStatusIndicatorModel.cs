using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Main.Module.Indicator
{
    public class MachineLayoutModuleStatusIndicatorModel : ViewModelBase
    {
        public const string DefaultColor = "DefaultColor";

        #region PublicProperties

        private string machineLayoutModuleStatusIndicatorKey = string.Empty;

        public string MachineLayoutModuleStatusIndicatorKey
        {
            get { return machineLayoutModuleStatusIndicatorKey; }
            set
            {
                if(machineLayoutModuleStatusIndicatorKey != value)
                {
                    machineLayoutModuleStatusIndicatorKey = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorKey));
                }
            }
        }

        private string machineLayoutModuleStatusIndicatorName = string.Empty;

        [XmlIgnore]
        public string MachineLayoutModuleStatusIndicatorName
        {
            get { return machineLayoutModuleStatusIndicatorName; }
            set
            {
                if(machineLayoutModuleStatusIndicatorName != value)
                {
                    machineLayoutModuleStatusIndicatorName = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorName));
                }
            }
        }

        private string machineLayoutModuleStatusIndicatorIndex = string.Empty;

        [XmlIgnore]
        public string MachineLayoutModuleStatusIndicatorIndex
        {
            get { return machineLayoutModuleStatusIndicatorIndex; }
            set
            {
                if(machineLayoutModuleStatusIndicatorIndex != value)
                {
                    machineLayoutModuleStatusIndicatorIndex = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorIndex));
                    Messenger.Default.Send(MachineLayoutUpdate.Index);
                }
            }
        }

        //private string machineLayoutModuleStatusIndicatorColor = GetResource<SolidColorBrush>(DefaultColor).ToString();
        //[XmlIgnore]
        //public string MachineLayoutModuleStatusIndicatorColor
        //{
        //    get { return machineLayoutModuleStatusIndicatorColor; }
        //    set
        //    {
        //        if (machineLayoutModuleStatusIndicatorColor != value)
        //        {
        //            machineLayoutModuleStatusIndicatorColor = value;
        //            RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorColor));
        //        }
        //    }
        //}

        private string machineLayoutModuleStatusIndicatorColor = Brushes.Transparent.ToString();

        [XmlIgnore]
        public string MachineLayoutModuleStatusIndicatorColor
        {
            get { return machineLayoutModuleStatusIndicatorColor; }
            set
            {
                if(machineLayoutModuleStatusIndicatorColor != value)
                {
                    machineLayoutModuleStatusIndicatorColor = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorColor));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorRightPosition = 0;

        public double MachineLayoutModuleStatusIndicatorRightPosition
        {
            get { return machineLayoutModuleStatusIndicatorRightPosition; }
            set
            {
                if(machineLayoutModuleStatusIndicatorRightPosition != value)
                {
                    machineLayoutModuleStatusIndicatorRightPosition = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorRightPosition));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorTopPosition = 0;

        public double MachineLayoutModuleStatusIndicatorTopPosition
        {
            get { return machineLayoutModuleStatusIndicatorTopPosition; }
            set
            {
                if(machineLayoutModuleStatusIndicatorTopPosition != value)
                {
                    machineLayoutModuleStatusIndicatorTopPosition = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorTopPosition));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine1X1 = 0;

        public double MachineLayoutModuleStatusIndicatorLine1X1
        {
            get { return machineLayoutModuleStatusIndicatorLine1X1; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine1X1 != value)
                {
                    machineLayoutModuleStatusIndicatorLine1X1 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine1X1));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine2X1 = 0;

        public double MachineLayoutModuleStatusIndicatorLine2X1
        {
            get { return machineLayoutModuleStatusIndicatorLine2X1; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine2X1 != value)
                {
                    machineLayoutModuleStatusIndicatorLine2X1 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine2X1));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine1X2 = 0;

        public double MachineLayoutModuleStatusIndicatorLine1X2
        {
            get { return machineLayoutModuleStatusIndicatorLine1X2; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine1X2 != value)
                {
                    machineLayoutModuleStatusIndicatorLine1X2 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine1X2));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine2X2 = 0;

        public double MachineLayoutModuleStatusIndicatorLine2X2
        {
            get { return machineLayoutModuleStatusIndicatorLine2X2; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine2X2 != value)
                {
                    machineLayoutModuleStatusIndicatorLine2X2 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine2X2));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine1Y1 = 0;

        public double MachineLayoutModuleStatusIndicatorLine1Y1
        {
            get { return machineLayoutModuleStatusIndicatorLine1Y1; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine1Y1 != value)
                {
                    machineLayoutModuleStatusIndicatorLine1Y1 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine1Y1));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine2Y1 = 0;

        public double MachineLayoutModuleStatusIndicatorLine2Y1
        {
            get { return machineLayoutModuleStatusIndicatorLine2Y1; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine2Y1 != value)
                {
                    machineLayoutModuleStatusIndicatorLine2Y1 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine2Y1));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine1Y2 = 0;

        public double MachineLayoutModuleStatusIndicatorLine1Y2
        {
            get { return machineLayoutModuleStatusIndicatorLine1Y2; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine1Y2 != value)
                {
                    machineLayoutModuleStatusIndicatorLine1Y2 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine1Y2));
                }
            }
        }

        private double machineLayoutModuleStatusIndicatorLine2Y2 = 0;

        public double MachineLayoutModuleStatusIndicatorLine2Y2
        {
            get { return machineLayoutModuleStatusIndicatorLine2Y2; }
            set
            {
                if(machineLayoutModuleStatusIndicatorLine2Y2 != value)
                {
                    machineLayoutModuleStatusIndicatorLine2Y2 = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorLine2Y2));
                }
            }
        }

        private MachineLayoutModuleStatusIndicatorTagModel machineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel();

        public MachineLayoutModuleStatusIndicatorTagModel MachineLayoutModuleStatusIndicatorTagModel
        {
            get { return machineLayoutModuleStatusIndicatorTagModel; }
            set
            {
                if(machineLayoutModuleStatusIndicatorTagModel != value)
                {
                    machineLayoutModuleStatusIndicatorTagModel = value;
                    RaisePropertyChanged(nameof(MachineLayoutModuleStatusIndicatorTagModel));
                }
            }
        }

        #endregion PublicProperties
    }
}