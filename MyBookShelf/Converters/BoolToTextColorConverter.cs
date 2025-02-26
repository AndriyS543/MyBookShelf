using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace MyBookShelf.Converters
{
    public class BoolToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                var color = isSelected ? (Brush)new BrushConverter().ConvertFromString("#3D2012") : (Brush)new BrushConverter().ConvertFromString("#C7B1A7");
                return color;
            }
            return Brushes.Brown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
