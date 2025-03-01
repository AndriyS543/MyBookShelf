using System;
using System.Globalization;
using System.Windows.Data;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converter that transforms a zero-based index into a one-based row number.
    /// This is useful for displaying row numbers in a UI.
    /// </summary>
    public class RowNumberConverter : IValueConverter
    {
        /// <summary>
        /// Converts a zero-based index (int) to a one-based row number.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return index + 1; // Add 1 to start from 1 instead of 0
            }
            return value;
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