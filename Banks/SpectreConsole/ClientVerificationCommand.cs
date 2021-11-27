using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole
{
    public class ClientVerificationCommand : Command<ClientVerificationCommand.Settings>
    {
        public override int Execute(CommandContext context, Settings settings)
        {
            Client client;
            if (settings.ClientId == Guid.Empty)
                ConsoleMethods.AskClientIdAndInitializeClient(out client);
            else
                client = CreateCentralBankCommand.CentralBank.GetClient(settings.ClientId);

            if (client.IsVerified())
            {
                ConsoleMethods.WriteGeneralActionRule($"Client {client}", "already verified");
                return 0;
            }

            List<string> uninitializedParams = ClientUninitializedParams(client);
            uninitializedParams.Add("None");

            List<string> paramsToInitialize = ParamsToInitializePrompt(uninitializedParams);

            foreach (string param in paramsToInitialize)
            {
                switch (param)
                {
                    case "Address":
                        string clientAddress = AnsiConsole.Ask<string>("[bold green]Your address[/]?");
                        AnsiConsole.WriteLine();

                        try
                        {
                            client.SetAddress(clientAddress);
                        }
                        catch (BankException)
                        {
                            ConsoleMethods.WriteErrorRule("Address", "is incorrect");
                            return 0;
                        }

                        break;

                    case "Phone number":
                        string clientPhoneNumber = AnsiConsole.Ask<string>("[bold green]Your phone number[/]?");
                        AnsiConsole.WriteLine();

                        try
                        {
                            client.SetPhoneNumber(clientPhoneNumber);
                        }
                        catch (BankException)
                        {
                            ConsoleMethods.WriteErrorRule("Phone number", "is incorrect");
                            return 0;
                        }

                        break;

                    default:
                        ConsoleMethods.WriteErrorRule($"{client} optional parameters", "didn't added");
                        return 0;
                }
            }

            ConsoleMethods.WriteGeneralActionRule($"{client} optional parameters", "successfully added");

            return 0;
        }

        private List<string> ClientUninitializedParams(Client client) => client.Address switch
        {
            null when client.PhoneNumber is null => new List<string> { "Address", "Phone number" },
            null => new List<string> { "Address" },
            _ => new List<string> { "Phone number" }
        };

        private List<string> ParamsToInitializePrompt(List<string> uninitializedParams) => AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .PageSize(3)
                .Title("Which params do you want to fill?")
                .AddChoices(uninitializedParams));

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[CLIENT_ID]")]
            [DefaultValue("00000000-0000-0000-0000-000000000000")]
            public Guid ClientId { get; init; }
        }
    }
}