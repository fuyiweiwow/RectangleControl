using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using RectangleControl.Interfaces;
using RectangleControl.Utils;

namespace RectangleControl.Converters
{
    public class DragArgsConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Point pt = new Point();
            if(value is RoutedEventArgs rea)
            {
                var d = rea.Source as DependencyObject;
                var canvas = d.FindParent<Canvas>();
                if (canvas is null)
                {
                    return null;
                }
                pt = Mouse.GetPosition(canvas);
            }

            if(value is DragDeltaEventArgs ddea)
            {
                return new DragDeltaArgs() { EventArgs = ddea , DragPoint = pt };
            }
            if(value is DragStartedEventArgs dsea)
            {
                return new DragStartedArgs() { EventArgs = dsea, DragPoint = pt };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }


}