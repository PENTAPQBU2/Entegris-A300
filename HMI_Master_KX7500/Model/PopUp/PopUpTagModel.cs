using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.PopUp
{
    public class PopUpTagModel : ViewModelBase
    {
        #region PublicProperties
        private string popUpTag = string.Empty;
        public string PopUpTag
        {
            get { return popUpTag; }
            set
            {
                if (popUpTag != value)
                {
                    popUpTag = value;
                    RaisePropertyChanged(nameof(PopUpTag));
                }
            }
        }
        #endregion
    }
}
