using System;
using System.Threading.Tasks;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.TransactionCommands.TransactionSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.TransactionCommands
{
    public class CancelTransactionCommand : Command<CancelTransactionSettings>
    {
        public override int Execute(CommandContext context, CancelTransactionSettings settings)
        {
            ConsoleMethods.AskClientIdAndInitializeClient(out Client accountOwner);

            do
            {
                settings.TransactionId = AnsiConsole.Ask<Guid>(
                    "Enter the [bold]ID[/] of transaction you want to [bold red]cancel[/]:");

                AnsiConsole.WriteLine();
            }
            while (!AnsiConsole.Confirm("[bold]All right?[/]"));
            AnsiConsole.WriteLine();

            settings.BankName = AnsiConsole.Ask<string>(
                    "Enter [bold]bank[/], which was provided this transaction:");

            Bank ownerBank = CreateCentralBankCommand.CentralBank.GetBank(settings.BankName);

            ownerBank.CancelTransaction(settings.TransactionId);

            Task.Run(async () =>
            {
                await AnsiConsole.Progress()
                    .StartAsync(async ctx =>
                    {
                        ProgressTask findingTransaction = ctx.AddTask("[green]Finding transaction[/]");
                        ProgressTask ifTransactionFraud = ctx.AddTask("[green]Checking if transaction fraud[/]");
                        ProgressTask cancelingTransaction = ctx.AddTask("[green]Canceling transaction[/]");

                        while (!ctx.IsFinished)
                        {
                            await Task.Delay(100);
                            findingTransaction.Increment(3.5);
                            ifTransactionFraud.Increment(2.5);
                            cancelingTransaction.Increment(1);
                        }
                    });

                ConsoleMethods.WriteGeneralActionRule("Transaction", "was successfully canceled");
            });

            return 0;
        }
    }
}