using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RectangleControl.Interfaces;

namespace RectangleControl.Selectors
{
    public class RectangleMarkStyleSelector : MarkupExtension
    {
        public class InnerSelector : StyleSelector
        {
            public Style? MarkStyle{ get; set; }
            public Style? RotateHandleStyle { get; set; }

            public override Style SelectStyle(object item, DependencyObject container)
            {
                if(item is IStyleMatcher<InnerSelector> m)
                {
                    return m.MatchStyle(this);
                }

                return base.SelectStyle(item, container);
            }
        }

        private InnerSelector _selector = new InnerSelector();

        public Style? MarkStyle
        { 
            get { return _selector.MarkStyle; } 
            set { _selector.MarkStyle = value; } 
        }

        public Style? RotateHandleStyle
        {
            get { return _selector.RotateHandleStyle; }
            set { _selector.RotateHandleStyle = value; }
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _selector;
        }
    }
}