using System.Windows;
using System.Windows.Controls.Primitives;

namespace RectangleControl.Interfaces
{
    public class DragDeltaArgs : IDragArgs<DragDeltaEventArgs>
    {
        public DragDeltaEventArgs EventArgs { get; set; }

        RoutedEventArgs IDragArgs.EventArgs => EventArgs;

        public Point DragPoint { get; set; }

    }

}