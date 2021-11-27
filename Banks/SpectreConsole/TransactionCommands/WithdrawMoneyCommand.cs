using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Banks.Accounts;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.TransactionCommands.TransactionSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.TransactionCommands
{
    public class WithdrawMoneyCommand : Command<WithdrawMoneySettings>
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;

        public override int Execute(CommandContext context, WithdrawMoneySettings settings)
        {
            ConsoleMethods.AskClientIdAndInitializeClient(out Client accountOwner);
            ConsoleMethods.WriteOptionalActionRule($"{accountOwner}", "started a transaction");

            var clientBanks = CreateCentralBankCommand.CentralBank.Banks
                .Where(b => b.IsClientExists(accountOwner)).ToList();

            Bank bankToWithdraw = ConsoleMethods.BanksPrompt(
                clientBanks, "What [bold green]account[/] you want to [bold]withdraw money[/]?");

            List<IAccount> clientBankAccounts = bankToWithdraw.ClientsAndAccounts
                .Single(clientAndAccounts => clientAndAccounts.Key.Equals(accountOwner))
                .Value;

            IAccount accountToWithdraw = ConsoleMethods.AccountsPrompt(
                clientBankAccounts, "What [bold green]account[/] you want to [bold]withdraw money[/]?");

            if (accountOwner.IsVerified())
            {
                AnsiConsole.Confirm(
                    $"Transaction limit in [aqua]{bankToWithdraw}[/] is [bold green]" +
                    $"{bankToWithdraw.BankTerms.TransactionLimit:C}[/]");
            }
            else
            {
                AnsiConsole.Confirm(
                    $"Transaction limit for [bold]unverified clients[/] in [aqua] {bankToWithdraw} [/] is " +
                    $"[bold green]{bankToWithdraw.BankTerms.TransactionLimitForUnverified:C}[/]");
            }

            AnsiConsole.WriteLine();

            AnsiConsole.Confirm($"Account [bold]{accountToWithdraw} balance[/] is [bold green]" +
                                $"{accountToWithdraw.GetAmountOfMoney():C}[/]");

            AnsiConsole.WriteLine();

            decimal moneyToWithdraw = AnsiConsole.Ask<decimal>(
                $"Enter [bold green]amount of money[/] you want to [bold]withdraw[/] [grey]{CurrencyIcon}[/] :");

            AnsiConsole.WriteLine();

            Transaction<IAccount> transaction
                = bankToWithdraw.AddTransaction(accountToWithdraw, moneyToWithdraw).Withdraw();

            ConsoleMethods.WriteOptionalActionRule("Transaction ID: ", $"{transaction.Id}");

            var transactionReceiptContent = new Markup(
                $"[bold green]From[/]: {accountOwner}\n"
                 + $"[bold green]Bank provider[/]: {bankToWithdraw}\n"
                 + $"[bold green]Sender account name[/]: {accountToWithdraw}\n"
                 + $"[bold green]Money withdrawn[/]: {moneyToWithdraw:C}\n"
                 + $"[bold green]Transaction ID[/]: {transaction.Id}");

            ConsoleMethods.WriteTransactionPanel(transactionReceiptContent);

            ConsoleMethods.WriteCustomRule(
                    $"[bold aqua on grey] {moneyToWithdraw:C} [/] [bold chartreuse1] successfully withdrawn[/] " +
                    $"[bold]from {accountToWithdraw}[/] in [bold aqua]{bankToWithdraw}[/] !",
                    BoxBorder.Rounded);

            return 0;
        }
    }
}