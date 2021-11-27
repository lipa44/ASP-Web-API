using System;

namespace Backups.Interfaces
{
    public interface ILogger
    {
        void LogRepo(string repoPath);
        void LogBackup(string backupName);
        void LogRestorePoint(int pointNumber, Type algoType, string backupName);
        void LogRestoreFile(string zipName, string backupName, string pointName);
    }
}