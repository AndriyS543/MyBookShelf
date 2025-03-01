using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converts a boolean value to a text color.
    /// True -> Dark Brown (#3D2012), False -> Light Brown (#C7B1A7).
    /// </summary>
    public class BoolToTextColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a corresponding text color.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                var colorString = isSelected ? "#3D2012" : "#C7B1A7";
                var brush = new BrushConverter().ConvertFromString(colorString) as Brush;

                return brush ?? Brushes.Brown; // Default to Brown if conversion fails
            }
            return Brushes.Brown; // Default color if value is not a boolean
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
