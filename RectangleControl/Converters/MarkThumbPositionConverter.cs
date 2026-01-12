using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace RectangleControl.Converters
{
    public class MarkThumbPositionConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length == 2 && values[0] is double pos && values[1] is double offset)
            {
                return pos - offset;
            }

            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}