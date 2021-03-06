using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Gdk;
using Gtk;
using Application = Gtk.Application;
using UI = Gtk.Builder.ObjectAttribute;
using Window = Gtk.Window;

namespace ParaStep.GtkErrorHandler
{
    class MainWindow : Window
    {
        [UI] private Label _label0 = null;
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;
        private Exception _shownException;

        public MainWindow(Exception _exception) : this(new Builder("MainWindow.glade"))
        {
            _shownException = Program.threw;
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            this.WindowPosition = WindowPosition.Center;
            
            this.Icon = Pixbuf.LoadFromResource("ParaStep.GtkErrorHandler.Icon.bmp");
            this.Iconify();
            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;
            _label0.Activate();
            
            _label0.Text = Program.threw.GetType().ToString() + ": " + Program.threw.Message;
            _label1.Text = Program.threw.StackTrace;
            //fucking GTK
            base.Present();
            base.KeepAbove = true;
            base.Show();
            base.GrabFocus();
            this.Present();
            this.KeepAbove = true;
            this.Show();
            this.Stick();
            this.GrabFocus();
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void Button1_Clicked(object sender, EventArgs a)
        {
            Process game = System.Diagnostics.Process.GetProcessesByName("ParaStep").First();
            //AHAHHAHAHAHHAA
            game.Kill();
            _label1.Text = _shownException.StackTrace;
        }
    }
}