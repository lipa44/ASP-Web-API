using System;
using System.IO;
using Backups.Tools;

namespace Backups.Entities
{
    public class JobObject
    {
        public JobObject(string name, string path)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BackupException("File name is null", new ArgumentNullException(nameof(name)));

            if (string.IsNullOrWhiteSpace(path))
                throw new BackupException("File path is null", new ArgumentNullException(nameof(path)));

            FileInfo = new FileInfo(Path.Combine(path, name));
        }

        public FileInfo FileInfo { get; }

        public override bool Equals(object? obj) => Equals(obj as JobObject);
        public override int GetHashCode() => HashCode.Combine(FileInfo);

        private bool Equals(JobObject? jobObject) => jobObject is not null && jobObject.FileInfo == FileInfo;
    }
}