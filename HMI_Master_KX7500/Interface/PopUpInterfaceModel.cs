using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.PopUp;
using HMI_Master_KX7500.Service.PopUp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Interface
{
    public class PopUpInterfaceModel :ViewModelBase
    {
        #region PrivateFields
        private readonly PopUpService popUpService = new PopUpService();
        #endregion

        #region PublicProperties
        private ObservableCollection<PopUpModel> popUpList = new ObservableCollection<PopUpModel>();
        public ObservableCollection<PopUpModel> PopUpList
        {
            get { return popUpList; }
            set
            {
                if (popUpList != value)
                {
                    popUpList = value;
                    RaisePropertyChanged(nameof(PopUpList));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public PopUpInterfaceModel()
        {
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
        }
        #endregion

        #region PrivateMessengerMethods
        private void updateLanguage(LanguageModel model = null)
        {
            PopUpList.ToList().ForEach(x =>
            {
                x.PopUpMessage = HmiHelper.GetResource<string>(x.PopUpMessageKey);

                x.PopUpActionList.ToList().ForEach(y =>
                {
                    y.PopUpActionName = HmiHelper.GetResource<string>(y.PopUpActionKey);
                });
            });
        }
        #endregion
         
        #region PublicMethods
        public bool OffPopUp(string popUpActionTag)
        {
            bool value = false;
            if (PopUpList.ToList().Exists(x => x.PopUp == true && x.PopUpActionList.ToList().Exists(
                y => y.PopUpActionTagModel.PopUpActionTag == popUpActionTag)))
            {
                PopUpList.ToList().Find(x => x.PopUp == true && x.PopUpActionList.ToList().Exists(
                    y => y.PopUpActionTagModel.PopUpActionTag == popUpActionTag)).PopUp = false;
                
                if (!PopUpList.ToList().Exists(x => x.PopUp == true))
                {
                    value = true;
                }
            }
            
            return value;
        }
        #endregion
    }
}
