using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MyBookShelf.Converters
{
    public class NullToImageSourceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }

            // Якщо null або порожнє значення, повертаємо UnsetValue
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
