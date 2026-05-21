using System.Windows;

namespace HMI_Master_KX7500.Helpers
{
    public static class MessageBoxHelper
    {
        #region PublicMethods

        public static MessageBoxResult ShowMessageBox(string messageBoxText,
            string messageBoxCaption = nameof(MessageBoxImage.Information),
            MessageBoxButton messageBoxButton = MessageBoxButton.OK,
            MessageBoxImage messageBoxImage = MessageBoxImage.Information,
            MessageBoxResult messageBoxDefaultResult = MessageBoxResult.OK,
            MessageBoxOptions messageBoxOptions = MessageBoxOptions.DefaultDesktopOnly)
        {
            MessageBoxResult messageBoxResult = messageBoxDefaultResult;

            MethodHelper.Action(nameof(MessageBoxHelper), nameof(ShowMessageBox), () =>
            {
                LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxText), messageBoxText);
                LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxCaption), messageBoxCaption);
                LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxButton), messageBoxButton);
                LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxImage), messageBoxImage);
                LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxDefaultResult), messageBoxDefaultResult);
                LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxOptions), messageBoxOptions);

                messageBoxResult = MessageBox.Show(messageBoxText, messageBoxCaption,
                    messageBoxButton, messageBoxImage, messageBoxDefaultResult, messageBoxOptions);
            });

            LogHelper.KeyValue(nameof(MessageBoxHelper), nameof(messageBoxResult), messageBoxResult);
            return messageBoxResult;
        }

        #endregion PublicMethods
    }
}