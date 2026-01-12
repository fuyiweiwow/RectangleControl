using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RectangleControl.Interfaces;

namespace RectangleControl.Selectors
{
    public class RectangleMarkTemplateSelector : MarkupExtension
    {
        public class InnerSelector : DataTemplateSelector
        {
            public DataTemplate? ResizeTemplate { get; set; }
            public DataTemplate? RotateTemplate { get; set; }
            public DataTemplate? RotateHandleTemplate { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                if (item is IDataTemplateMatcher<InnerSelector> macher)
                {
                    return macher.MatchTemplate(this);
                }

                return base.SelectTemplate(item, container);
            }
        }

        private InnerSelector _selector = new InnerSelector();

        public DataTemplate? ResizeTemplate
        {
            get { return _selector.ResizeTemplate; }
            set { _selector.ResizeTemplate = value; }
        }

        public DataTemplate? RotateTemplate
        {
            get { return _selector.RotateTemplate; }
            set { _selector.RotateTemplate = value; }
        }

        public DataTemplate? RotateHandleTemplate
        {
            get { return _selector.RotateHandleTemplate; }
            set { _selector.RotateHandleTemplate = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _selector;
        }
    }
}