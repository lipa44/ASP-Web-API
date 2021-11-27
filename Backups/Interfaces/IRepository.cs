using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IRepository
    {
        void Save(BackupJob backupJob);
    }
}