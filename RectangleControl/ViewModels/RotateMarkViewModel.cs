using System.Windows;
using System.Windows.Controls.Primitives;
using RectangleControl.Interfaces;
using RectangleControl.Selectors;

namespace RectangleControl.ViewModels
{
    public class RotateMarkViewModel : BindableBase, 
        IDraggableMark, 
        IDataTemplateMatcher<RectangleMarkTemplateSelector.InnerSelector>, 
        IStyleMatcher<RectangleMarkStyleSelector.InnerSelector>
    {
        #region IMark
        public DelegateCommand<DragStartedArgs> DragStartedCommand { get; set; }
        public DelegateCommand<DragDeltaArgs> DragDeltaCommand { get; set; }
        public DelegateCommand<DragCompletedEventArgs> DragCompletedCommand { get; set; }

        public event EventHandler<DragStartedArgs>? DragStarted;
        public event EventHandler<DragDeltaArgs>? DragDelta;
        public event EventHandler<DragCompletedEventArgs>? DragCompleted;

        public double Width { get { return _width; } set { SetProperty(ref _width, value); } }
        private double _width = 20;

        public double Offset => Width / 2;

        public double Angle { get { return _angle; } set { SetProperty(ref _angle, value); } }
        double _angle;

        public Point Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
            }
        }
        private Point _position;

        #endregion

        public double HandleLength
        {
            get { return _handleLength; }
            set
            {
                SetProperty(ref _handleLength, value);
            }
        }
        private double _handleLength;

        public RotateMarkViewModel(double handleLength)
        {
            _handleLength = handleLength;

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
            return selector.RotateTemplate;
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