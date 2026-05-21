using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HMI_BYD_WeldingMachine
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region PublicInterfaceMethods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Visible;

            try
            {
                visibility = System.Convert.ToBoolean(value) ? Visibility.Collapsed : Visibility.Visible;
            }
            catch (Exception ex) { logger.Error(ex); }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}
