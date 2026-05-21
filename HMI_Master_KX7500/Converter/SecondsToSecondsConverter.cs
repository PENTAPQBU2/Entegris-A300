using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMI_Master_KX7500
{
    public class SecondsToSecondsConverter : IValueConverter
    {
        #region PublicInterfaceMethods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int seconds = 0;
            double _seconds = System.Convert.ToDouble(value);
            seconds = TimeSpan.FromSeconds(_seconds).Seconds;
            return seconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion PublicInterfaceMethods
    }
}