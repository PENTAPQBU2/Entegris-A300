using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.TrackingStatus.Module.Indicator
{
    public class TrackingStatusModuleStatusIndicatorModel : ViewModelBase
    {
        private string trackingStatusModuleKey = string.Empty;
        public string TrackingStatusModuleKey
        {
            get { return trackingStatusModuleKey; }
            set
            {
                if (trackingStatusModuleKey != value)
                {
                    trackingStatusModuleKey = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleKey));
                }
            }
        }
        #region PublicProperties
        private string trackingStatusModuleStatusIndicatorKey = string.Empty;
        public string TrackingStatusModuleStatusIndicatorKey
        {
            get { return trackingStatusModuleStatusIndicatorKey; }
            set
            {
                if (trackingStatusModuleStatusIndicatorKey != value)
                {
                    trackingStatusModuleStatusIndicatorKey = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorKey));
                }
            }
        }

        private string trackingStatusModuleStatusIndicatorName = string.Empty;
        [XmlIgnore]
        public string TrackingStatusModuleStatusIndicatorName
        {
            get { return trackingStatusModuleStatusIndicatorName; }
            set
            {
                if (trackingStatusModuleStatusIndicatorName != value)
                {
                    trackingStatusModuleStatusIndicatorName = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorName));
                }
            }
        }

        //
        private string trackingStatusModuleStatusIndicatorID = string.Empty;
        public string TrackingStatusModuleStatusIndicatorID
        {
            get { return trackingStatusModuleStatusIndicatorID; }
            set
            {
                if (trackingStatusModuleStatusIndicatorID != value)
                {
                    trackingStatusModuleStatusIndicatorID = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorID));
                }
            }
        }
        //

        private string trackingStatusModuleStatusIndicatorIndex = string.Empty;
        [XmlIgnore]
        public string TrackingStatusModuleStatusIndicatorIndex
        {
            get { return trackingStatusModuleStatusIndicatorIndex; }
            set
            {
                if (trackingStatusModuleStatusIndicatorIndex != value)
                {
                    trackingStatusModuleStatusIndicatorIndex = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorIndex));
                    Messenger.Default.Send(TrackingStatusUpdate.Index);
                }
            }
        }

        private string trackingStatusModuleStatusIndicatorColor = Brushes.Transparent.ToString();
        [XmlIgnore]
        public string TrackingStatusModuleStatusIndicatorColor
        {
            get { return trackingStatusModuleStatusIndicatorColor; }
            set
            {
                if (trackingStatusModuleStatusIndicatorColor != value)
                {
                    trackingStatusModuleStatusIndicatorColor = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorColor));
                }
            }
        }

        private double trackingStatusModuleStatusIndicatorRightPosition = 0;
        public double TrackingStatusModuleStatusIndicatorRightPosition
        {
            get { return trackingStatusModuleStatusIndicatorRightPosition; }
            set
            {
                if (trackingStatusModuleStatusIndicatorRightPosition != value)
                {
                    trackingStatusModuleStatusIndicatorRightPosition = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorRightPosition));
                }
            }
        }

        private double trackingStatusModuleStatusIndicatorTopPosition = 0;
        public double TrackingStatusModuleStatusIndicatorTopPosition
        {
            get { return trackingStatusModuleStatusIndicatorTopPosition; }
            set
            {
                if (trackingStatusModuleStatusIndicatorTopPosition != value)
                {
                    trackingStatusModuleStatusIndicatorTopPosition = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorTopPosition));
                }
            }
        }

        private double trackingStatusModuleStatusIndicatorLineX1 = 0;
        public double TrackingStatusModuleStatusIndicatorLineX1
        {
            get { return trackingStatusModuleStatusIndicatorLineX1; }
            set
            {
                if (trackingStatusModuleStatusIndicatorLineX1 != value)
                {
                    trackingStatusModuleStatusIndicatorLineX1 = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorLineX1));
                }
            }
        }

        private double trackingStatusModuleStatusIndicatorLineX2 = 0;
        public double TrackingStatusModuleStatusIndicatorLineX2
        {
            get { return trackingStatusModuleStatusIndicatorLineX2; }
            set
            {
                if (trackingStatusModuleStatusIndicatorLineX2 != value)
                {
                    trackingStatusModuleStatusIndicatorLineX2 = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorLineX2));
                }
            }
        }

        private double trackingStatusModuleStatusIndicatorLineY1 = 0;
        public double TrackingStatusModuleStatusIndicatorLineY1
        {
            get { return trackingStatusModuleStatusIndicatorLineY1; }
            set
            {
                if (trackingStatusModuleStatusIndicatorLineY1 != value)
                {
                    trackingStatusModuleStatusIndicatorLineY1 = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorLineY1));
                }
            }
        }

        private double trackingStatusModuleStatusIndicatorLineY2 = 0;
        public double TrackingStatusModuleStatusIndicatorLineY2
        {
            get { return trackingStatusModuleStatusIndicatorLineY2; }
            set
            {
                if (trackingStatusModuleStatusIndicatorLineY2 != value)
                {
                    trackingStatusModuleStatusIndicatorLineY2 = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorLineY2));
                }
            }
        }

        private TrackingStatusModuleStatusIndicatorTagModel trackingStatusModuleStatusIndicatorTagModel = new TrackingStatusModuleStatusIndicatorTagModel();
        public TrackingStatusModuleStatusIndicatorTagModel TrackingStatusModuleStatusIndicatorTagModel
        {
            get { return trackingStatusModuleStatusIndicatorTagModel; }
            set
            {
                if (trackingStatusModuleStatusIndicatorTagModel != value)
                {
                    trackingStatusModuleStatusIndicatorTagModel = value;
                    RaisePropertyChanged(nameof(TrackingStatusModuleStatusIndicatorTagModel));
                }
            }
        }

        [XmlIgnore]
        public ICommand trackingStatusTriggerCommand { get; set; }
        #endregion
    }
}
