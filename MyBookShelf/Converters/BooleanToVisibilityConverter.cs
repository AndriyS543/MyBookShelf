using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converts a boolean value to Visibility.
    /// True -> Visible, False -> Collapsed.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a Visibility value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed; // Default case if value is not a boolean
        }

        /// <summary>
        /// Converts a Visibility value back to a boolean.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false; // Default case if value is not Visibility
        }
    }
}
