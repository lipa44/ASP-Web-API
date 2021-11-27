using System.Collections.Generic;
using System.Linq;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Tools;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class PointsMerger : IPointsMerger
    {
        public PointsMerger(RestorePoint pointToRemove, RestorePoint pointToMergeIn)
        {
            PointToRemove = pointToRemove ?? throw new BackupException("Point to remove in merge operation is null");
            PointToMergeIn = pointToMergeIn ?? throw new BackupException("Point to merge in in merge operation is null");
        }

        private RestorePoint PointToRemove { get; }
        private RestorePoint PointToMergeIn { get; }

        public void Merge()
        {
            if (PointToRemove.AlgorithmType is SplitStorage)
                ExceptStorages(PointToRemove, PointToMergeIn).ForEach(PointToMergeIn.AddStorage);
        }

        private List<Storage> ExceptStorages(RestorePoint pointToRemove, RestorePoint pointToMergeIn)
            => pointToRemove.StorageFiles.Where(storage => pointToMergeIn.StorageFiles.All(s => !s.Equals(storage))).ToList();
    }
}