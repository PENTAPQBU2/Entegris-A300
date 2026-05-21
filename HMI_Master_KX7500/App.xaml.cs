using NLog;
using System;
using System.Windows;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string msg = " Application_DispatcherUnhandledException Error" + Environment.NewLine + e.Exception.Message + e.Exception.StackTrace;
            logger.Error(e);
            logger.Debug(msg);
        }
    }
}