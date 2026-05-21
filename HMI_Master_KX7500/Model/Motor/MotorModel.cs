using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model
{
    public class MotorModel : ViewModelBase
    {
        #region PublicCommandProperties
        private ICommand axisSelectionChangedCommand = null;
        [XmlIgnore]
        public ICommand AxisSelectionChangedCommand
        {
            get { return axisSelectionChangedCommand; }
            set
            {
                if (axisSelectionChangedCommand != value)
                {
                    axisSelectionChangedCommand = value;
                    RaisePropertyChanged(nameof(AxisSelectionChangedCommand));
                }
            }
        }
        #endregion

        #region PublicProperties
        private List<MotorAxisModel> axisList = new List<MotorAxisModel>();
        public List<MotorAxisModel> AxisList
        {
            get { return axisList; }
            set
            {
                if (axisList != value)
                {
                    axisList = value;
                    RaisePropertyChanged(nameof(AxisList));
                    RaisePropertyChanged(nameof(AxisListView));
                }
            }
        }

        private ICollectionView axisListView = null;
        [XmlIgnore]
        public ICollectionView AxisListView
        {
            get
            {
                axisListView = null;
                axisListView = CollectionViewSource.GetDefaultView(AxisList);
                return axisListView;
            }
        }
        #endregion
    }
}