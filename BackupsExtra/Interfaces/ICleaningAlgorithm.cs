using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.Entities;

namespace BackupsExtra.Interfaces
{
    public interface ICleaningAlgorithm
    {
        IReadOnlyCollection<RestorePoint> FindPointsToClean(ExtraBackupJob backupJob);
    }
}