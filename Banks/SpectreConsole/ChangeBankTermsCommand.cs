using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole
{
    public class ChangeBankTermsCommand : Command<ChangeBankTermsCommand.Settings>
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;

        public override int Execute(CommandContext context, Settings settings)
        {
            Bank bank = CreateCentralBankCommand.CentralBank.GetBank(settings.BankName);
            BankTerms bankBankTerms = bank.BankTerms;
            var bankParametersNames = new List<string>
            {
                "Credit commission", "Debit percent", "Credit max limit", "Credit min limit",
                "Transaction limit", "Transaction limit for unverified clients", "First deposit and percents",
            };

            List<string> parametersToChangeNames = BankParametersToChangeNamePrompt(bankParametersNames);

            foreach (string parameterName in parametersToChangeNames)
            {
                switch (parameterName)
                {
                    case "Credit commission":
                        decimal newCreditCommission
                            = AnsiConsole.Ask<decimal>($"New [bold green]Credit commission[/] [grey]{CurrencyIcon}[/]?");
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetCreditCommission(newCreditCommission);
                        break;

                    case "Debit percent":
                        decimal newDebitPercent
                            = AnsiConsole.Ask<decimal>($"New [bold green]Debit percent[/] [grey]{CurrencyIcon}[/]?");
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetDebitPercent(newDebitPercent);
                        break;

                    case "Credit max limit":
                        decimal newCreditMaxLimit
                            = AnsiConsole.Ask<decimal>($"New [bold green]Credit max limit[/] [grey]{CurrencyIcon}[/]?");
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetCreditMaxLimit(newCreditMaxLimit);
                        break;

                    case "Credit min limit":
                        decimal newCreditMinLimit
                            = AnsiConsole.Ask<decimal>($"New [bold green]Credit min limit[/] [grey]{CurrencyIcon}[/]?");
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetCreditMinLimit(newCreditMinLimit);
                        break;

                    case "Transaction limit":
                        decimal newTransactionLimit
                            = AnsiConsole.Ask<decimal>($"New [bold green]Transaction limit[/] [grey]{CurrencyIcon}[/]?");
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetTransactionLimit(newTransactionLimit);
                        break;

                    case "Transaction limit for unverified clients":
                        decimal newTransactionLimitForUnverified
                            = AnsiConsole.Ask<decimal>("New [bold green]Transaction limit for unverified clients[/]?");
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetTransactionLimitForUnverified(newTransactionLimitForUnverified);
                        break;

                    case "First deposit and percents":
                        var newFirstDepositsAndPercents = new List<(Range, decimal)>();
                        do
                        {
                            var newDepositRanges = new List<Range>();
                            var newPercents = new List<decimal>();

                            do
                            {
                                AnsiConsole.WriteLine();

                                int depositRangeStart = AnsiConsole.Ask<int>(
                                    "Percent [bold green]percent range start[/] in [bold aqua]" +
                                    $"{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                                int depositRangeEnd = AnsiConsole.Ask<int>(
                                    "Percent [bold green]percent range end[/] in [bold aqua]" +
                                    $"{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                                var depositRange = new Range(depositRangeStart, depositRangeEnd);

                                decimal rangePercent = AnsiConsole.Ask<decimal>(
                                    "Percent [bold green]value in range[/] [grey]%[/] ?");

                                newDepositRanges.Add(depositRange);
                                newPercents.Add(rangePercent);
                                newFirstDepositsAndPercents.Add((depositRange, rangePercent));

                                AnsiConsole.WriteLine();
                            }
                            while (AnsiConsole.Confirm("[bold]One more range?[/]"));
                            AnsiConsole.WriteLine();

                            Table depositRangesAndPercentsTable = new Table()
                                .Title("[bold white]Deposit percents[/]").Centered()
                                .AddColumns(newDepositRanges
                                    .Select(range => $"{range.Start.Value} - {range.End.Value} {CurrencyIcon}")
                                    .ToArray())
                                .AddRow(newPercents.Select(percent => percent
                                    .ToString(CultureInfo.InvariantCulture))
                                    .ToArray());

                            AnsiConsole.Write(depositRangesAndPercentsTable);
                        }
                        while (!AnsiConsole.Confirm("[bold]All right?[/]"));
                        AnsiConsole.WriteLine();

                        bankBankTerms.SetFirstDepositAndPercents(newFirstDepositsAndPercents);
                        break;
                }
            }

            bank.UpdateBankTermsInAccounts();

            ConsoleMethods.WriteOptionalActionRule(
                $"Bank {settings.BankName} parameters", "successfully changed");

            return 0;
        }

        private List<string> BankParametersToChangeNamePrompt(IEnumerable<string> bankParameters) => AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("What [bold green]bank parameters[/] you want to [bold]change[/]?")
                .AddChoices(bankParameters));

        public sealed class Settings : CommandSettings
        {
            [Description("Bank to change paremeters name.")]
            [CommandArgument(0, "<BANK_NAME>")]
            public string BankName { get; init; }
        }
    }
}