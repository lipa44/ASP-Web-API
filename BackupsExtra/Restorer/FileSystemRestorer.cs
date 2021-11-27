using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Aspose.Zip;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace BackupsExtra.Restorer
{
    public class FileSystemRestorer
    {
        private readonly FileInfo _restoreFilePath;
        private XDocument _document = new ();

        public FileSystemRestorer(FileInfo restoreFilePath)
                => _restoreFilePath = restoreFilePath
                                      ?? throw new BackupException("Restore XML file path is null");

        public IReadOnlyCollection<BackupJob> RestoreRepository()
        {
            _document = XDocument.Load(_restoreFilePath.FullName);

            XElement oldRepoNode = XmlAlgorithms.GetRepoInXml(_document);
            string oldRepoPath = GetAttributeValueByName(oldRepoNode, "repoPath");
            var restoredBackupJobs = new List<BackupJob>();

            foreach (XElement backupJob in GetBackupsFromConfigByRepo(oldRepoNode))
            {
                string backupName = GetAttributeValueByName(backupJob, "backupName");
                BackupJob backup = new (backupName, GetFirstPointCompressingAlgorithm(backupJob));
                restoredBackupJobs.Add(backup);

                foreach (XElement restorePoint in GetRestorePointsFromConfigByBackup(backupJob))
                {
                    if (!Enum.TryParse(GetAttributeValueByName(restorePoint, "algoType"), out AlgorithmTypes algoType))
                        throw new BackupException("Can't recognize config file: algorithm type can't be found");

                    IAlgorithm algorithm = GetIAlgorithmByEnum(algoType);
                    backup.SetCompressionAlgorithm(algorithm);

                    if (!DateTime.TryParse(GetAttributeValueByName(restorePoint, "creationTime"), out DateTime creationTime))
                        throw new BackupException("Can't recognize config file: point creation time can't be found");

                    string pointName = GetAttributeValueByName(restorePoint, "pointName");
                    backup.CreateRestorePoint(pointName, creationTime);

                    foreach (XElement restoredFile in GetFilesFromConfigByRestorePoint(restorePoint))
                    {
                        FileSystemRestorerManager.AddArchiveEntitiesToStorage(
                            new Archive(Path.Combine(oldRepoPath, backupName, pointName, restoredFile.Value)),
                            backup.LastRestorePoint);
                    }
                }
            }

            return restoredBackupJobs;
        }

        private IReadOnlyCollection<XElement> GetBackupsFromConfigByRepo(XElement oldRepoNode)
            => oldRepoNode.Elements("backup").ToList();

        private IReadOnlyCollection<XElement> GetRestorePointsFromConfigByBackup(XElement backup)
            => backup.Elements("restore_point").ToList();

        private IReadOnlyCollection<XElement> GetFilesFromConfigByRestorePoint(XElement restorePoint)
            => restorePoint.Elements("file").ToList();

        private string GetAttributeValueByName(XElement xmlNode, string attributeName)
            => xmlNode.Attribute(attributeName)?.Value
               ?? throw new BackupException($"Can't recognize config file: attribute {attributeName} can't be found");

        private IAlgorithm GetIAlgorithmByEnum(AlgorithmTypes algorithmType) => algorithmType switch
        {
            AlgorithmTypes.SingleAlgorithm => new SingleStorage(),
            AlgorithmTypes.SplitAlgorithm => new SplitStorage(),
            _ => throw new BackupException("Unrecognized algorithm type")
        };

        private IAlgorithm GetFirstPointCompressingAlgorithm(XElement backupJob)
        {
            XElement firstRestorePoint = backupJob.Elements("restore_point").First();
            Enum.TryParse(GetAttributeValueByName(firstRestorePoint, "algoType"), out AlgorithmTypes algorithmType);

            return GetIAlgorithmByEnum(algorithmType);
        }
    }
}