using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.ShowCommands.ShowSettings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.ShowCommands
{
    public class ShowBanksTableCommand : Command<ShowCommandsSettings>
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;

        public override int Execute(CommandContext context, ShowCommandsSettings settings)
        {
            Table banksParametersTable = new Table().Title("Banks terms")
                .Centered().RoundedBorder().Expand();

            var tableColumns = new List<string>
            {
                "Bank", "Credit commission", "Credit max limit", "Credit min limit",
                "Transaction limit", "Transaction limit for unverified", "Debit percent",
            };

            Task.Run(async () =>
            {
                await AnsiConsole.Live(banksParametersTable)
                    .StartAsync(async ctx =>
                    {
                        foreach (string bankParameter in tableColumns)
                        {
                            banksParametersTable.AddColumn(new TableColumn($"[bold]{bankParameter}[/]").Centered());
                            ctx.Refresh();
                            await Task.Delay(250);
                        }

                        foreach (Bank bank in CreateCentralBankCommand.CentralBank.Banks)
                        {
                            banksParametersTable.AddRow(
                                new Markup($"[bold aqua] {bank} [/]"),
                                new Markup($"{bank.BankTerms.CreditCommission:C}"),
                                new Markup($"{bank.BankTerms.CreditMaxLimit:C}"),
                                new Markup($"{bank.BankTerms.CreditMinLimit:C}"),
                                new Markup($"{bank.BankTerms.TransactionLimit:C}"),
                                new Markup($"{bank.BankTerms.TransactionLimitForUnverified:C}"),
                                new Markup($"{bank.BankTerms.DebitPercent}[grey] %[/]"));

                            ctx.Refresh();
                            await Task.Delay(500);
                        }
                    });

                AnsiConsole.WriteLine();
                Table depositsAndPercentsTable = new Table().Title("Banks deposits and percents").Expand()
                    .Centered().RoundedBorder();

                await AnsiConsole.Live(depositsAndPercentsTable)
                    .StartAsync(async ctx =>
                    {
                        foreach (Bank bank in CreateCentralBankCommand.CentralBank.Banks)
                        {
                            depositsAndPercentsTable.AddColumn(new TableColumn($"[bold aqua]{bank}[/]")
                                .Centered());
                            ctx.Refresh();
                            await Task.Delay(500);
                        }

                        var panelsList = new List<Panel>();
                        foreach (Bank bank in CreateCentralBankCommand.CentralBank.Banks)
                        {
                            string depositsAndPercentsText = string.Empty;
                            foreach ((Range range, decimal percent) in bank.BankTerms.FirstDepositAndPercents)
                                depositsAndPercentsText += $"\n{range.Start.Value} - {range.End.Value} " +
                                                           $"{CurrencyIcon}: {percent} [grey]%[/]";

                            panelsList.Add(new Panel(depositsAndPercentsText)
                                .HeaderAlignment(Justify.Center).Expand());
                        }

                        depositsAndPercentsTable.AddRow(panelsList.Select(p => p));
                    });
                AnsiConsole.WriteLine();
            });

            return 0;
        }
    }
}