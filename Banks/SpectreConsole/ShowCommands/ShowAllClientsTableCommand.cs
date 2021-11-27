using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banks.Accounts;
using Banks.Entities;
using Banks.Interfaces;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.ShowCommands.ShowSettings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.ShowCommands
{
    public class ShowAllClientsTableCommand : Command<ShowCommandsSettings>
    {
        private readonly List<string> _columns
            = new () { "Name", "Surname", "PassportId", "Address", "Phone number", "Money without credits" };

        public override int Execute(CommandContext context, ShowCommandsSettings settings)
        {
            IReadOnlyList<Client> clients = CreateCentralBankCommand.CentralBank.Clients;

            Table clientsTable = new Table().Title("All clients")
                .Centered().Expand().RoundedBorder();

            Task.Run(async () =>
            {
                await AnsiConsole.Live(clientsTable)
                    .StartAsync(async ctx =>
                    {
                        foreach (string column in _columns)
                        {
                            clientsTable.AddColumn(new TableColumn($"[bold green]{column}[/]").Centered());
                            ctx.Refresh();
                            await Task.Delay(300);
                        }

                        ctx.Refresh();
                        await Task.Delay(750);

                        foreach (Client client in clients)
                        {
                            clientsTable.AddRow(
                                $"[bold aqua]{client.Name}[/]",
                                $"[bold aqua]{client.Surname}[/]",
                                $"{client.PassportId}",
                                $"{client.Address}",
                                $"{client.PhoneNumber ?? "-"}",
                                $"{CalculateSumOfMoneyInAllAccounts(client):C}");

                            ctx.Refresh();
                            await Task.Delay(500);
                        }
                    });
            });

            AnsiConsole.WriteLine();

            return 0;
        }

        private decimal CalculateSumOfMoneyInAllAccounts(Client client)
        {
            var clientBanks = CreateCentralBankCommand.CentralBank.Banks
                .Where(b => b.IsClientExists(client)).ToList();

            var clientAccountsInBanks = clientBanks
                .SelectMany(b => b.ClientsAndAccounts
                    .Where(acc => acc.Key.Equals(client))).ToList();

            var clientAccountsWithoutCreditOnes = clientAccountsInBanks
                .Select(kvp => kvp.Value
                    .Where(acc => acc is not CreditAccount)).ToList();

            var clientAccounts = new List<IAccount>();

            foreach (IEnumerable<IAccount> clientBankAccounts in clientAccountsWithoutCreditOnes)
                clientAccounts.AddRange(clientBankAccounts);

            return clientAccounts.Sum(acc => acc.GetAmountOfMoney());
        }
    }
}