using Backups.Tools;
using BackupsExtra.Entities;

namespace BackupsExtra.ConflictsResolverState
{
    public abstract class ConflictsResolverState
    {
        protected ExtraBackupJob ExtraBackupJob { get; set; }

        public void SetBackupJobContext(ExtraBackupJob extraBackupJob)
        {
            if (extraBackupJob is null)
                throw new BackupException("Backup job as context in conflicts resolver state is null");

            ExtraBackupJob = extraBackupJob;
        }

        public abstract void ResolveConflicts();
    }
}