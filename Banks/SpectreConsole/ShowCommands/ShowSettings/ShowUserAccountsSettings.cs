using System;
using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.ShowCommands.ShowSettings
{
    public class ShowUserAccountsSettings : ShowCommandsSettings
    {
        [CommandArgument(0, "[CLIENT_ID]")]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid ClientId { get; init; }
    }
}