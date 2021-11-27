using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Backups.Tools;

namespace Backups.Algorithms
{
    public static class XmlAlgorithms
    {
        public static XElement GetBackupInXml(XDocument xmlDocument, string backupName)
            => GetRepoInXml(xmlDocument).XPathSelectElements("./backup")
                   .FirstOrDefault(backup => backup.FirstAttribute?.Value == backupName)
               ?? throw new BackupException("Backup doesn't exist");

        public static XElement GetRestorePointInXml(XElement backup, string pointName)
            => backup.XPathSelectElements("./restore_point")
                   .Single(p => p.FirstAttribute?.Value == pointName)
               ?? throw new BackupException("Restore point doesn't exist!");

        public static XElement GetRepoInXml(XDocument xmlDocument)
            => xmlDocument.XPathSelectElement("repository")
               ?? throw new BackupException("Repo doesn't exist");
    }
}