using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500
{
    public class IOModel : ViewModelBase
    {
        private ICommand ioSelectionChangedCommand = null;
        [XmlIgnore]
        public ICommand IOSelectionChangedCommand
        {
            get { return ioSelectionChangedCommand; }
            set 
            {
                if(ioSelectionChangedCommand!=value)
                {
                    ioSelectionChangedCommand = value;
                    RaisePropertyChanged(nameof(IOSelectionChangedCommand));
                }
            }
        }

        private int ioSelectedIndex = 0;
        public int IOSelectedIndex
        {
            get { return ioSelectedIndex; }
            set 
            {
                if(ioSelectedIndex!=value)
                {
                    ioSelectedIndex = value;
                    RaisePropertyChanged(nameof(IOSelectedIndex));
                }
            }
        }

        private List<IOItemsModel> ioItemsModels = new List<IOItemsModel>();
        public List<IOItemsModel> IOItemsModels
        {
            get { return ioItemsModels; }
            set 
            {
                if(ioItemsModels!=value)
                {
                    ioItemsModels = value;
                    RaisePropertyChanged(nameof(IOItemsModels));
                    RaisePropertyChanged(nameof(IOListView));
                }
            }
        }

        private ICollectionView ioListView = null;
        public ICollectionView IOListView
        {
            get
            {
                ioListView = null;
                ioListView = CollectionViewSource.GetDefaultView(IOItemsModels);
                return ioListView;
            }
        }
    }
}
