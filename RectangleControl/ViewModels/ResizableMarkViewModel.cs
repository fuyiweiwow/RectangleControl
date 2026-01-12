using System.Windows;
using System.Windows.Controls.Primitives;
using RectangleControl.Enums;
using RectangleControl.Interfaces;
using RectangleControl.Selectors;

namespace RectangleControl.ViewModels
{
    public class ResizeMarkViewModel : BindableBase,
    IDraggableMark,
    IDataTemplateMatcher<RectangleMarkTemplateSelector.InnerSelector>, 
    IStyleMatcher<RectangleMarkStyleSelector.InnerSelector>
    {
        #region IMark
        public double Width { get { return _width; } set { SetProperty(ref _width, value); } }
        private double _width = 10;

        public double Offset { get { return Width / 2; } }
        
        public Point Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
            }
        }

        private Point _position;

        public double Angle { get { return _angle; } set { SetProperty(ref _angle, value); } }
        private double _angle;

        public ResizeDirection Direction { get; set; }
        public DelegateCommand<DragStartedArgs> DragStartedCommand { get; set; }
        public DelegateCommand<DragDeltaArgs> DragDeltaCommand { get; set; }
        public DelegateCommand<DragCompletedEventArgs> DragCompletedCommand { get; set; }

        public event EventHandler<DragStartedArgs>? DragStarted;

        public event EventHandler<DragDeltaArgs>? DragDelta;
        public event EventHandler<DragCompletedEventArgs>? DragCompleted;

        #endregion IMarkThumb

        public ResizeMarkViewModel()
        {
            DragStartedCommand = new DelegateCommand<DragStartedArgs>(DragStartedCommandExec);
            DragDeltaCommand = new DelegateCommand<DragDeltaArgs>(DragDeltaCommandExec);
        }

        private void DragDeltaCommandExec(DragDeltaArgs args)
        {
            DragDelta?.Invoke(this, args);
        }

        private void DragStartedCommandExec(DragStartedArgs args)
        {
            DragStarted?.Invoke(this, args);
        }

        #region IDataTemplateMatcher
        public DataTemplate? MatchTemplate(RectangleMarkTemplateSelector.InnerSelector selector)
        {
            return selector.ResizeTemplate;
        }
        #endregion

        #region IStyleMatcher
        public Style? MatchStyle(RectangleMarkStyleSelector.InnerSelector selector)
        {
            return selector.MarkStyle;
        }
        #endregion

    }
}