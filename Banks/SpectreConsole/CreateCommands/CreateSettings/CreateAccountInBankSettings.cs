using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands.CreateSettings
{
    public class CreateAccountInBankSettings : CreateCommandsSettings
    {
        [CommandArgument(0, "<BANK_NAME>")]
        public string BankName { get; init; }
        public string AccountName { get; set; }
    }
}