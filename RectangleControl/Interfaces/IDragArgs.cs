using System.Windows;

namespace RectangleControl.Interfaces
{
    public interface IDragArgs
    {
        Point DragPoint { get; set; }
        RoutedEventArgs EventArgs { get; }
    }

    public interface IDragArgs<out TEventArgs> : IDragArgs
        where TEventArgs : RoutedEventArgs
    {
        new TEventArgs EventArgs { get; }
    }
}