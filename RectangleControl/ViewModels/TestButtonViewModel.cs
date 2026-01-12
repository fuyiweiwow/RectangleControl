namespace RectangleControl.ViewModels
{
    public class TestButtonViewModel : BindableBase
    {
        public string? Text { get; set; }    

        public DelegateCommand? Command { get; set; }

    }
}
    