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
    public class RemoveSubscriptionCommand : Command<RemoveSubscriptionSettings>
    {
        public override int Execute(CommandContext context, RemoveSubscriptionSettings settings)
        {
            Client client;
            if (settings.ClientId == Guid.Empty)
                ConsoleMethods.AskClientIdAndInitializeClient(out client);
            else
                client = CreateCentralBankCommand.CentralBank.GetClient(settings.ClientId);

            ConsoleMethods.WriteCustomRule(
                "[bold green]Client[/] [bold aqua]remove subscription[/] [bold green]option[/]",
                BoxBorder.Ascii);

            List<AccountType> accountTypesToRemoveSubscription = ConsoleMethods.AccountTypeMultiPrompt(
                "What [green]type of account[/] do tou want to add to subscriptions?");
            AnsiConsole.WriteLine();

            IReadOnlyList<Bank> banks = CreateCentralBankCommand.CentralBank.Banks;
            foreach (AccountType accountType in accountTypesToRemoveSubscription)
            {
                foreach (Bank bank in banks)
                    bank.BankTerms.RemoveOnUpdateSubscriber(client, accountType);

                ConsoleMethods.WriteCustomRule(
                    $"[bold aqua on grey]{accountType} subscription[/] [bold]for {client}[/] " +
                    "[bold chartreuse1]successfully removed[/]!",
                    BoxBorder.Rounded);
            }

            return 0;
        }
    }
}