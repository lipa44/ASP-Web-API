using System;
using Aspose.Zip;
using Backups.Tools;

namespace Backups.Entities
{
    public class Storage
    {
        public Storage(Archive archive, string zipName, RestorePoint point)
        {
            if (archive is null)
                throw new BackupException("Zip bytes to create storage is null", new ArgumentNullException(nameof(archive)));

            if (string.IsNullOrWhiteSpace(zipName))
                throw new BackupException("Zip file name to create storage is null", new ArgumentNullException(nameof(zipName)));

            Archive = archive;
            InitialName = zipName;
            FullName = $"{zipName}_{point.PointNumber}";
        }

        public Archive Archive { get; }
        public string FullName { get; }
        public string InitialName { get; }

        public override bool Equals(object? obj) => Equals(obj as Storage);
        public override int GetHashCode() => HashCode.Combine(InitialName);
        private bool Equals(Storage? storage) => storage is not null && storage.InitialName == InitialName;
    }
}