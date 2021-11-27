using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(int pointNumber, IAlgorithm compressingAlgorithm, string pointName, DateTime? creationTime = null)
        {
            if (pointNumber <= 0)
                throw new BackupException("Restore point number must be positive");

            PointNumber = pointNumber;
            AlgorithmType = compressingAlgorithm;
            Name = pointName;

            CreationTime = creationTime ?? DateTime.UtcNow;
            Storages = new List<Storage>();
        }

        public int PointNumber { get; }
        public string Name { get; }
        public DateTime CreationTime { get; }
        public IAlgorithm AlgorithmType { get; }
        public IReadOnlyList<Storage> StorageFiles => Storages;

        protected List<Storage> Storages { get; }

        public void AddStorage(Storage storage)
        {
            if (IsStorageExists(storage))
                throw new BackupException($"Storage to add already exists in {Name}");

            Storages.Add(storage);
        }

        public override bool Equals(object? obj) => Equals(obj as RestorePoint);
        public override int GetHashCode() => HashCode.Combine(Name);

        private bool Equals(RestorePoint? restorePoint) => restorePoint is not null && restorePoint.Name == Name &&
                                                           restorePoint.PointNumber == PointNumber &&
                                                           restorePoint.AlgorithmType == AlgorithmType;

        private bool IsStorageExists(Storage storage) => Storages.Any(s => s.Equals(storage));
    }
}