using System.Windows;
using System.Windows.Controls;

namespace RectangleControl.Interfaces
{
    public interface IDataTemplateMatcher<TSelector> where TSelector : DataTemplateSelector
    {
        DataTemplate? MatchTemplate(TSelector selector);

    } 
}