using System;
using System.Collections.Generic;
using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IBackupJob
    {
        void SetCompressionAlgorithm(IAlgorithm compressionAlgorithm);
        void CreateRestorePoint(string pointName, DateTime? creationTime = null);
        void AddFilesToBackup(List<JobObject> filesToBackup);
        void RemoveFilesFromBackup(List<JobObject> filesToBackup);
        void AddFileToBackup(JobObject fileToBackup);
        void RemoveFileFromBackup(JobObject fileToBackup);
    }
}