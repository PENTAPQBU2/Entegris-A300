using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMI_Master_KX7500
{
    public class OpacityBooleanConverter : IValueConverter
    {
        #region PublicInterfaceMethods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double opacity = 1.0;
            if(!string.IsNullOrEmpty(value.ToString()))
            {
                if(System.Convert.ToBoolean(value))
                {
                    opacity = 1;
                }
                else
                {
                    opacity = 0.1;
                }
            }
            else
            {
                opacity = 0.1;
            }
            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion PublicInterfaceMethods
    }
}