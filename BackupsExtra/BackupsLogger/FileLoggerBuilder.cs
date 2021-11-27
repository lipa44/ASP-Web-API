using System.IO;
using Backups.Tools;
using Serilog;
using Serilog.Core;

namespace BackupsExtra.BackupsLogger
{
    public class FileLoggerBuilder : BackupsLoggerBuilder
    {
        private string _logFilePath;

        public FileLoggerBuilder SetFilePath(FileInfo filePath)
        {
            if (filePath is null)
                throw new BackupException("File path to set logger is null");

            _logFilePath = filePath.FullName;
            return this;
        }

        public override Logger CreateInstance()
            => new LoggerConfiguration()
                .WriteTo.File(
                    _logFilePath,
                    outputTemplate: "[ " + $"{TimeFormat}" + " {Level:u" + $"{LevelSymbols}" + "} ] " + "{Message:lj} {NewLine}").CreateLogger();
    }
}