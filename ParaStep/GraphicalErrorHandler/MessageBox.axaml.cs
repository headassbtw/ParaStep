using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ParaStep.GraphicalErrorHandler
{
    public class MessageBox : Window
    {
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public enum MessageBoxIcon
        {
            Info,
            Error,
            BT
        }

        
        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
            this.MaxHeight = this.Height;
            this.MaxWidth = this.Width;
            this.CanResize = false;
        }
        
        public MessageBox(string exceptionType, string stackTrace, string title, MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            AvaloniaXamlLoader.Load(this);
            Title = title;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CanResize = false;
            SizeToContent = SizeToContent.WidthAndHeight;
            this.FindControl<TextBlock>("Text").Text = exceptionType;
            TextBox StackTrace = this.FindControl<TextBox>("StackTrace");
            StackTrace.IsReadOnly = true;
            StackTrace.Text = stackTrace;
            

            var buttonPanel = this.FindControl<StackPanel>("Buttons");
            var res = MessageBoxResult.Ok;
            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button {Content = caption};
                btn.Click += (_, __) => { 
                    res = r;
                    this.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    res = r;
            }

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            //Bitmap bitmap = new Bitmap(assets.Open(new Uri($"avares://PlaylistToHitbloqPool/icons/{Enum.GetName(icon).ToLower()}.png")));
            
            Image img = this.FindControl<Image>("Image");
            
            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
                AddButton("Ok", MessageBoxResult.Ok, true);
            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
                AddButton("Cancel", MessageBoxResult.Cancel, true);

            
            var tcs = new TaskCompletionSource<MessageBoxResult>();
            
            this.Closed += delegate { tcs.TrySetResult(res); };
            //img.Source = bitmap;
            this.MaxHeight = this.Height;
            this.MaxWidth = this.Width;
            this.CanResize = false;
            AsyncTaskHandler.Handle(tcs.Task);
        }
    }
}