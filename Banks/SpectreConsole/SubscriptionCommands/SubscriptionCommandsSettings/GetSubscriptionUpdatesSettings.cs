using System;
using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.SubscriptionCommands.SubscriptionCommandsSettings
{
    public class GetSubscriptionUpdatesSettings : SubscriptionCommandSettings
    {
        [CommandArgument(0, "[CLIENT_ID]")]
        [Description("Get subscription updetes.")]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid ClientId { get; init; }
    }
}