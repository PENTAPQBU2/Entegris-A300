using System;
using System.Windows;

namespace HMI_Master_KX7500.Helpers
{
    public static class MethodHelper
    {
        #region PublicMethods

        public static void Action(string className, Action action)
        {
            try
            {
                action();
            }
            catch(Exception exception)
            {
                LogHelper.Error(className, exception);
            }
        }

        public static int Action(string className, Func<int> action)
        {
            try
            {
                return action();
            }
            catch(Exception exception)
            {
                LogHelper.Error(className, exception);
                return 0;
            }
        }

        public static double Action(string className, Func<double> action)
        {
            try
            {
                return action();
            }
            catch(Exception exception)
            {
                LogHelper.Error(className, exception);
                return 0;
            }
        }

        public static void Action(string className, string name, Action action)
        {
            try
            {
                LogHelper.StartTo(className, name);

                action();

                LogHelper.Done(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailTo(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static bool Action(string className, string name, Func<bool> action)
        {
            bool value = false;

            try
            {
                LogHelper.StartTo(className, name);

                value = action();
            }
            catch(Exception exception)
            {
                LogHelper.Error(className, exception);
            }

            if(value)
            {
                LogHelper.Done(className, name);
            }
            else
            {
                LogHelper.FailTo(className, name);
            }

            return value;
        }

        public static void Action(string className, string name, Action action1, Action action2)
        {
            try
            {
                LogHelper.StartTo(className, name);

                action1();
                action2();

                LogHelper.Done(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailTo(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static void Execute(string className, string name, Action action, string eventMessage = "")
        {
            try
            {
                if(!string.IsNullOrEmpty(eventMessage))
                {
                    LogHelper.Event(className, eventMessage);
                }

                LogHelper.StartToExecute(className, name);

                action();

                LogHelper.ExecuteDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToExecute(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static void Execute(string className, string name, string messageBoxText, Action action,
            string eventMessage = "", MessageBoxResult messageBoxResult = MessageBoxResult.Yes,
            string messageBoxCaption = nameof(MessageBoxImage.Question),
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo,
            MessageBoxImage messageBoxImage = MessageBoxImage.Question,
            MessageBoxResult messageBoxDefaultResult = MessageBoxResult.No,
            MessageBoxOptions messageBoxOptions = MessageBoxOptions.DefaultDesktopOnly)
        {
            try
            {
                if(!string.IsNullOrEmpty(eventMessage))
                {
                    LogHelper.Event(className, eventMessage);
                }

                LogHelper.StartToExecute(className, name);

                if(MessageBoxHelper.ShowMessageBox(messageBoxText, messageBoxCaption, messageBoxButton,
                    messageBoxImage, messageBoxDefaultResult, messageBoxOptions) == messageBoxResult)
                {
                    action();
                }

                LogHelper.ExecuteDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToExecute(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static void Get(string className, string name, Action action)
        {
            try
            {
                LogHelper.StartToGet(className, name);

                action();

                LogHelper.GetDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToGet(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static bool Get(string className, string name, Func<bool> action)
        {
            bool value = false;

            try
            {
                LogHelper.StartToGet(className, name);

                value = action();
            }
            catch(Exception exception)
            {
                LogHelper.Error(className, exception);
            }

            if(value)
            {
                LogHelper.GetDone(className, name);
            }
            else
            {
                LogHelper.FailToGet(className, name);
            }

            return value;
        }

        public static void Initialize(string className, string name)
        {
            try
            {
                LogHelper.StartToInitialize(className, name);
                LogHelper.InitializeDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToInitialize(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static void Initialize(string className, string name, Action action)
        {
            try
            {
                LogHelper.StartToInitialize(className, name);

                action();

                LogHelper.InitializeDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToInitialize(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static void Initialize(string className, string name, Action action1, Action action2)
        {
            try
            {
                LogHelper.StartToInitialize(className, name);

                action1();
                action2();

                LogHelper.InitializeDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToInitialize(className, name);
                LogHelper.Error(className, exception);
            }
        }

        public static void Initialize(string className, string name, Action action1, Action action2, Action action3)
        {
            try
            {
                LogHelper.StartToInitialize(className, name);

                action1();
                action2();
                action3();

                LogHelper.InitializeDone(className, name);
            }
            catch(Exception exception)
            {
                LogHelper.FailToInitialize(className, name);
                LogHelper.Error(className, exception);
            }
        }

        #endregion PublicMethods
    }
}