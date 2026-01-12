using System.Windows.Controls.Primitives;

namespace RectangleControl.Interfaces
{
    public interface IDraggableMark : IMark
    {
        DelegateCommand<DragStartedArgs> DragStartedCommand { get; set; }
        DelegateCommand<DragDeltaArgs> DragDeltaCommand { get; set; }
        DelegateCommand<DragCompletedEventArgs> DragCompletedCommand { get; set; }

        event EventHandler<DragStartedArgs>? DragStarted;

        event EventHandler<DragDeltaArgs>? DragDelta;

        event EventHandler<DragCompletedEventArgs>? DragCompleted;
    }
}