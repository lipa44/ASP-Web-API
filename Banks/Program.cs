using System;
using Banks.SpectreConsole;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.CreateCommands.CreateSettings;
using Banks.SpectreConsole.ShowCommands;
using Banks.SpectreConsole.ShowCommands.ShowSettings;
using Banks.SpectreConsole.SubscriptionCommands;
using Banks.SpectreConsole.SubscriptionCommands.SubscriptionCommandsSettings;
using Banks.SpectreConsole.TransactionCommands;
using Banks.SpectreConsole.TransactionCommands.TransactionSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();

            app.Configure(config =>
            {
                config.SetApplicationName(string.Empty);
                config.AddExample(new[] { "create", "cb" });
                config.AddExample(new[] { "create", "client", "Misha", "Libchenko" });
                config.AddExample(new[] { "create", "bank", "Tinkoff" });
                config.AddExample(new[] { "create", "account", "Tinkoff" });
                config.AddExample(new[] { "transaction", "charge" });
                config.AddExample(new[] { "transaction", "withdraw" });
                config.AddExample(new[] { "transaction", "transfer" });
                config.AddExample(new[] { "transaction", "cancel" });
                config.AddExample(new[] { "subscription", "add", "11111111-1111-1111-1111-111111111111" });
                config.AddExample(new[] { "subscription", "remove", "11111111-1111-1111-1111-111111111111" });
                config.AddExample(new[] { "subscription", "updates", "11111111-1111-1111-1111-111111111111" });
                config.AddExample(new[] { "show", "banks" });
                config.AddExample(new[] { "show", "accounts" });
                config.AddExample(new[] { "show", "clients" });
                config.AddExample(new[] { "show", "client-account", "11111111-1111-1111-1111-111111111111" });
                config.AddExample(new[] { "reset", "Tinkoff" });
                config.AddExample(new[] { "rewind", "-m", "1" });
                config.ValidateExamples();

                config.AddBranch<CreateCommandsSettings>("create", acc =>
                {
                    acc.SetDescription("All actions with creating entities");
                    acc.AddCommand<CreateCentralBankCommand>("cb");
                    acc.AddCommand<CreateClientCommand>("client");
                    acc.AddCommand<CreateBankCommand>("bank");
                    acc.AddCommand<CreateAccountInBankCommand>("account");
                });

                config.AddBranch<TransactionCommandsSettings>("transaction", acc =>
                {
                    acc.SetDescription("All actions with transactions");
                    acc.AddCommand<ChargeMoneyCommand>("charge");
                    acc.AddCommand<WithdrawMoneyCommand>("withdraw");
                    acc.AddCommand<TransferMoneyCommand>("transfer");
                    acc.AddCommand<CancelTransactionCommand>("cancel");
                });

                config.AddBranch<SubscriptionCommandSettings>("subscription", acc =>
                {
                    acc.SetDescription("All actions with subscriptions");
                    acc.AddCommand<AddSubscriptionCommand>("add");
                    acc.AddCommand<RemoveSubscriptionCommand>("remove");
                    acc.AddCommand<GetSubscriptionUpdatesCommand>("updates");
                });

                config.AddBranch<ShowCommandsSettings>("show", acc =>
                {
                    acc.SetDescription("Shows data in banks ecosystem (table/tree)");
                    acc.AddCommand<ShowBanksTableCommand>("banks");
                    acc.AddCommand<ShowAllAccountsCommand>("accounts");
                    acc.AddCommand<ShowUserAccountsCommand>("client-account");
                    acc.AddCommand<ShowAllClientsTableCommand>("clients");
                });

                config.AddCommand<ChangeBankTermsCommand>("reset")
                    .WithDescription("Change bank's parameters.");

                config.AddCommand<TimeRewindCommand>("rewind")
                    .WithDescription("Rewinds time for N months.");

                config.AddCommand<ClientVerificationCommand>("verification")
                    .WithDescription("Add optional parameters to remove transaction limit.");
            });

            InputHandler.BanksFiglet.Invoke();
            app.Run(args);

            while (true)
            {
                string command = Console.ReadLine();
                AnsiConsole.WriteLine();

                if (InputHandler.ClearCommandAndContinue.Invoke(command)) continue;
                if (InputHandler.QuitProgramCommandAndContinue.Invoke(command)) return 0;

                app.Run(command.Split(' '));
            }
        }
    }
}