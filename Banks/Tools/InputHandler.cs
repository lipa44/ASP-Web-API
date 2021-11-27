using System;
using System.IO;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace Banks.Tools
{
    public class InputHandler
    {
        public static readonly InputHandlerDelegate BanksFiglet = () =>
        {
            var font = FigletFont.Load(Pwd + $"{Sep}Tools{Sep}Fonts{Sep}Colossal.flf");
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText(font, "Banks")
                .Centered()
                .Color(Color.Red));
            AnsiConsole.WriteLine();
        };

        public static readonly Func<string, bool> ClearCommandAndContinue = command =>
        {
            if (!new Regex("clear", RegexOptions.IgnoreCase).IsMatch(command ?? string.Empty)) return false;
            BanksFiglet.Invoke();
            return true;
        };

        public static readonly Func<string, bool> QuitProgramCommandAndContinue = command =>
        {
            if (!new Regex("q|quit|выйти", RegexOptions.IgnoreCase).IsMatch(command ?? string.Empty)) return false;
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText(Font, "VseGO hoROsheGO!")
                .Centered()
                .Color(Color.Chartreuse1));
            AnsiConsole.WriteLine();
            return true;
        };

        private static readonly string Pwd = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        private static readonly char Sep = Path.DirectorySeparatorChar;
        private static readonly FigletFont Font = FigletFont.Load(Pwd + $"{Sep}Tools{Sep}Fonts{Sep}Colossal.flf");

        public delegate void InputHandlerDelegate();
    }
}