using System.IO;
using Aspose.Zip;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Tools;

namespace BackupsExtra.Restorer
{
    public class FileSystemRestorerManager
    {
        public static void AddArchiveEntitiesToStorage(Archive oldArchive, RestorePoint point)
        {
            if (oldArchive is null)
                throw new BackupException("Archive to add entities from is null");

            if (point is null)
                throw new BackupException("Point to add entities is null");

            switch (point.AlgorithmType)
            {
                case SplitStorage:
                    AddEntitiesToSplitStorage(oldArchive, point);
                    break;

                case SingleStorage:
                    AddEntitiesToSingleStorage(oldArchive, point);
                    break;

                default:
                    throw new BackupException("Archive entities can't be added into storage");
            }
        }

        private static void AddEntitiesToSplitStorage(Archive oldArchive, RestorePoint point)
        {
            foreach (ArchiveEntry archiveEntry in oldArchive.Entries)
            {
                var newArchive = new Archive();
                Stream archiveEntryStream = archiveEntry.Open();

                newArchive.CreateEntry(archiveEntry.Name, archiveEntryStream);

                string newZipName = RemoveExtension(new (archiveEntry.Name));
                point.AddStorage(new (newArchive, newZipName, point));

                archiveEntryStream.Dispose();
            }
        }

        private static void AddEntitiesToSingleStorage(Archive zip, RestorePoint point)
            => point.AddStorage(new (zip, point.Name, point));

        private static string RemoveExtension(FileInfo fileName) =>
            fileName.Name.Replace(fileName.Extension, string.Empty);
    }
}