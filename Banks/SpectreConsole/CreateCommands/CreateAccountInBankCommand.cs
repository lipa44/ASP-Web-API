using System;
using System.Globalization;
using Banks.Accounts.Creators;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands.CreateSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands
{
    public class CreateAccountInBankCommand : Command<CreateAccountInBankSettings>
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;

        public override int Execute(CommandContext context, CreateAccountInBankSettings settings)
        {
            Bank bank = CreateCentralBankCommand.CentralBank.GetBank(settings.BankName);

            ConsoleMethods.AskClientIdAndInitializeClient(out Client client);

            AccountType accountType
                = ConsoleMethods.AccountTypePrompt("What [green]type of account[/] do tou want to create?");

            ConsoleMethods.WriteOptionalActionRule($"{accountType} in {bank}", "creating");

            settings.AccountName = AnsiConsole.Ask<string>("What will be the [bold green]name of account[/]?");
            AnsiConsole.WriteLine();

            switch (accountType)
            {
                case AccountType.CreditAccount:
                    if (!IsClientAgreeWithCreditTerms(bank)) break;
                    AnsiConsole.WriteLine();

                    IAccount creditAccount = new CreditAccountCreator(client, bank, settings.AccountName).CreateAccount();
                    bank.RegisterAccount(client, creditAccount);
                    ConsoleMethods.WriteGeneralActionRule($"{accountType} in {bank}", "successfully created");

                    break;

                case AccountType.DebitAccount:
                    if (!IsClientAgreeWithDebitTerms(bank)) break;
                    AnsiConsole.WriteLine();

                    IAccount debitAccount = new DebitAccountCreator(client, bank, settings.AccountName).CreateAccount();
                    bank.RegisterAccount(client, debitAccount);
                    ConsoleMethods.WriteGeneralActionRule($"{accountType} in {bank}", "successfully created");

                    break;

                case AccountType.DepositAccount:
                    int firstDeposit, monthsAmount;
                    do
                    {
                        firstDeposit = AnsiConsole.Ask<int>(
                            $"What will be your [bold green]first deposit[/]? [grey]{CurrencyIcon}[/]");

                        monthsAmount = AnsiConsole.Ask<int>(
                            "What will be the [bold green]duration of your deposit[/]? [grey](in months)[/]");

                        AnsiConsole.WriteLine();

                        ConsoleMethods.WriteCustomRule(
                                $"Your [bold]deposit percent[/] [green] {bank.BankTerms.CalculatePercentByDeposit(firstDeposit)}[/] %",
                                BoxBorder.Ascii);

                        Table depositDataTable = new Table().AddColumns("First deposit", "Unfreeze date")
                            .AddRow(
                                $"[bold green]{firstDeposit:C}[/]",
                                $"[bold green]{DateTime.Today.AddMonths(monthsAmount):D}[/]")
                            .RoundedBorder()
                            .Centered();

                        AnsiConsole.Write(depositDataTable);
                        AnsiConsole.WriteLine();
                    }
                    while (!AnsiConsole.Confirm("[bold]All right?[/]"));
                    AnsiConsole.WriteLine();

                    IAccount depositAccount
                        = new DepositAccountCreator(
                                client,
                                bank,
                                firstDeposit,
                                DateTime.Today.AddMonths(monthsAmount),
                                settings.AccountName)
                        .CreateAccount();

                    bank.RegisterAccount(client, depositAccount);
                    ConsoleMethods.WriteGeneralActionRule($"{accountType} in {bank}", "successfully created");

                    break;

                default:
                    throw new BankException("Account type didn't recognized");
            }

            return 0;
        }

        private bool IsClientAgreeWithCreditTerms(Bank bank) => AnsiConsole.Confirm(
            $"[bold]Credit max limit[/] will be [bold green]{bank.BankTerms.CreditMaxLimit:C}[/] [grey]{CurrencyIcon}[/]\n"
            + $"[bold]Credit min limit[/] will be [bold green]{bank.BankTerms.CreditMinLimit:C}[/] [grey]{CurrencyIcon}[/]\n"
            + $"[bold]Credit commission[/] is [bold green]{bank.BankTerms.CreditCommission:C}[/] [grey]{CurrencyIcon}[/]\n\n"
            + "Are you agree with terms?");

        private bool IsClientAgreeWithDebitTerms(Bank bank) => AnsiConsole.Confirm(
            $"[bold]Debit percent[/] will be [bold green]{bank.BankTerms.DebitPercent}[/] [grey]%[/]\n\n"
            + "Are you agree with terms?");
    }
}