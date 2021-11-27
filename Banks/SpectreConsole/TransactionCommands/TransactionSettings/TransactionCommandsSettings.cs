using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.TransactionCommands.TransactionSettings
{
    public class TransactionCommandsSettings : CommandSettings
    {
        [CommandArgument(0, "[ACTION]")]
        [DefaultValue(true)]
        public bool IsWithoutArgs { get; init; }
    }
}