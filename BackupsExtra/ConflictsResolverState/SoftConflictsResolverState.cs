using Backups.Entities;
using Backups.Tools;

namespace BackupsExtra.ConflictsResolverState
{
    public class SoftConflictsResolverState : ConflictsResolverState
    {
        public override void ResolveConflicts()
        {
            if (ExtraBackupJob is null)
                throw new BackupException("Extra backup to resolve conflicts didn't set");

            foreach (RestorePoint restorePointToRemove in ExtraBackupJob.PointsToResolveConflicts())
                ExtraBackupJob.MergePoints(restorePointToRemove, ExtraBackupJob.LastRestorePoint);
        }
    }
}