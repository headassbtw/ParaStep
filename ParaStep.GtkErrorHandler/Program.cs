using System;
using Gtk;

namespace ParaStep.GtkErrorHandler
{
    public class Program
    {
        public static Exception threw;
        [STAThread]
        public static void Main(string[] args)
        {
            
        }

        public static void ShowError(Exception _exception)
        {
            CrashDumps.Save(_exception);
            Application.Init();
            threw = _exception;
            var app = new Application("org.ParaStep.GtkErrorHandler.ParaStep.GtkErrorHandler",
                GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow(_exception);
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}