using NLog;
using System;
using System.Threading;
using System.Windows;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        // Only one HMI may run: every copy polls the PLC and tracks the shells by itself
        // (FOUP CSV got 3 running numbers for 1 shell with 3 copies open), only one can open the torque driver COM port
        private static Mutex singleInstanceMutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            singleInstanceMutex = new Mutex(true, "HMI_SSDC_PIM_Entegris_A300", out bool firstInstance);
            if (!firstInstance)
            {
                logger.Warn("HMI already running, second copy closed");
                // DefaultDesktopOnly: not owned by the splash screen, which would close the message box when it fades
                MessageBox.Show("HMI is already running.", "HMI", MessageBoxButton.OK, MessageBoxImage.Warning,
                    MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                Shutdown();
                return;
            }
            base.OnStartup(e);

            // Main window opened here instead of StartupUri, so a second copy never creates it (PLC polling)
            new Modules.Frame.FrameView().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try { singleInstanceMutex?.ReleaseMutex(); } catch (ApplicationException) { } // not owned (second copy)
            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string msg = " Application_DispatcherUnhandledException Error" + Environment.NewLine + e.Exception.Message + e.Exception.StackTrace;
            logger.Error(e);
            logger.Debug(msg);
        }
    }
}