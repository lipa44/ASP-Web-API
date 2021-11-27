using System;
using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.SubscriptionCommands.SubscriptionCommandsSettings
{
    public class RemoveSubscriptionSettings : SubscriptionCommandSettings
    {
        [CommandArgument(0, "[CLIENT_ID]")]
        [Description("Remove account type updetes.")]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid ClientId { get; init; }
    }
}