using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converts a boolean value to a text decoration.
    /// True -> Underline, False -> No decoration.
    /// </summary>
    public class BoolToTextDecorationConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a TextDecoration value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? TextDecorations.Underline : new TextDecorationCollection();
            }
            return new TextDecorationCollection(); // Default case if value is not a boolean
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
