using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.PopUp.Action
{
    public class PopUpActionTagModel : ViewModelBase
    {
        #region PublicProperties
        private string popUpActionTag = string.Empty;
        public string PopUpActionTag
        {
            get { return popUpActionTag; }
            set
            {
                if (popUpActionTag != value)
                {
                    popUpActionTag = value;
                    RaisePropertyChanged(nameof(PopUpActionTag));
                }
            }
        }
        #endregion
    }
}
