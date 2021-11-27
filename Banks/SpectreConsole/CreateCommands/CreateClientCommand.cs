using System;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands.CreateSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands
{
    public class CreateClientCommand : Command<CreateClientSettings>
    {
        public override int Execute(CommandContext context, CreateClientSettings settings)
        {
            do
            {
                ConsoleMethods.WriteOptionalActionRule(
                    $"Client {settings.ClientName} {settings.ClientSurname}", "general settings");

                settings.PassportId = AnsiConsole.Prompt(new TextPrompt<Guid>("[bold green]Your passport Id[/]?"));

                settings.Address =
                    AnsiConsole.Prompt(new TextPrompt<string>("[grey][[Optional]][/] [bold green]Your address[/]?")
                        .AllowEmpty());

                settings.PhoneNumber =
                    AnsiConsole.Prompt(new TextPrompt<string>("[grey][[Optional]][/] [bold green]Your phone number[/]?")
                        .AllowEmpty());

                Table clientDataTable = new Table()
                    .Title($"[bold green]{settings.ClientName} {settings.ClientSurname}[/]")
                    .AddColumns("Name", "Surname", "Passport ID", "Address", "Phone number")
                    .Centered();

                clientDataTable.AddRow(
                        settings.ClientName,
                        settings.ClientSurname,
                        settings.PassportId.ToString(),
                        string.IsNullOrEmpty(settings.Address) ? "-" : settings.Address,
                        string.IsNullOrEmpty(settings.PhoneNumber) ? "-" : settings.PhoneNumber)
                    .Centered();

                AnsiConsole.Write(clientDataTable);
                AnsiConsole.WriteLine();
            }
            while (!AnsiConsole.Confirm("[bold]All right?[/]"));
            AnsiConsole.WriteLine();

            try
            {
                CreateCentralBankCommand.CentralBank.GetClient(settings.PassportId);
                ConsoleMethods.WriteErrorRule($"Client with ID {settings.PassportId}", "already exists");
            }
            catch (BankException)
            {
                Client client = CreateCentralBankCommand.CentralBank
                    .RegisterClient(settings.ClientName, settings.ClientSurname, settings.PassportId);

                if (!string.IsNullOrEmpty(settings.Address))
                    client.SetAddress(settings.Address);

                if (!string.IsNullOrEmpty(settings.PhoneNumber))
                    client.SetPhoneNumber(settings.PhoneNumber);

                ConsoleMethods.WriteGeneralActionRule(
                    $"Client {settings.ClientName} {settings.ClientSurname}", "successfully created");
            }

            return 0;
        }
    }
}