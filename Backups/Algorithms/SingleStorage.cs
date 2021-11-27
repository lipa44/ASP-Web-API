using System;
using System.Collections.Generic;
using Aspose.Zip;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SingleStorage : IAlgorithm
    {
        public IReadOnlyCollection<Storage> ArchiveFiles(BackupJob backupJob)
        {
            if (backupJob is null)
                throw new BackupException("Backup job to archive files is null", new ArgumentNullException(nameof(backupJob)));

            var archive = new Archive();
            foreach (JobObject file in backupJob.FilesToBackup)
                archive.CreateEntry(file.FileInfo.Name, file.FileInfo);

            RestorePoint lastPoint = backupJob.LastRestorePoint;
            var storage = new Storage(archive, $"{lastPoint.Name}", lastPoint);

            return archive.Entries.Count != 0 ? new List<Storage> { storage } : new List<Storage>();
        }

        public override string ToString() => AlgorithmTypes.SingleAlgorithm.ToString();
    }
}