using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob : IBackupJob
    {
        private int _restorePointsCreatedCount;

        public BackupJob(string name, IAlgorithm compressingAlgorithm)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BackupException("Backup job name is null", new ArgumentNullException(nameof(name)));

            if (compressingAlgorithm is null)
                throw new BackupException("Compressing algorithm to create backup job is null");

            Name = name;
            Algorithm = compressingAlgorithm;
            FilesToBackupList = new List<JobObject>();
            RestorePointsList = new LinkedList<RestorePoint>();

            XmlFileLogger.LogBackup(Name);
        }

        public string Name { get; }
        public AlgorithmTypes AlgorithmType { get; private set; }

        public IReadOnlyList<RestorePoint> RestorePoints => RestorePointsList.ToList();
        public RestorePoint LastRestorePoint => RestorePointsList.Last?.Value ?? throw new BackupException("No restore points");
        public IReadOnlyList<JobObject> FilesToBackup => FilesToBackupList;

        protected List<JobObject> FilesToBackupList { get; }
        protected LinkedList<RestorePoint> RestorePointsList { get; }
        protected IAlgorithm Algorithm { get; private set; }

        public void SetCompressionAlgorithm(IAlgorithm compressionAlgorithm)
        {
            if (!Enum.TryParse(compressionAlgorithm.ToString(), out AlgorithmTypes algoType))
                throw new BackupException("Can't recognize config file: algorithm type can't be found");

            AlgorithmType = algoType;
            Algorithm = compressionAlgorithm;
        }

        /// <summary>
        /// Creates new RestorePoint and clears _filesToBackup list.
        /// </summary>
        /// <param name="pointName">Name of restore point to create.</param>>
        /// <param name="creationTime">Restore point's creation time.</param>>
        public void CreateRestorePoint(string pointName, DateTime? creationTime = null)
        {
            if (Algorithm is null)
                throw new BackupException("Set archive algorithm before using restore point creation");

            if (string.IsNullOrWhiteSpace(pointName))
                throw new BackupException("Point name to create restore point is empty");

            var restorePoint = new RestorePoint(++_restorePointsCreatedCount, Algorithm, pointName, creationTime);
            RestorePointsList.AddLast(restorePoint);

            Algorithm.ArchiveFiles(this)
                .ToList().ForEach(storage => restorePoint.AddStorage(storage));

            XmlFileLogger.LogRestorePoint(restorePoint, Name);
        }

        public void AddFilesToBackup(List<JobObject> filesToBackup)
        {
            if (filesToBackup is null)
                throw new BackupException("Files to add to backup are null", new ArgumentNullException(nameof(filesToBackup)));

            foreach (JobObject file in filesToBackup)
            {
                if (IsFileToBackupExist(file))
                    throw new BackupException("File to add to backup is already exists");

                FilesToBackupList.Add(file);
            }
        }

        public void RemoveFilesFromBackup(List<JobObject> filesToBackup)
        {
            if (filesToBackup is null)
                throw new BackupException("Files to remove from backup are null", new ArgumentNullException(nameof(filesToBackup)));

            if (filesToBackup.Any(file => !FilesToBackupList.Remove(file)))
                throw new BackupException("File to remove from backup doesn't exist");
        }

        public void AddFileToBackup(JobObject fileToBackup) => AddFilesToBackup(new List<JobObject> { fileToBackup });
        public void RemoveFileFromBackup(JobObject fileToBackup) => RemoveFilesFromBackup(new List<JobObject> { fileToBackup });

        public override bool Equals(object? obj) => Equals(obj as BackupJob);
        public override int GetHashCode() => HashCode.Combine(Name);
        private bool Equals(BackupJob? backupJob) => backupJob is not null && backupJob.Name == Name;

        private bool IsFileToBackupExist(JobObject file) => FilesToBackupList.Any(f => f.Equals(file));
    }
}