using System;
using System.Collections.Generic;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.SubscriptionCommands.SubscriptionCommandsSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.SubscriptionCommands
{
    public class AddSubscriptionCommand : Command<AddSubscriptionSettings>
    {
        public override int Execute(CommandContext context, AddSubscriptionSettings settings)
        {
            Client client;
            if (settings.ClientId == Guid.Empty)
                ConsoleMethods.AskClientIdAndInitializeClient(out client);
            else
                client = CreateCentralBankCommand.CentralBank.GetClient(settings.ClientId);

            ConsoleMethods.WriteOptionalActionRule("Client add subscription", "option");

            List<AccountType> accountTypesToSubscribe = ConsoleMethods
                .AccountTypeMultiPrompt("What [green]type of account[/] do tou want to add to subscriptions?");
            AnsiConsole.WriteLine();

            IReadOnlyList<Bank> banks = CreateCentralBankCommand.CentralBank.Banks;
            foreach (AccountType accountType in accountTypesToSubscribe)
            {
                foreach (Bank bank in banks)
                    bank.BankTerms.AddOnUpdateSubscriber(client, accountType);

                ConsoleMethods.WriteCustomRule(
                    $"[bold aqua on grey]{accountType} subscription[/] [bold]for {client}[/] " +
                    "[bold chartreuse1]successfully added[/]!",
                    BoxBorder.Rounded);
            }

            return 0;
        }
    }
}