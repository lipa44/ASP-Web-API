using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class FileSystemRepository : IRepository
    {
        private readonly List<BackupJob> _backupJobs;
        private readonly List<RestorePoint> _savedPoints;

        public FileSystemRepository(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new BackupException("Directory to create repository is null", new ArgumentNullException(nameof(directoryPath)));

            RepoDirectory = new DirectoryInfo(directoryPath);
            _backupJobs = new List<BackupJob>();
            _savedPoints = new List<RestorePoint>();

            XmlFileLogger.LogRepo(RepoDirectory.FullName);
        }

        public DirectoryInfo RepoDirectory { get; }
        public IReadOnlyList<BackupJob> BackupJobs => _backupJobs;

        /// <summary>
        /// Saves all files produced in backupJob's lastRestorePoint.
        /// </summary>
        /// <param name="backupJob">BackupJob, which contains restorePoint to save.</param>>
        public void Save(BackupJob backupJob)
        {
            string backupDir = Path.Combine(RepoDirectory.FullName, backupJob.Name);

            foreach (RestorePoint restorePoint in GetUnsavedRestorePoints(backupJob))
            {
                Directory.CreateDirectory(Path.Combine(backupDir, restorePoint.Name));
                foreach (Storage storage in restorePoint.StorageFiles)
                {
                    storage.Archive.Save(Path.Combine(backupDir, restorePoint.Name, $"{storage.FullName}.zip"));

                    XmlFileLogger.LogFile(storage, backupJob.Name, restorePoint.Name);
                }

                _savedPoints.Add(restorePoint);
            }
        }

        public void AddBackupJob(BackupJob backupJob)
        {
            if (backupJob is null)
                throw new BackupException("Backup to add into repository is null");

            if (IsBackupJobExist(backupJob))
                throw new BackupException("Backup already added");

            _backupJobs.Add(backupJob);
        }

        private IReadOnlyCollection<RestorePoint> GetUnsavedRestorePoints(BackupJob backupJob) => backupJob.RestorePoints
            .Where(restorePoint => !IsRestorePointExist(restorePoint)).ToList();

        private bool IsBackupJobExist(BackupJob backupJob) => _backupJobs.Any(b => b.Equals(backupJob));
        private bool IsRestorePointExist(RestorePoint restorePoint) => _savedPoints.Any(p => p.Equals(restorePoint));
    }
}