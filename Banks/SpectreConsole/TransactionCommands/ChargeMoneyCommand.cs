using System.Collections.Generic;
using System.Linq;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.TransactionCommands.TransactionSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.TransactionCommands
{
    public class ChargeMoneyCommand : Command<AddMoneySettings>
    {
        public override int Execute(CommandContext context, AddMoneySettings settings)
        {
            ConsoleMethods.AskClientIdAndInitializeClient(out Client accountOwner);

            decimal amountOfMoneyToAdd = AnsiConsole.Ask<decimal>(
                    "Enter [bold green]amount of money[/] you want to [bold green]add[/]:");

            AnsiConsole.WriteLine();

            var clientBanks = CreateCentralBankCommand.CentralBank.Banks
                .Where(b => b.IsClientExists(accountOwner)).ToList();

            Bank bankToAddMoney =
                ConsoleMethods.BanksPrompt(clientBanks, "What [bold green]account[/] you want to [bold]add money[/]?");

            List<IAccount> clientBankAccounts = bankToAddMoney.ClientsAndAccounts
                .Single(clientAndAccounts => clientAndAccounts.Key.Equals(accountOwner))
                .Value;

            IAccount accountToAddMoney = ConsoleMethods.AccountsPrompt(
                clientBankAccounts, "What [bold green]account[/] you want to [bold]add money[/]?");

            accountToAddMoney.ChargeMoney(amountOfMoneyToAdd);

            ConsoleMethods.WriteCustomRule(
                $"[bold aqua on grey] {amountOfMoneyToAdd:C} [/] [bold chartreuse1] successfully added[/] [bold]to " +
                $"{accountToAddMoney.GetAccountName()}[/] in [bold aqua]{bankToAddMoney.Name}[/] !",
                BoxBorder.Rounded);

            return 0;
        }
    }
}