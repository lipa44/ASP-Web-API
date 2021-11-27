using System.ComponentModel;
using Banks.Entities;
using Banks.SpectreConsole.CreateCommands;
using Banks.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole
{
    public class TimeRewindCommand : Command<TimeRewindCommand.Settings>
    {
        public override int Execute(CommandContext context, Settings settings)
        {
            CentralBank cb = CreateCentralBankCommand.CentralBank;

            if (cb is null)
            {
                ConsoleMethods.WriteErrorRule("Central bank to rewind time", "doesn't exist");
                return 1;
            }

            DateTimeProvider.RewindTime(settings.MonthsAmount);
            cb.TryUpdateAllPercentAccounts();

            ConsoleMethods.WriteGeneralActionRule($"Time rewound for {settings.MonthsAmount} months", "successfully");

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandOption("-m|--months <MONTHS_AMOUNT>")]
            [Description("Rewinds time.")]
            [CommandArgument(0, "<MONTHS_AMOUNT>")]
            public int MonthsAmount { get; init; }
        }
    }
}