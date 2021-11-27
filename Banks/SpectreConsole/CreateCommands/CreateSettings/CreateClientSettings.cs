using System;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands.CreateSettings
{
    public class CreateClientSettings : CreateCommandsSettings
    {
        [CommandArgument(0, "<NAME>")]
        public string ClientName { get; init; }

        [CommandArgument(1, "<SURNAME>")]
        public string ClientSurname { get; init; }
        public Guid PassportId { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}