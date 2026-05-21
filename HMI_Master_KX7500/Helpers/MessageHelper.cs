namespace HMI_Master_KX7500.Helpers
{
    public static class MessageHelper
    {
        #region PublicMethods

        public static string AreYouSureYouWantTo(string message)
        {
            return $"Are you sure you want to {message}?";
        }

        public static string CannotBeNullOrEmpty(string message)
        {
            return $"{message} cannot be null or empty.";
        }

        public static string DataChange(string message)
        {
            return $"Data change <<< {message}.";
        }

        public static string Done(string message)
        {
            return $"{message} done.";
        }

        public static string Exists(string message)
        {
            return $"{message} exists.";
        }

        public static string FailTo(string message)
        {
            return $"Fail to {message}.";
        }

        public static string IsAlreadyRunning(string message)
        {
            return $"{message} is already running.";
        }

        public static string KeyValue(string key, object value)
        {
            return $"[{key}]=[{value}]";
        }

        public static string Read(string message)
        {
            return $"Read <<< {message}.";
        }

        public static string Repeated(string message)
        {
            return $"{message} repeated.";
        }

        public static string UserClickedButton(string button)
        {
            return $"User clicked {button} button.";
        }

        public static string UserClickedToggleButton(string toggleButton)
        {
            return $"User clicked {toggleButton} toggleButton.";
        }

        public static string UserSelectedAxis(string axis)
        {
            return $"User selected {axis} axis.";
        }

        public static string UserSelectedLanguage(string language)
        {
            return $"User selected {language} language.";
        }

        public static string UserSelectedMenu(string menu)
        {
            return $"User selected {menu} menu.";
        }

        public static string UserSelectedRecipe(string recipe)
        {
            return $"User selected {recipe} recipe.";
        }

        public static string Write(string message)
        {
            return $"Write >>> {message}.";
        }

        #endregion PublicMethods
    }
}