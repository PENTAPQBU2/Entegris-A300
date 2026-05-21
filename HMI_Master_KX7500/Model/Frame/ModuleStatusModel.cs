using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.Frame
{
    public class ModuleStatusModel : ViewModelBase
    {
        private string moduleStatus_TopVisionStationNode = string.Empty;

        public string ModuleStatus_TopVisionStationNode
        {
            get { return moduleStatus_TopVisionStationNode; }
            set
            {
                if(moduleStatus_TopVisionStationNode != value)
                {
                    moduleStatus_TopVisionStationNode = value;
                    RaisePropertyChanged(nameof(ModuleStatus_TopVisionStationNode));
                }
            }
        }

        private string moduleStatus_LeftStageNode = string.Empty;

        public string ModuleStatus_LeftStageNode
        {
            get { return moduleStatus_LeftStageNode; }
            set
            {
                if(moduleStatus_LeftStageNode != value)
                {
                    moduleStatus_LeftStageNode = value;
                    RaisePropertyChanged(nameof(ModuleStatus_LeftStageNode));
                }
            }
        }

        private string moduleStatus_WelderStationNode = string.Empty;

        public string ModuleStatus_WelderStationNode
        {
            get { return moduleStatus_WelderStationNode; }
            set
            {
                if(moduleStatus_WelderStationNode != value)
                {
                    moduleStatus_WelderStationNode = value;
                    RaisePropertyChanged(nameof(ModuleStatus_WelderStationNode));
                }
            }
        }

        private string moduleStatus_RightStageNode = string.Empty;

        public string ModuleStatus_RightStageNode
        {
            get { return moduleStatus_RightStageNode; }
            set
            {
                if(moduleStatus_RightStageNode != value)
                {
                    moduleStatus_RightStageNode = value;
                    RaisePropertyChanged(nameof(ModuleStatus_RightStageNode));
                }
            }
        }
    }
}