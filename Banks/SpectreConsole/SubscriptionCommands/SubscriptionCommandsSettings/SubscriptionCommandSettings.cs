using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.SubscriptionCommands.SubscriptionCommandsSettings
{
    public class SubscriptionCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[ACTION]")]
        [DefaultValue(true)]
        public bool IsWithoutArgs { get; set; }
    }
}