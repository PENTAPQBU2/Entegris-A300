using GalaSoft.MvvmLight.Command;
using HMI_Master_KX7500.Model.OEE;

namespace HMI_Master_KX7500
{
    public class EventArgsConverter : IEventArgsConverter
    {
        #region PublicInterfaceMethods

        public object Convert(object value, object parameter)
        {
            EventArgsModel eventArgsModel = new EventArgsModel();

            eventArgsModel.EventArgs = value;
            eventArgsModel.Parameter = parameter;

            return eventArgsModel;
        }

        #endregion PublicInterfaceMethods
    }
}