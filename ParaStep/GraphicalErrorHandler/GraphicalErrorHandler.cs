using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;

namespace ParaStep.GraphicalErrorHandler
{
    public class Main
    {
        public static Exception CaughtException;
        
        public static void main(Exception e)
        {
            CaughtException = e;
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(new string[0]);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToTrace();
    }
}