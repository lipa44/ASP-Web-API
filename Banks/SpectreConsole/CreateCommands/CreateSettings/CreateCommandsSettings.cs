using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands.CreateSettings
{
    public class CreateCommandsSettings : CommandSettings
    {
        [CommandArgument(0, "[INSTANCE]")]
        [DefaultValue(true)]
        public bool IsWithoutArgs { get; set; }
    }
}