using Banks.Entities;
using Banks.SpectreConsole.CreateCommands.CreateSettings;
using Banks.Tools;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands
{
    public class CreateCentralBankCommand : Command<CreateCommandsSettings>
    {
        public static CentralBank CentralBank { get; set; }

        public override int Execute(CommandContext context, CreateCommandsSettings settings)
        {
            try
            {
                var centralBank = CentralBank.GetInstance();
                CentralBank = centralBank;

                ConsoleMethods.WriteGeneralActionRule("Central bank", "successfully created");
            }
            catch (BankException)
            {
                ConsoleMethods.WriteErrorRule("Central bank", "already exists");
            }

            return 0;
        }
    }
}