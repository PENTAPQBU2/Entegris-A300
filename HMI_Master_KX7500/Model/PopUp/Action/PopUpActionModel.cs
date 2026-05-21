using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.PopUp.Action
{
    public class PopUpActionModel :ViewModelBase
    {
        #region PublicCommandProperties
        [XmlIgnore]
        public ICommand PopUpActionCommand { get; set; }
        #endregion

        #region PublicProperties
        private string popUpActionKey = string.Empty;
        public string PopUpActionKey
        {
            get { return popUpActionKey; }
            set
            {
                if (popUpActionKey != value)
                {
                    popUpActionKey = value;
                    RaisePropertyChanged(nameof(PopUpActionKey));
                }
            }
        }

        private string popUpActionName = string.Empty;
        [XmlIgnore]
        public string PopUpActionName
        {
            get { return popUpActionName; }
            set
            {
                if (popUpActionName != value)
                {
                    popUpActionName = value;
                    RaisePropertyChanged(nameof(PopUpActionName));
                }
            }
        }

        private PopUpActionTagModel popUpActionTagModel = new PopUpActionTagModel();
        public PopUpActionTagModel PopUpActionTagModel
        {
            get { return popUpActionTagModel; }
            set
            {
                if (popUpActionTagModel != value)
                {
                    popUpActionTagModel = value;
                    RaisePropertyChanged(nameof(PopUpActionTagModel));
                }
            }
        }

        private bool popUpEnableInput = false;
        [XmlIgnore]
        public bool PopUpEnableInput
        {
            get { return popUpEnableInput; }
            set
            {
                if (popUpEnableInput != value)
                {
                    popUpEnableInput = value;
                    RaisePropertyChanged(nameof(PopUpEnableInput));
                }
            }
        }

        private short popUpInputQuantity = 0;
        [XmlIgnore]
        public short PopUpInputQuantity
        {
            get { return popUpInputQuantity; }
            set
            {
                if (popUpInputQuantity != value)
                {
                    popUpInputQuantity = value;
                    RaisePropertyChanged(nameof(PopUpInputQuantity));
                }
            }
        }
        #endregion
    }
}
