using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Models
{
    public class CycleTimeHistoryModel : ViewModelBase
    {
        #region PublicProperties
        private List<CycleTimeHistoryTypeModel> cycleTimeHistoryTypeList = new List<CycleTimeHistoryTypeModel>();
        public List<CycleTimeHistoryTypeModel> CycleTimeHistoryTypeList
        {
            get { return cycleTimeHistoryTypeList; }
            set
            {
                if (cycleTimeHistoryTypeList != value)
                {
                    cycleTimeHistoryTypeList = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeList));
                    RaisePropertyChanged(nameof(CycleTimeHistoryTypeListView));
                    RaisePropertyChanged(nameof(CycleTimeHistorySeries));
                }
            }
        }

        private ICollectionView cycleTimeHistoryTypeListView = null;
        [XmlIgnore]
        public ICollectionView CycleTimeHistoryTypeListView
        {
            get
            {
                cycleTimeHistoryTypeListView = null;
                cycleTimeHistoryTypeListView = CollectionViewSource.GetDefaultView(CycleTimeHistoryTypeList);
                return cycleTimeHistoryTypeListView;
            }
        }

        private SeriesCollection cycleTimeHistorySeries = new SeriesCollection();
        [XmlIgnore]
        public SeriesCollection CycleTimeHistorySeries
        {
            get
            {
                cycleTimeHistorySeries = new SeriesCollection();

                CycleTimeHistoryTypeList.ForEach(x =>
                {
                    ChartValues<double> values = new ChartValues<double>();

                    x.CycleTimeHistoryTypeTimerList.ForEach(y =>
                    {
                        values.Add(y.CycleTimeHistoryTypeTimer);
                    });

                    cycleTimeHistorySeries.Add(new LineSeries
                    {
                        DataLabels = true,
                        Title = x.CycleTimeHistoryTypeName,
                        Values = values
                    });
                });

                return cycleTimeHistorySeries;
            }
        }

        private List<CycleTimeHistoryLabelModel> cycleTimeHistoryLabelList = new List<CycleTimeHistoryLabelModel>();
        public List<CycleTimeHistoryLabelModel> CycleTimeHistoryLabelList
        {
            get { return cycleTimeHistoryLabelList; }
            set
            {
                if (cycleTimeHistoryLabelList != value)
                {
                    cycleTimeHistoryLabelList = value;
                    RaisePropertyChanged(nameof(CycleTimeHistoryLabelList));
                    RaisePropertyChanged(nameof(CycleTimeHistoryLabelListView));
                    RaisePropertyChanged(nameof(CycleTimeHistoryLabels));
                }
            }
        }

        private ICollectionView cycleTimeHistoryLabelListView = null;
        [XmlIgnore]
        public ICollectionView CycleTimeHistoryLabelListView
        {
            get
            {
                cycleTimeHistoryLabelListView = null;
                cycleTimeHistoryLabelListView = CollectionViewSource.GetDefaultView(CycleTimeHistoryLabelList);
                return cycleTimeHistoryLabelListView;
            }
        }

        private ObservableCollection<string> cycleTimeHistoryLabels = new ObservableCollection<string>();
        [XmlIgnore]
        public ObservableCollection<string> CycleTimeHistoryLabels
        {
            get
            {
                cycleTimeHistoryLabels = new ObservableCollection<string>(
                    CycleTimeHistoryLabelList.Select(x => x.CycleTimeHistoryLabelName));
                return cycleTimeHistoryLabels;
            }
        }

        [XmlIgnore]
        public Func<double, string> CycleTimeHistoryLabelFormatter
        {
            get { return x => x.ToString("N1"); }
        }
        #endregion
    }
}