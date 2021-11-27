using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Tools;
using BackupsExtra.Entities;
using BackupsExtra.Interfaces;

namespace BackupsExtra.CleaningAlgorithms
{
    public class PointsLimitAlgorithm : ICleaningAlgorithm
    {
        private readonly int _maxAmountOfPoints;

        public PointsLimitAlgorithm(int maxAmountOfPoints) =>
            _maxAmountOfPoints = maxAmountOfPoints > 0
                ? maxAmountOfPoints
                : throw new BackupException("Max amount of points for cleaning algorithm must be positive");

        public IReadOnlyCollection<RestorePoint> FindPointsToClean(ExtraBackupJob backupJob) =>
            backupJob.RestorePoints.Count <= _maxAmountOfPoints
                ? new List<RestorePoint>()
                : backupJob.RestorePoints.Take(backupJob.RestorePoints.Count - _maxAmountOfPoints).ToList();
    }
}