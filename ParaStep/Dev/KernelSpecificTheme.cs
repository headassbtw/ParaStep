using System;
using RadLine;
using Spectre.Console;

namespace dotnet_repl
{
    public class Theme
    {
        public Style AnnouncementTextStyle { get; set; } = new(Color.SandyBrown);

        public Style AnnouncementBorderStyle { get; set; } = new(Color.Aqua);

        public static Theme Default { get; set; } = new();
    }

    public abstract class KernelSpecificTheme : Theme
    {
        public abstract Style AccentStyle { get; }

        public Style ErrorOutputBorderStyle { get; set; } = new(Color.Red);

        public Style SuccessOutputBorderStyle { get; set; } = new(Color.Green);

        public abstract string PromptText { get; }

        public virtual string KernelDisplayName => PromptText;

        public virtual ILineEditorPrompt Prompt => new DelegatingPrompt(
            $"[{AnnouncementTextStyle.Foreground}]{PromptText} [/][{Decoration.Bold} {AccentStyle.Foreground} {Decoration.SlowBlink}]>[/]",
            $"[{Decoration.Bold} {AccentStyle.Foreground} {Decoration.SlowBlink}] ...[/]");

        public IStatusMessageGenerator StatusMessageGenerator { get; set; } = new SillyExecutionStatusMessageGenerator();

        public static KernelSpecificTheme? GetTheme(string kernelName) => kernelName switch
        {
            "csharp" => new CSharpTheme(),
            _ => null
        };
    }

    public class CSharpTheme : KernelSpecificTheme
    {
        public override Style AccentStyle => new(Color.Aqua);

        public override string PromptText => "C#";
    }

    internal class DelegatingPrompt : ILineEditorPrompt
    {
        public DelegatingPrompt(string prompt, string? more = null)
        {
            InnerPrompt = new LineEditorPrompt(prompt, more);
        }

        public (Markup Markup, int Margin) GetPrompt(ILineEditorState state, int line)
        {
            return InnerPrompt.GetPrompt(state, line);
        }

        public ILineEditorPrompt InnerPrompt { get; set; }
    }
}