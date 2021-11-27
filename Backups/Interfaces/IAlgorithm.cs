using System.Collections.Generic;
using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IAlgorithm
    {
        IReadOnlyCollection<Storage> ArchiveFiles(BackupJob backupJob);
    }
}