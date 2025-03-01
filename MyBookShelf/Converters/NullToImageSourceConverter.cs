using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MyBookShelf.Converters
{
    /// <summary>
    /// Converter that transforms a string path into an ImageSource.
    /// If the path is null or empty, it returns DependencyProperty.UnsetValue.
    /// </summary>
    public class NullToImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string file path to a BitmapImage.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }

            // If the path is null or empty, return UnsetValue to indicate no valid image.
            return DependencyProperty.UnsetValue;
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
