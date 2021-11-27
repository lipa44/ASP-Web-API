using Serilog;
using Serilog.Core;

namespace BackupsExtra.BackupsLogger
{
    public class ConsoleLoggerBuilder : BackupsLoggerBuilder
    {
        public override Logger CreateInstance()
            => new LoggerConfiguration()
                .WriteTo.ColoredConsole(outputTemplate:
                    "[ " + $"{TimeFormat}" + " {Level:u" + $"{LevelSymbols}" + "} ] " + "{Message:lj} {NewLine}")
                .CreateLogger();
    }
}