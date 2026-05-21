using System;

namespace HMI_Master_KX7500.Interfaces
{
    public interface ILog
    {
        #region PublicMethods

        void COMPort(string className, string message);
        //-------------------------------------------

        void Debug(string className, string message);

        void Error(string className, Exception exception);

        void Error(string className, string message);

        void Event(string className, string message);

        void OPCTrace(string className, string message);

        #endregion PublicMethods
    }
}