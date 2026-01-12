using System.Windows;
using System.Windows.Controls.Primitives;

namespace RectangleControl.Interfaces
{
    public class DragStartedArgs : IDragArgs<DragStartedEventArgs>
    {
        public DragStartedEventArgs EventArgs { get; set; }
        public Point DragPoint { get; set; }

        RoutedEventArgs IDragArgs.EventArgs => EventArgs;
    }
}