using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using HMI_Master_KX7500.Globals;

namespace HMI_Master_KX7500.Model.OEE
{
    public class OEEInterfaceModel : ViewModelBase
    {
        #region PrivateFields

        //private readonly OEEEditorView oEEEditorView = new OEEEditorView();

        #endregion PrivateFields

        #region PublicCommandProperties

        public ICommand EditOEECommand { get; set; }

        public ICommand OEEHourlyUphCartesianChartMouseMoveCommand { get; set; }

        #endregion PublicCommandProperties

        #region PublicProperties

        private bool allowEditOEE = false;

        public bool AllowEditOEE
        {
            get { return allowEditOEE; }
            set
            {
                if(allowEditOEE != value)
                {
                    allowEditOEE = value;
                    RaisePropertyChanged(nameof(AllowEditOEE));
                }
            }
        }

        private ObservableCollection<string> oEEHourlyUphLabels = new ObservableCollection<string>();

        public ObservableCollection<string> OEEHourlyUphLabels
        {
            get
            {
                oEEHourlyUphLabels = new ObservableCollection<string>(
                    OEEModel.OEEHourlyUphList.Select(x => x.OEEHourlyUphName));
                return oEEHourlyUphLabels;
            }
        }

        private double oEEHourlyUphXPointer = 0;

        public double OEEHourlyUphXPointer
        {
            get { return oEEHourlyUphXPointer; }
            set
            {
                if(oEEHourlyUphXPointer != value)
                {
                    oEEHourlyUphXPointer = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphXPointer));
                }
            }
        }

        public Func<double, string> OEEHourlyUphLabelFormatter
        {
            get { return x => x.ToString("N0"); }
        }

        private double oEEHourlyUphYPointer = 0;

        public double OEEHourlyUphYPointer
        {
            get { return oEEHourlyUphYPointer; }
            set
            {
                if(oEEHourlyUphYPointer != value)
                {
                    oEEHourlyUphYPointer = value;
                    RaisePropertyChanged(nameof(OEEHourlyUphYPointer));
                }
            }
        }

        private ChartValues<ObservableValue> oEEHourlyUphValues = new ChartValues<ObservableValue>();

        public ChartValues<ObservableValue> OEEHourlyUphValues
        {
            get
            {
                oEEHourlyUphValues = new ChartValues<ObservableValue>();

                OEEModel.OEEHourlyUphList.ToList().ForEach(x =>
                {
                    oEEHourlyUphValues.Add(new ObservableValue(x.OEEHourlyUph));
                });

                return oEEHourlyUphValues;
            }
        }

        private OEEModel oEEModel = new OEEModel();

        public OEEModel OEEModel
        {
            get { return oEEModel; }
            set
            {
                if(oEEModel != value)
                {
                    oEEModel = value;
                    RaisePropertyChanged(nameof(OEEModel));
                    updateLanguage();
                }
            }
        }

        #endregion PublicProperties

        #region Constructor

        public OEEInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeCommand()
        {
            EditOEECommand = new RelayCommand(editOEECommand);
            OEEHourlyUphCartesianChartMouseMoveCommand = new RelayCommand<EventArgsModel>(
                oEEHourlyUphCartesianChartMouseMoveCommand);
            updateLanguage();
        }

        private void initializeMessenger()
        {
            Messenger.Default.Register<OEEUpdate>(this, updateOEE);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }

        #endregion PrivateInitializeMethods

        #region PrivateCommandMethods

        private void editOEECommand()
        {
            //oEEEditorView.ShowDialog();
        }

        private void oEEHourlyUphCartesianChartMouseMoveCommand(EventArgsModel eventArgsModel)
        {
            MouseEventArgs mouseEventArgs = eventArgsModel.EventArgs as MouseEventArgs;
            CartesianChart cartesianChart = eventArgsModel.Parameter as CartesianChart;

            Point mouseCoordinates = mouseEventArgs.GetPosition(cartesianChart);
            Point chartValues = cartesianChart.ConvertToChartValues(mouseCoordinates);

            OEEHourlyUphYPointer = chartValues.Y;

            ISeriesView series = cartesianChart.Series[0];
            if(series.ActualValues.Count <= 0)
            {
                return;
            }
            ChartPoint closestPoint = series.ClosestPointTo(chartValues.X, AxisOrientation.X);

            if(closestPoint != null)
            {
                OEEHourlyUphXPointer = closestPoint.X;
            }
        }

        #endregion PrivateCommandMethods

        #region PrivateMessengerMethods

        private void updateOEE(OEEUpdate oEEUpdate)
        {
            switch(oEEUpdate)
            {
                case OEEUpdate.Uph:
                    RaisePropertyChanged(nameof(OEEHourlyUphValues));
                    break;

                default:
                    break;
            }
        }

        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditOEE = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.OEEEditor));
        //    });
        //}

        //private void updateLanguage(LanguageModel languageModel = null)
        private void updateLanguage(LanguageModel model = null)
        {
            OEEModel.OEEHourlyUphList.ToList().ForEach(x =>
            {
                x.OEEHourlyUphName = HmiHelper.GetResource<string>(x.OEEHourlyUphKey);
            });

            OEEModel.OEEDataList.ToList().ForEach(x =>
            {
                x.OEEDataName = HmiHelper.GetResource<string>(x.OEEDataKey);

                x.OEEReadOnlyList.ToList().ForEach(y =>
                {
                    //y.OEEReadOnlyName = y.OEEReadOnlyKey;
                    y.OEEReadOnlyName = HmiHelper.GetResource<string>(y.OEEReadOnlyKey);
                });

                x.OEETimeList.ToList().ForEach(y =>
                {
                    y.OEETimeName = HmiHelper.GetResource<string>(y.OEETimeKey);
                });

                x.OEECountList.ToList().ForEach(y =>
                {
                    //y.OEECountName = y.OEECountKey;
                    y.OEECountName = HmiHelper.GetResource<string>(y.OEECountKey);
                });
            });

            RaisePropertyChanged(nameof(OEEHourlyUphLabels));
        }

        //--------------------------------------------------------------------------------------------

        //private KeyenceDll keyenceDll = null;
        
        //public void readOEEModelTag()
        //{
        //    foreach (var x in OEEModel.OEEDataList)
        //    {
        //        if(x.OEEDataKey == "LotOEE")
        //        {
        //            foreach (var y in x.OEECountList)
        //            {
        //                //OEECountKey
        //                y.OEECountTagModel.OEECountTag = keyenceDll.Read<int>(PLCTag.Lot_OEE_Total_Quantity_Out).ToString();
        //            }
        //        }
        //    }
        //}

        #endregion PrivateMessengerMethods
    }
}