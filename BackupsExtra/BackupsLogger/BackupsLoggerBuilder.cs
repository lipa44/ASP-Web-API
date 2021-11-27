using Backups.Tools;
using BackupsExtra.Interfaces;
using Serilog.Core;

namespace BackupsExtra.BackupsLogger
{
    public abstract class BackupsLoggerBuilder : ILoggerBuilder
    {
        protected int LevelSymbols { get; set; }
        protected string TimeFormat { get; set; }

        public BackupsLoggerBuilder SetLevelSymbolsAmount(int symbolsAmount)
        {
            if (symbolsAmount <= 0)
                throw new BackupException("Symbols amount must be positive");

            LevelSymbols = symbolsAmount;
            return this;
        }

        public BackupsLoggerBuilder SetIsWithTime(string timeFormatPattern)
        {
            if (string.IsNullOrWhiteSpace(timeFormatPattern))
                throw new BackupException("Time format for logger is empty");

            TimeFormat = "{Timestamp:" + timeFormatPattern + "}";
            return this;
        }

        public abstract Logger CreateInstance();
    }
}