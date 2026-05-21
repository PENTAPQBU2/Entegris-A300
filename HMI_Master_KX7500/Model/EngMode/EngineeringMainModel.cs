using GalaSoft.MvvmLight;
using HMI_Master_KX7500.Models;
using System.Collections.Generic;

namespace HMI_Master_KX7500.Model
{
    public class EngineeringMainModel : ViewModelBase
    {
        private List<EngineeringGroupModel> engineeringGroupModel = new List<EngineeringGroupModel>();
        public List<EngineeringGroupModel> EngineeringGroupModel
        {
            get { return engineeringGroupModel; }
            set
            {
                if(engineeringGroupModel != value)
                {
                    engineeringGroupModel = value;
                    RaisePropertyChanged(nameof(EngineeringGroupModel));
                }
            }
        }
    }
}
