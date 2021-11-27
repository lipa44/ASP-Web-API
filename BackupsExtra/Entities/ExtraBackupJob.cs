using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Tools;
using Logger = BackupsExtra.BackupsLogger.BackupsLogger;

namespace BackupsExtra.Entities
{
    public class ExtraBackupJob : BackupJob
    {
        private ConflictsResolverState.ConflictsResolverState _conflictsResolver;

        public ExtraBackupJob(string name, ConflictsResolverState.ConflictsResolverState conflictsResolver, BackupJobSettings backupJobSettings)
            : base(name, backupJobSettings.CompressingAlgorithm)
        {
            if (backupJobSettings is null)
                throw new BackupException("Backup settings to create extra backup job are null");

            if (conflictsResolver is null)
                throw new BackupException("Conflicts resolver state to create extra backup job null");

            _conflictsResolver = conflictsResolver;
            _conflictsResolver.SetBackupJobContext(this);
            BackupJobSettings = backupJobSettings;

            Logger.LogBackupJob(name);
        }

        public ConflictsResolverState.ConflictsResolverState ResolverState
        {
            get => _conflictsResolver;
            set => SetConflictsResolverState(value);
        }

        public BackupJobSettings BackupJobSettings { get; }

        public void SafetyCreateRestorePoint(string pointName, DateTime? creationTime = null)
        {
            CreateRestorePoint(pointName, creationTime);

            ResolverState.ResolveConflicts();

            foreach (Storage storage in LastRestorePoint.StorageFiles)
                Logger.LogFileRestored(storage.FullName);

            Logger.LogRestorePoint(LastRestorePoint.Name);
        }

        public void MergePoints(RestorePoint pointToRemove, RestorePoint pointToMergeIn)
        {
           new PointsMerger(pointToRemove, pointToMergeIn).Merge();

           RemoveRestorePoint(pointToRemove);

           Logger.LogMerge(pointToRemove.Name, pointToMergeIn.Name);
        }

        public void RemoveRestorePoint(RestorePoint restorePoint)
        {
            if (restorePoint is null)
                throw new BackupException("Restore point to delete is null");

            if (!RestorePointsList.Remove(restorePoint))
                throw new BackupException($"Restore point to delete doesn't exist in {restorePoint.Name}");

            Logger.LogPointRemoved(restorePoint.Name);
        }

        public void CleanRestorePoints()
        {
            IReadOnlyCollection<RestorePoint> pointsToClean = PointsToResolveConflicts();

            if (pointsToClean.Count == RestorePointsList.Count)
                throw new BackupException("Something went wrong: all points need to be cleaned");

            pointsToClean.ToList().ForEach(RemoveRestorePoint);
        }

        public IReadOnlyCollection<RestorePoint> PointsToResolveConflicts()
            => BackupJobSettings.CleaningAlgorithm.FindPointsToClean(backupJob: this);

        private void SetConflictsResolverState(ConflictsResolverState.ConflictsResolverState conflictsResolverState)
        {
            if (conflictsResolverState is null)
                throw new BackupException("Conflicts resolver state to set is null");

            conflictsResolverState.SetBackupJobContext(this);
            _conflictsResolver = conflictsResolverState;
        }
    }
}