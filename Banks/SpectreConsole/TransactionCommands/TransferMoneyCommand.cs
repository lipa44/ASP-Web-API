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
    public class TransferMoneyCommand : Command<TransferMoneySettings>
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;

        public override int Execute(CommandContext context, TransferMoneySettings settings)
        {
            ConsoleMethods.AskClientIdAndInitializeClient(out Client sender);

            ConsoleMethods.WriteOptionalActionRule($"{sender}", "started a transaction");

            var senderBanks = CreateCentralBankCommand.CentralBank.Banks
                .Where(b => b.IsClientExists(sender)).ToList();

            Bank senderBank = ConsoleMethods.BanksPrompt(
                senderBanks, "Choose [green]a bank[/] you want to [bold]transfer money from[/]?");

            List<IAccount> senderAccountsInBank = senderBank.ClientsAndAccounts
                .Single(acc => acc.Key.Equals(sender))
                .Value;

            IAccount senderAccount = ConsoleMethods.AccountsPrompt(
                senderAccountsInBank, "Choose [green]an account[/] you want to [bold]transfer money from[/]?");

            AnsiConsole.Confirm(
                $"Account {senderAccount} balance: [bold green]{senderAccount.GetAmountOfMoney():C}[/], ok?");

            AnsiConsole.WriteLine();

            Client recipient;
            do
            {
                Guid recipientId = AnsiConsole.Ask<Guid>("What is the [bold]recipient ID[/]?");
                recipient = CreateCentralBankCommand.CentralBank.GetClient(recipientId);
                AnsiConsole.WriteLine();
            }
            while (!AnsiConsole.Confirm($"Recipient: [bold green]{recipient}[/]?"));
            AnsiConsole.WriteLine();

            var recipientBanks = CreateCentralBankCommand.CentralBank.Banks
                .Where(b => b.IsClientExists(recipient)).ToList();

            Bank recipientBank =
                ConsoleMethods.BanksPrompt(recipientBanks, "Choose [green]a bank[/] of [bold]money recipient[/]?");

            List<IAccount> recipientAccountsInBank = recipientBank.ClientsAndAccounts
                .Single(acc => acc.Key.Equals(recipient))
                .Value;

            IAccount recipientAccount = ConsoleMethods.AccountsPrompt(
                recipientAccountsInBank, "Choose [green]an account[/] of [bold]money recipient[/]?");

            if (sender.IsVerified())
            {
                AnsiConsole.Confirm($"Transaction limit in [aqua]{senderBank}[/] is " +
                                    $"[bold green]{senderBank.BankTerms.TransactionLimit:C}");
            }
            else
            {
                AnsiConsole.Confirm("Transaction limit for [bold]unverified clients[/] in [aqua] " +
                                    $"{senderBank}[/] is [bold green]{senderBank.BankTerms.TransactionLimitForUnverified:C}[/]");
            }

            AnsiConsole.WriteLine();

            decimal moneyToTransfer;
            do
            {
                moneyToTransfer = AnsiConsole.Ask<decimal>(
                    $"How much money do you [bold]want to transfer[/] [grey]{CurrencyIcon}[/] ?");

                AnsiConsole.WriteLine();
            }
            while (!AnsiConsole.Confirm($"[bold green]{moneyToTransfer:C}[/] to transfer"));
            AnsiConsole.WriteLine();

            Transaction<IAccount> transaction
                = senderBank.AddTransaction(senderAccount, moneyToTransfer).Transfer(recipientAccount);

            ConsoleMethods.WriteOptionalActionRule("Transaction ID: ", $"{transaction.Id}");

            var transactionReceiptContent = new Markup(
                $"[bold green]From[/]: {sender}\n"
                 + $"[bold green]Bank provider[/]: {senderBank}\n"
                 + $"[bold green]Sender account name[/]: {senderAccount}\n"
                 + $"[bold green]Recipient[/]: {recipient}\n"
                 + $"[bold green]Recipient bank[/]: {recipientBank}\n"
                 + $"[bold green]Money transferred[/]: {moneyToTransfer:C}\n"
                 + $"[bold green]Transaction ID[/]: {transaction.Id}");

            ConsoleMethods.WriteTransactionPanel(transactionReceiptContent);

            ConsoleMethods.WriteCustomRule(
                $"[bold aqua on grey] {moneyToTransfer:C} to {recipient} [/] [bold chartreuse1]" +
                "successfully transferred[/]!",
                BoxBorder.Rounded);

            return 0;
        }
    }
}