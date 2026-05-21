using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace HMI_Master_KX7500.Model.Frame
{
    public class ControlPanelModel : ViewModelBase
    {
        private List<ButtonModel> buttonModelList = new List<ButtonModel>();

        public List<ButtonModel> ButtonModelList
        {
            get { return buttonModelList; }
            set
            {
                if(buttonModelList != value)
                {
                    buttonModelList = value;
                    RaisePropertyChanged(nameof(ButtonModelList));
                }
            }
        }

        private List<ToggleModel> toggleModelList = new List<ToggleModel>();

        public List<ToggleModel> ToggleModelList
        {
            get { return toggleModelList; }
            set
            {
                if(toggleModelList != value)
                {
                    toggleModelList = value;
                    RaisePropertyChanged(nameof(ToggleModelList));
                }
            }
        }
    }
}