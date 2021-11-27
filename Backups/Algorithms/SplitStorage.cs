using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Zip;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SplitStorage : IAlgorithm
    {
        public IReadOnlyCollection<Storage> ArchiveFiles(BackupJob backupJob)
        {
            if (backupJob is null)
                throw new BackupException("Backup job to archive files is null", new ArgumentNullException(nameof(backupJob)));

            var storages = new List<Storage>();

            foreach (JobObject file in backupJob.FilesToBackup)
            {
                var archive = new Archive();
                archive.CreateEntry(file.FileInfo.Name, file.FileInfo);
                storages.Add(new Storage(archive, RemoveExtension(file.FileInfo), backupJob.LastRestorePoint));
            }

            return storages;
        }

        public override string ToString() => AlgorithmTypes.SplitAlgorithm.ToString();

        private string RemoveExtension(FileInfo fileName) =>
            fileName.Name.Replace(fileName.Extension, string.Empty);
    }
}