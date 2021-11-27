using System.Collections.Generic;
using System.Threading;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.ShowCommands.ShowSettings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.ShowCommands
{
    public class ShowAllAccountsCommand : Command<ShowCommandsSettings>
    {
        public override int Execute(CommandContext context, ShowCommandsSettings settings)
        {
            IReadOnlyList<Bank> banks = CreateCentralBankCommand.CentralBank.Banks;

            Tree banksTree = new Tree("[bold yellow]All bank accounts[/]").Guide(TreeGuide.Ascii);

            AnsiConsole.Live(banksTree)
                .Start(ctx =>
                {
                    foreach (Bank bank in banks)
                    {
                        TreeNode userAccounts = banksTree.AddNode($"[bold aqua on grey] {bank} [/]");
                        ctx.Refresh();
                        Thread.Sleep(500);

                        foreach ((Client client, List<IAccount> accounts) in bank.ClientsAndAccounts)
                        {
                            Thread.Sleep(150);
                            TreeNode accountNode = userAccounts
                                .AddNode($"[bold chartreuse1]{client}'s[/] accounts");

                            ctx.Refresh();
                            Thread.Sleep(250);

                            foreach (IAccount account in accounts)
                            {
                                Panel accountPanel =
                                    new Panel($"[italic yellow4]{account.GetAmountOfMoney():C}[/]")
                                        .Header($"[grey]({account.GetType().Name})[/]").Padding(5, 0)
                                        .RoundedBorder();

                                accountNode.AddNode(accountPanel);
                                ctx.Refresh();
                                Thread.Sleep(150);
                            }
                        }
                    }
                });

            AnsiConsole.WriteLine();

            return 0;
        }
    }
}