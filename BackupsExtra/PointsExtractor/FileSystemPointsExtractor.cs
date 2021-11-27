using System.IO;
using System.Linq;
using Aspose.Zip;
using Backups.Entities;
using Backups.Tools;
using Logger = BackupsExtra.BackupsLogger.BackupsLogger;

namespace BackupsExtra.PointsExtractor
{
    public class FileSystemPointsExtractor : IPointsExtractor
    {
        private readonly FileSystemRepository _repository;
        private readonly DirectoryInfo _pathToExtract;

        public FileSystemPointsExtractor(FileSystemRepository repository, DirectoryInfo directoryInfo)
        {
            _repository = repository ?? throw new BackupException("Repository to create extractor is null");
            _pathToExtract = directoryInfo ?? throw new BackupException("Path to extract restore point is null");
        }

        public void ExtractPointToDirectory(RestorePoint restorePoint)
        {
            BackupJob backup = GetBackupJobByRestorePoint(restorePoint);
            Directory.CreateDirectory(_pathToExtract.FullName);

            foreach (Storage storage in restorePoint.StorageFiles)
            {
                string pathToStorageZip = Path.Combine(
                    _repository.RepoDirectory.FullName, backup.Name, restorePoint.Name, $"{storage.FullName}.zip");

                using FileStream zipFile = File.Open(pathToStorageZip, FileMode.Open);
                using var archive = new Archive(zipFile);
                foreach (ArchiveEntry archiveEntry in archive.Entries)
                {
                    archiveEntry.Extract(Path.Combine(_pathToExtract.FullName, archiveEntry.Name));
                    Logger.LogFileExtracted(archiveEntry.Name, _pathToExtract.FullName);
                }
            }
        }

        private BackupJob GetBackupJobByRestorePoint(RestorePoint restorePoint)
            => _repository.BackupJobs.SingleOrDefault(b => b.RestorePoints.Contains(restorePoint))
               ?? throw new BackupException($"BackupJob with {restorePoint.Name} can't be found");
    }
}