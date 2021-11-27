using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.ShowCommands.ShowSettings
{
    public class ShowCommandsSettings : CommandSettings
    {
        [DefaultValue(null)]
        public bool IsWithoutArgs { get; init; }
    }
}