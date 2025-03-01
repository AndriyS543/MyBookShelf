using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converter that transforms a null or empty string into a Visibility value.
    /// If the value is null or empty, it returns Visibility.Visible.
    /// Otherwise, it returns Visibility.Collapsed.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string to a Visibility value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// ConvertBack is not implemented as one-way binding is expected.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
