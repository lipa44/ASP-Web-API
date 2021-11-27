using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.ShowCommands.ShowSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.ShowCommands
{
    public class ShowUserAccountsCommand : Command<ShowUserAccountsSettings>
    {
        public override int Execute(CommandContext context, ShowUserAccountsSettings settings)
        {
            Client client;
            if (settings.ClientId == Guid.Empty)
                ConsoleMethods.AskClientIdAndInitializeClient(out client);
            else
                client = CreateCentralBankCommand.CentralBank.GetClient(settings.ClientId);

            var clientBanks = CreateCentralBankCommand.CentralBank.Banks
                .Where(b => b.ClientsAndAccounts.ContainsKey(client))
                .ToList();

            var clientBankAndAccounts = clientBanks
                .ToDictionary(b => b.Name, b => b.ClientsAndAccounts
                    .Where(kvp => kvp.Key.PassportId == client.PassportId)
                    .SelectMany(kvp => kvp.Value)
                    .ToList());

            Tree userAccounts = new Tree($"[bold green]{client}'s[/] accounts").Guide(TreeGuide.BoldLine);

            AnsiConsole.Live(userAccounts)
                .Start(ctx =>
                {
                    foreach ((string bankName, List<IAccount> accounts) in clientBankAndAccounts)
                    {
                        TreeNode accountNode = userAccounts.AddNode($"[bold aqua]{bankName}[/]");
                        ctx.Refresh();
                        Thread.Sleep(500);

                        foreach (IAccount account in accounts)
                        {
                            TreeNode node = accountNode.AddNode($"{account.GetType().Name}");
                            ctx.Refresh();
                            Thread.Sleep(500);

                            node.AddNode($"[bold]{account.GetAmountOfMoney():C}[/]");
                            ctx.Refresh();
                            Thread.Sleep(250);
                        }
                    }
                });

            AnsiConsole.WriteLine();

            return 0;
        }
    }
}