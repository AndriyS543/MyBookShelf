using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converts a boolean value to Visibility.
    /// True -> Visible, False -> Collapsed.
    /// </summary>
    public class InvertedBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
