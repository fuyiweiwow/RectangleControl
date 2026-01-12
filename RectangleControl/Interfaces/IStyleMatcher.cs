using System.Windows;
using System.Windows.Controls;

namespace RectangleControl.Interfaces
{
    public interface IStyleMatcher<TSelector> where TSelector : StyleSelector
    {
        Style? MatchStyle(TSelector selector);
    }
}