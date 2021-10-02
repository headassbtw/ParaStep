using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace ParaStep.GraphicalErrorHandler
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                
                desktop.MainWindow = new MessageBox(GraphicalErrorHandler.Main.CaughtException.Message,Main.CaughtException.StackTrace,"Critical Error", MessageBox.MessageBoxButtons.Ok);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}