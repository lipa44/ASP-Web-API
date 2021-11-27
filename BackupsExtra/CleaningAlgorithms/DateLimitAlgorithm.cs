using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Entities;
using BackupsExtra.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.CleaningAlgorithms
{
    public class DateLimitAlgorithm : ICleaningAlgorithm
    {
        private readonly TimeSpan _timeBeforeDeletePoint;

        public DateLimitAlgorithm(TimeSpan timeBeforeDeletePoint) =>
            _timeBeforeDeletePoint = timeBeforeDeletePoint;

        public IReadOnlyCollection<RestorePoint> FindPointsToClean(ExtraBackupJob backupJob)
            => backupJob.RestorePoints.Where(
                p => DateTimeProvider.IsNeedToClearPoints(p.CreationTime, _timeBeforeDeletePoint)).ToList();
    }
}