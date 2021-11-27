using System;
using System.Collections.Generic;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.SpectreConsole.SubscriptionCommands.SubscriptionCommandsSettings;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.SubscriptionCommands
{
    public class GetSubscriptionUpdatesCommand : Command<GetSubscriptionUpdatesSettings>
    {
        public override int Execute(CommandContext context, GetSubscriptionUpdatesSettings settings)
        {
            Client client;
            if (settings.ClientId == Guid.Empty)
                ConsoleMethods.AskClientIdAndInitializeClient(out client);
            else
                client = CreateCentralBankCommand.CentralBank.GetClient(settings.ClientId);

            List<string> updates = client.GetBankParametersUpdates();

            foreach (string update in updates)
                AnsiConsole.Write(new Markup(update + "\n"));

            ConsoleMethods.WriteCustomRule(
                $"[bold]All[/] [bold aqua on grey]subscription updates[/] [bold]for {client}[/] " +
                "[bold chartreuse1]successfully got[/]!",
                BoxBorder.Rounded);

            return 0;
        }
    }
}