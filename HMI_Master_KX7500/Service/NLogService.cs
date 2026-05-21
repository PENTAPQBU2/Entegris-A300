//using HMI_Master_Keyence.CommunicationProtocols;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Interfaces;
using NLog;
using System;

namespace HMI_Master_KX7500.Services
{
    public class NLogService : ILog
    {
        #region PrivateFields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerKeyenceDll = LogManager.GetLogger("OPC");
        private static readonly Logger loggerCOMPort = LogManager.GetLogger("COMPort");

        #endregion PrivateFields

        #region PublicMethods

        public void COMPort(string className, string message)
        {
            MethodHelper.Action(className, () =>
            {
                loggerCOMPort.Info("{0}{1}{2}{3}", PunctuationHelper.VerticalBar, className,
                    PunctuationHelper.VerticalBar, message);
            });
        }
        //---------------------------------------------------------------

        public void Debug(string className, string message)
        {
            MethodHelper.Action(className, () =>
            {
                logger.Debug("{0}{1}{2}{3}{4}", PunctuationHelper.VerticalBar, PunctuationHelper.VerticalBar, className,
                    PunctuationHelper.VerticalBar, message);
            });
        }

        public void Error(string className, Exception exception)
        {
            MethodHelper.Action(className, () =>
            {
                logger.Error("{0}{1}{2}{3}", PunctuationHelper.VerticalBar, className,
                    PunctuationHelper.VerticalBar, exception);
            });
        }

        public void Error(string className, string message)
        {
            MethodHelper.Action(className, () =>
            {
                logger.Error("{0}{1}{2}{3}", PunctuationHelper.VerticalBar, className,
                    PunctuationHelper.VerticalBar, message);
            });
        }

        public void Event(string className, string message)
        {
            MethodHelper.Action(className, () =>
            {
                logger.Info("{0}{1}{2}{3}", PunctuationHelper.VerticalBar, className,
                    PunctuationHelper.VerticalBar, message);
            });
        }

        public void OPCTrace(string className, string message)
        {
            MethodHelper.Action(className, () =>
            {
                loggerKeyenceDll.Trace("{0}{1}{2}{3}", PunctuationHelper.VerticalBar,
                    className, PunctuationHelper.VerticalBar, message);
            });
        }

        #endregion PublicMethods
    }
}