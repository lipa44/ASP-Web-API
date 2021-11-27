using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands;
using Spectre.Console;

namespace Banks.Tools
{
    public class ConsoleMethods
    {
        public static void WriteGeneralActionRule(string generalText, string additionalText)
        {
            Rule actionRule
                = new Rule($"[italic bold aqua on grey] {generalText} [/][bold chartreuse1] {additionalText} [/]!")
                    .Centered();

            AnsiConsole.Write(actionRule);
            AnsiConsole.WriteLine();
        }

        public static void WriteOptionalActionRule(string generalText, string additionalText)
        {
            Rule actionRule
                = new Rule($"[bold green]{generalText}[/][italic bold aqua on grey] {additionalText} [/]!")
                    .AsciiBorder()
                    .Centered();

            AnsiConsole.Write(actionRule);
            AnsiConsole.WriteLine();
        }

        public static void WriteCustomRule(string ruleText, BoxBorder borderStyle)
        {
            Rule customRule = new Rule($"{ruleText}")
                .Border(borderStyle)
                .Centered();

            AnsiConsole.Write(customRule);
            AnsiConsole.WriteLine();
        }

        public static void WriteErrorRule(string generalText, string errorText)
        {
            Rule clientExistRule
                = new Rule($"[bold aqua] {generalText} [/][italic bold red on grey] {errorText} [/]")
                    .Centered();

            AnsiConsole.Write(clientExistRule);
            AnsiConsole.WriteLine();
        }

        public static void WriteTransactionPanel(Markup transactionData)
        {
            Panel transactionReceiptPanel = new Panel(transactionData)
                .Header("[italic bold aqua on grey] Transaction receipt [/]")
                .HeaderAlignment(Justify.Center);

            AnsiConsole.Write(transactionReceiptPanel);
            AnsiConsole.WriteLine();
        }

        public static AccountType AccountTypePrompt(string title)
        {
            var accountTypes = (AccountType[])Enum.GetValues(typeof(AccountType));

            return AnsiConsole.Prompt(
                new SelectionPrompt<AccountType>()
                    .PageSize(accountTypes.Length)
                    .Title($"{title}")
                    .AddChoices(accountTypes));
        }

        public static List<AccountType> AccountTypeMultiPrompt(string title)
        {
            var accountTypes = (AccountType[])Enum.GetValues(typeof(AccountType));

            return AnsiConsole.Prompt(
                new MultiSelectionPrompt<AccountType>()
                    .PageSize(accountTypes.Length)
                    .Title($"{title}")
                    .AddChoices(accountTypes));
        }

        public static void AskClientIdAndInitializeClient(out Client client)
        {
            do
            {
                Guid clientId = AnsiConsole.Ask<Guid>("What is your [bold]passport ID[/]?");
                client = CreateCentralBankCommand.CentralBank.GetClient(clientId);
                AnsiConsole.WriteLine();
            }
            while (!AnsiConsole.Confirm($"[bold green]{client}[/], right?"));

            AnsiConsole.WriteLine();
        }

        public static IAccount AccountsPrompt(IEnumerable<IAccount> accounts, string title) => AnsiConsole.Prompt(
            new SelectionPrompt<Account>()
                .Title($"{title}")
                .AddChoices(accounts.Select(acc => acc as Account)));

        public static Bank BanksPrompt(IEnumerable<Bank> banks, string title) => AnsiConsole.Prompt(
            new SelectionPrompt<Bank>()
                .Title($"{title}")
                .AddChoices(banks));
    }
}