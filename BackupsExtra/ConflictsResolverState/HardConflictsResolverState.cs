using Backups.Tools;

namespace BackupsExtra.ConflictsResolverState
{
    public class HardConflictsResolverState : ConflictsResolverState
    {
        public override void ResolveConflicts()
        {
            if (ExtraBackupJob is null)
                throw new BackupException("Extra backup to resolve conflicts didn't set");

            ExtraBackupJob.CleanRestorePoints();
        }
    }
}