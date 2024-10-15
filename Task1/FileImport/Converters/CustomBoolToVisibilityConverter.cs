using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace FileImport.Converters
{
    class CustomBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility.Visible)
                return true;
            else
                return false;

        }
    }
}
