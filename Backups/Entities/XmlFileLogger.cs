using System;
using System.IO;
using System.Xml.Linq;
using Backups.Algorithms;
using Backups.Tools;

namespace Backups.Entities
{
    public class XmlFileLogger
    {
        private static readonly XDocument XmlDocument = new ();
        private static XmlFileLogger? _instance;
        private readonly DirectoryInfo _xmlDocPath;

        protected XmlFileLogger(string xmlFilePath)
        {
            if (string.IsNullOrWhiteSpace(xmlFilePath))
                throw new BackupException("Directory to create xml logger is null", new ArgumentNullException(nameof(xmlFilePath)));

            _xmlDocPath = new DirectoryInfo(xmlFilePath);
        }

        public static XmlFileLogger CreateInstance(string xmlFilePath) => _instance is null
            ? _instance = new XmlFileLogger(xmlFilePath)
            : throw new BackupException("Logger can be created only once");

        public static void LogRepo(string repoPath)
        {
            var repositoryElem = new XElement("repository");

            repositoryElem.Add(new XAttribute("repoPath", repoPath));
            XmlDocument.Add(repositoryElem);
        }

        public static void LogBackup(string backupName)
        {
            var backupElem = new XElement("backup");

            backupElem.Add(new XAttribute("backupName", backupName));
            XmlAlgorithms.GetRepoInXml(XmlDocument).Add(backupElem);
        }

        public static void LogRestorePoint(RestorePoint restorePoint, string backupName)
        {
            var pointElem = new XElement("restore_point");

            pointElem.Add(new XAttribute("pointName", $"{restorePoint.Name}"));
            pointElem.Add(new XAttribute("algoType", $"{restorePoint.AlgorithmType}"));
            pointElem.Add(new XAttribute("creationTime", $"{restorePoint.CreationTime}"));

            XmlAlgorithms.GetBackupInXml(XmlDocument, backupName).Add(pointElem);
        }

        public static void LogFile(Storage storage, string backupName, string pointName)
        {
            var fileElem = new XElement("file", storage.FullName + ".zip");

            XElement backup = XmlAlgorithms.GetBackupInXml(XmlDocument, backupName);
            XElement restorePoint = XmlAlgorithms.GetRestorePointInXml(backup, pointName);

            restorePoint.Add(fileElem);
        }

        public void Save() => XmlDocument.Save(_xmlDocPath.FullName);
    }
}