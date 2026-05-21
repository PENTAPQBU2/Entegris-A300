using System;
using System.Windows;

namespace HMI_Master_KX7500.Helpers
{
    public static class HmiHelper
    {
        #region PublicFields

        public const string DefaultColor = "DefaultColor";
        public const string DefaultLanguage = "English";
        public const string ChineseSimplified = "ChineseSimplified";
        public const string IsInDesignMode = "IsInDesignMode";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public const string ShortDateFormat = "yyyy-MM-dd";
        public const string LongDateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff";

        #endregion PublicFields

        #region PublicMethods

        public static T GetResource<T>(string key)
        {
            var value = default(T);
            try
            {
                if(key != null)
                {
                    //MethodHelper.Action(nameof(HmiHelper), nameof(GetResource), () =>
                    //{
                    //    LogHelper.KeyValue(nameof(HmiHelper), nameof(key), key);

                    //    value = (T)Application.Current.TryFindResource(key);
                    //});

                    //LogHelper.KeyValue(nameof(HmiHelper), nameof(value), value);
                    value = (T)Application.Current.TryFindResource(key);
                }
                if(typeof(T) == typeof(string))
                {
                    if(value != null && string.IsNullOrEmpty(value.ToString()))
                    {
                        return (T)Convert.ChangeType(key, typeof(T));
                    }
                }
            }
            catch(Exception ex)
            {
                //LogHelper.Error(nameof(HmiHelper), $"Exception : {ex}, Key : {key}");
            }
            return value;
        }

        #endregion PublicMethods
    }
}