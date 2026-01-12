using System.Drawing;
using System.Windows;
using System.Windows.Media;
using RectangleControl.Interfaces;
using RectangleControl.Selectors;
using Point = System.Windows.Point;

namespace RectangleControl.ViewModels
{
    public class RotateHandle : BindableBase, 
        IMark, 
        IDataTemplateMatcher<RectangleMarkTemplateSelector.InnerSelector>, 
        IStyleMatcher<RectangleMarkStyleSelector.InnerSelector>
    {
        public double Length 
        {
            get { return _length; } 
            set
            { 
                SetProperty(ref _length, value); 
                RaisePropertyChanged(nameof(Width));
            } 
        }

        private double _length = 20;

        public Point Start 
        {
            get => _start; 
            set
            { 
                SetProperty(ref _start, value);
                RaisePropertyChanged(nameof(Position));
                RaisePropertyChanged(nameof(End));
            }
        }

        private Point _start;

        public Point End { get { return  _end; } set { SetProperty(ref _end, value); } }
        private Point _end;
       

        public bool IsVisible { get { return _isVisible; } set { SetProperty(ref _isVisible, value); } }
        bool _isVisible = true;

        #region IMark
        public double Width { get { return Length; } set { Length = value; } }

        public double Offset => 0;

        public double Angle { get { return _angle; } set { SetProperty(ref _angle, value); } }
        private double _angle;

        public Point Position { get { return Start; } set { Start = value; } }

        public void Update(Point initStart, double angle, Matrix mat)
        {
            Angle = angle;
            var initEnd = new Point(initStart.X, initStart.Y - Length);
            var curEnd = mat.Transform(initEnd);
            End = curEnd;
        }

        #endregion


        #region
        public DataTemplate? MatchTemplate(RectangleMarkTemplateSelector.InnerSelector selector)
        {
            return selector.RotateHandleTemplate;
        }

        #endregion

        #region IStyleMatcher
        public Style? MatchStyle(RectangleMarkStyleSelector.InnerSelector selector)
        {
            return selector.RotateHandleStyle;
        }

        #endregion

    }
}