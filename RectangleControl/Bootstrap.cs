using System.Windows;
using RectangleControl.Views;

namespace RectangleControl
{
    public class Bootstrap : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }    
}
