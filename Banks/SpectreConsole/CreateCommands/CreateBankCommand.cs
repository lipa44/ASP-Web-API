using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands.CreateSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands
{
    public class CreateBankCommand : Command<CreateBankSettings>
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;

        public override int Execute(CommandContext context, CreateBankSettings settings)
        {
            try
            {
                CreateCentralBankCommand.CentralBank.GetBank(settings.BankName);

                ConsoleMethods.WriteErrorRule($"{settings.BankName} bank", "already exists");
                return 0;
            }
            catch (BankException)
            {
                ConsoleMethods
                    .WriteOptionalActionRule($"{settings.BankName} bank", "general settings");
            }

            do
            {
                settings.DebitPercent = AnsiConsole.Ask<decimal>(
                        $"[bold green]Debit percent[/] in [bold aqua]{settings.BankName}[/] [grey]%[/] ?");

                settings.CreditCommission = AnsiConsole.Ask<decimal>(
                        $"[bold green]Credit commission[/] in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                settings.CreditMaxLimit = AnsiConsole.Ask<decimal>(
                        $"[bold green]Credit max limit[/] in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                settings.CreditMinLimit = AnsiConsole.Ask<decimal>(
                        $"[bold green]Credit min limit[/] in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                settings.TransactionLimit = AnsiConsole.Ask<decimal>(
                        $"[bold green]Transaction limit[/] in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                settings.TransactionLimitForUnverified =
                    AnsiConsole.Ask<decimal>("[bold green]Transaction limit for unverified clients[/] " +
                                             $"in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                Table bankParametersTable = new Table()
                    .Title("[bold white]General settings[/]")
                    .AddColumns(
                        new TableColumn("Debit percent").Centered(),
                        new TableColumn("Credit commission").Centered(),
                        new TableColumn("Credit max limit").Centered(),
                        new TableColumn("Credit min limit").Centered(),
                        new TableColumn("Transaction limit").Centered(),
                        new TableColumn("Transaction limit for unverified").Centered())
                    .AddRow(
                        $"[bold green]{settings.DebitPercent:C}[/]",
                        $"[bold green]{settings.CreditCommission:C}[/]",
                        $"[bold green]{settings.CreditMaxLimit:C}[/]",
                        $"[bold green]{settings.CreditMinLimit:C}[/]",
                        $"[bold green]{settings.TransactionLimit:C}[/]",
                        $"[bold green]{settings.TransactionLimitForUnverified:C}[/]")
                    .Centered();

                AnsiConsole.Write(bankParametersTable);
                AnsiConsole.WriteLine();
            }
            while (!AnsiConsole.Confirm("[bold]All right?[/]"));

            AnsiConsole.WriteLine();

            settings.DepositsAndPercents = new List<(Range, decimal)>();

            ConsoleMethods
                .WriteOptionalActionRule($"{settings.BankName} bank", "deposit percents settings");

            do
            {
                var depositRanges = new List<Range>();
                var percents = new List<decimal>();

                do
                {
                    AnsiConsole.WriteLine();

                    int depositRangeStart = AnsiConsole.Ask<int>(
                        $"Percent [bold green]range start[/] in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");
                    int depositRangeEnd = AnsiConsole.Ask<int>(
                        $"Percent [bold green]range end[/] in [bold aqua]{settings.BankName}[/] [grey]{CurrencyIcon}[/] ?");

                    var depositRange = new Range(depositRangeStart, depositRangeEnd);

                    decimal rangePercent =
                        AnsiConsole.Ask<decimal>("Percent [bold green]value in range[/] [grey] % [/]?");

                    depositRanges.Add(depositRange);
                    percents.Add(rangePercent);
                    settings.DepositsAndPercents.Add((depositRange, rangePercent));

                    AnsiConsole.WriteLine();
                }
                while (AnsiConsole.Confirm("[bold]One more range?[/]"));

                AnsiConsole.WriteLine();

                Table depositRangesAndPercentsTable = new Table()
                    .Title("[bold white]Deposit percents[/]").Centered()
                    .AddColumns(depositRanges
                        .Select(range => $"{range.Start.Value} - {range.End.Value} {CurrencyIcon}")
                        .ToArray())
                    .AddRow(percents
                        .Select(percent => $"{percent.ToString(CultureInfo.InvariantCulture)} %")
                        .ToArray());

                AnsiConsole.Write(depositRangesAndPercentsTable);
            }
            while (!AnsiConsole.Confirm("[bold]All right?[/]"));

            AnsiConsole.WriteLine();

            var bankTermsBuilder = new BankTerms(
                settings.DebitPercent,
                settings.CreditCommission,
                settings.CreditMaxLimit,
                settings.CreditMinLimit,
                settings.TransactionLimit,
                settings.TransactionLimitForUnverified,
                settings.DepositsAndPercents);

            CreateCentralBankCommand.CentralBank.RegisterBank(settings.BankName, bankTermsBuilder);

            ConsoleMethods.WriteGeneralActionRule($"{settings.BankName} bank", "successfully created");

            return 0;
        }
    }
}