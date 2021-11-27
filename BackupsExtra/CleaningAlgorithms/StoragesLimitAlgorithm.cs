using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Tools;
using BackupsExtra.Entities;
using BackupsExtra.Interfaces;

namespace BackupsExtra.CleaningAlgorithms
{
    public class StoragesLimitAlgorithm : ICleaningAlgorithm
    {
        private readonly int _maxAmountOfStorages;

        public StoragesLimitAlgorithm(int maxAmountOfStorages) =>
            _maxAmountOfStorages = maxAmountOfStorages > 0
                ? maxAmountOfStorages
                : throw new BackupException("Max amount of storages for cleaning algorithm must be positive");

        public IReadOnlyCollection<RestorePoint> FindPointsToClean(ExtraBackupJob backupJob) =>
            CalculateAmountOfStorages(backupJob.RestorePoints) <= _maxAmountOfStorages
                ? new List<RestorePoint>()
                : backupJob.RestorePoints.Take(GetAmountOfPointsToRemove(backupJob.RestorePoints)).ToList();

        private int GetAmountOfPointsToRemove(IReadOnlyCollection<RestorePoint> restorePoints)
        {
            int storagesCount = CalculateAmountOfStorages(restorePoints);
            int pointsCount = 0;
            return restorePoints
                .ToDictionary(count => ++pointsCount, storagesLeft =>
                    storagesCount -= storagesLeft.StorageFiles.Count)
                .FirstOrDefault(ptsAndStoragesCount => ptsAndStoragesCount.Value <= _maxAmountOfStorages).Key;
        }

        private int CalculateAmountOfStorages(IReadOnlyCollection<RestorePoint> restorePoints) =>
            restorePoints.Sum(restorePoint => restorePoint.StorageFiles.Count);
    }
}