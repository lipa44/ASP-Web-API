using Backups.Tools;
using Serilog.Core;

namespace BackupsExtra.BackupsLogger
{
    public static class BackupsLogger
    {
        private static Logger _serilogLogger;

        public static Logger SetInstance(Logger backupsLogger)
        {
            if (_serilogLogger is null)
            {
                _serilogLogger = backupsLogger;
                return _serilogLogger;
            }

            _serilogLogger.Error("Logger can be created only once");
            throw new BackupException("Logger can be created only once successfully");
        }

        public static void LogBackupJob(string jobName)
            => _serilogLogger?.Information($"BackupJob {jobName} created successfully");

        public static void LogRestorePoint(string pointName)
            => _serilogLogger?.Information($"Restore point {pointName} created successfully");

        public static void LogFileRestored(string fileName)
            => _serilogLogger?.Information($"File {fileName} restored successfully");

        public static void LogFileExtracted(string fileName, string dir)
            => _serilogLogger?.Information($"File {fileName} extracted to {dir} successfully");

        public static void LogMerge(string firstPoint, string secondPoint)
            => _serilogLogger?.Warning($"Restore points {firstPoint} and {secondPoint} merged successfully");

        public static void LogPointRemoved(string restorePoint)
            => _serilogLogger?.Information($"Restore point {restorePoint} removed successfully");
    }
}