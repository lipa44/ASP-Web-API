using System.Xml.Linq;

namespace Backups.Interfaces
{
    public interface IXmlAlgorithms
    {
        XElement GetRestorePointInXml(XElement backup, string pointName);
        XElement GetBackupInXml(XDocument xmlDocument, string backupName);
        XElement GetRepoInXml(XDocument xmlDocument);
    }
}