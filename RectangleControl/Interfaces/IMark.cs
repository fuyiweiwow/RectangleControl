using System.Windows;

namespace RectangleControl.Interfaces
{
    public interface IMark
    {
        double Width { get; set; }
        double Offset { get; } 
        double Angle { get; set; }
        Point Position { get; set; }
    }
}