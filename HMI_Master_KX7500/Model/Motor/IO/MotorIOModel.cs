using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace HMI_Master_KX7500.Model
{
    public class MotorIOModel : ViewModelBase
    {
        private List<MotorIOInputModel> motorIOInputList = new List<MotorIOInputModel>();
        public List<MotorIOInputModel> MotorIOInputList
        {
            get { return motorIOInputList; }
            set
            {
                if (motorIOInputList != value)
                {
                    motorIOInputList = value;
                    RaisePropertyChanged(nameof(MotorIOInputList));
                }
            }
        }

        private List<MotorIOOutputModel> motorIOOutputList = new List<MotorIOOutputModel>();
        public List<MotorIOOutputModel> MotorIOOutputList
        {
            get { return motorIOOutputList; }
            set
            {
                if (motorIOOutputList != value)
                {
                    motorIOOutputList = value;
                    RaisePropertyChanged(nameof(MotorIOOutputList));
                }
            }
        }
    }
}
