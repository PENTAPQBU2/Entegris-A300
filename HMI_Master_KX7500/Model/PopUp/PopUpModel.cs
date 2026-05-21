using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Model.PopUp.Action;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.PopUp
{
    public class PopUpModel : ViewModelBase
    {
        #region PublicProperties
        private string popUpMessageKey = string.Empty;
        public string PopUpMessageKey
        {
            get { return popUpMessageKey; }
            set
            {
                if (popUpMessageKey != value)
                {
                    popUpMessageKey = value;
                    RaisePropertyChanged(nameof(PopUpMessageKey));
                    RaisePropertyChanged(nameof(PopUpMessage));
                }
            }
        }

        private string popUpMessage = string.Empty;
        [XmlIgnore]
        public string PopUpMessage
        {
            get { return popUpMessage; /*+ PopUpValue; */}
            set
            {
                if (popUpMessage != value)
                {
                    popUpMessage = value;
                    RaisePropertyChanged(nameof(PopUpMessage));
                }
            }
        }

        private bool popUp = false;
        [XmlIgnore]
        public bool PopUp
        {
            get { return popUp; }
            set
            {
                if (popUp != value)
                {
                    popUp = value;
                    RaisePropertyChanged(nameof(PopUp));
                }
            }
        }

        private bool popUpEnableAutoClose = false;
        public bool PopUpEnableAutoClose
        {
            get { return popUpEnableAutoClose; }
            set
            {
                if (popUpEnableAutoClose != value)
                {
                    popUpEnableAutoClose = value;
                    RaisePropertyChanged(nameof(PopUpEnableAutoClose));
                }
            }
        }

        private PopUpTagModel popUpTagModel = new PopUpTagModel();
        public PopUpTagModel PopUpTagModel
        {
            get { return popUpTagModel; }
            set
            {
                if (popUpTagModel != value)
                {
                    popUpTagModel = value;
                    RaisePropertyChanged(nameof(PopUpTagModel));
                }
            }
        }

        private ObservableCollection<PopUpActionModel> popUpActionList = new ObservableCollection<PopUpActionModel>();
        public ObservableCollection<PopUpActionModel> PopUpActionList
        {
            get { return popUpActionList; }
            set
            {
                if (popUpActionList != value)
                {
                    popUpActionList = value;
                    RaisePropertyChanged(nameof(PopUpActionList));
                }
            }
        }

        private string popUpValue = string.Empty;
        [XmlIgnore]
        public string PopUpValue
        {
            get { return popUpValue; }
            set
            {
                if (popUpValue != value)
                {
                    popUpValue = value;
                    RaisePropertyChanged(nameof(PopUpValue));
                    //RaisePropertyChanged(nameof(PopUpMessage));
                }
            }
        }

        private string popUpValueTag = string.Empty;
        public string PopUpValueTag
        {
            get { return popUpValueTag; }
            set
            {
                if (popUpValueTag != value)
                {
                    popUpValueTag = value;
                    RaisePropertyChanged(nameof(PopUpValueTag));
                }
            }
        }

        private int popUpValueTagLength = 0;
        public int PopUpValueTagLength
        {
            get { return popUpValueTagLength; }
            set
            {
                if (popUpValueTagLength != value)
                {
                    popUpValueTagLength = value;
                    RaisePropertyChanged(nameof(PopUpValueTagLength));
                }
            }
        }
        #endregion
    }
}
