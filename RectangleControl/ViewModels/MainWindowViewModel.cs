using System.Collections.ObjectModel;

namespace RectangleControl.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<object> CanvasItems { get { return _canvasItems; } set { SetProperty(ref _canvasItems, value); } }
        private ObservableCollection<object> _canvasItems = new();

        public ObservableCollection<TestButtonViewModel> TestButtons { get { return _testButtons; } set { SetProperty(ref _testButtons, value); } }
        private ObservableCollection<TestButtonViewModel> _testButtons = new();

        public DelegateCommand ResetCommand { get; set; }

        IEventAggregator _eventAggregator;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ResetCommand = new DelegateCommand(ResetCommandExec);

            TestButtons.Add(new TestButtonViewModel { Text = "Reset canvas", Command = ResetCommand });

            var rectVm = new MarkRectangleViewModel
            {
                X = 200,
                Y = 200,
                Width = 500,
                Height = 250,
            };
            CanvasItems.Add(rectVm);
        }

        private void ResetCommandExec()
        {
            var rect = CanvasItems.FirstOrDefault(p => p is MarkRectangleViewModel);
            if(rect is null)
            {
                return;   
            }
            _ = CanvasItems.Remove(rect);
            rect = new MarkRectangleViewModel
            {
                X = 200,
                Y = 200,
                Width = 500,
                Height = 250,
            };
            CanvasItems.Add(rect);
        }

    }
}