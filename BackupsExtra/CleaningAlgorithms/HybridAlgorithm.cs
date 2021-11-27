using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Tools;
using BackupsExtra.Entities;
using BackupsExtra.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.CleaningAlgorithms
{
    public class HybridAlgorithm : ICleaningAlgorithm
    {
        private readonly IReadOnlyCollection<ICleaningAlgorithm> _cleaningAlgorithms;

        public HybridAlgorithm(IReadOnlyCollection<ICleaningAlgorithm> cleaningAlgorithms, PointsCleaningRule cleaningRule)
        {
            if (cleaningAlgorithms is null)
                throw new BackupException("Cleaning algorithms to create hybrid algorithm are null");

            _cleaningAlgorithms = cleaningAlgorithms;
            CleaningRule = cleaningRule;
        }

        public PointsCleaningRule CleaningRule { get; }

        public IReadOnlyCollection<RestorePoint> FindPointsToClean(ExtraBackupJob backupJob)
        {
            var pointsToRemove = _cleaningAlgorithms
                .SelectMany(algo => algo.FindPointsToClean(backupJob)).ToList();

            return CleaningRule switch
            {
                PointsCleaningRule.AtLeastOneRule => pointsToRemove.Distinct().ToList(),
                PointsCleaningRule.AllRule => pointsToRemove
                    .Where(point => pointsToRemove.Count(p => p.Equals(point)) == _cleaningAlgorithms.Count)
                    .Distinct().ToList(),
                _ => throw new BackupException("Cleaning rule can't be recognized")
            };
        }
    }
}