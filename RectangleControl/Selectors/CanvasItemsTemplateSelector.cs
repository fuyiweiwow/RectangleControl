using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RectangleControl.Interfaces;

namespace RectangleControl.Selectors
{
    public class CanvasItemsTemplateSelector : MarkupExtension
    {
        public class InnerSelector : DataTemplateSelector
        {
            public DataTemplate? MarkRectangleTemplate { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                if(item is IDataTemplateMatcher<InnerSelector> macher)
                {
                    return macher.MatchTemplate(this);
                }

                return base.SelectTemplate(item, container);
            }
        }

        private InnerSelector _selector = new InnerSelector();

        public DataTemplate? MarkRectangleTemplate
        {
            get { return _selector.MarkRectangleTemplate; }
            set { _selector.MarkRectangleTemplate = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _selector;
        }
    }


}