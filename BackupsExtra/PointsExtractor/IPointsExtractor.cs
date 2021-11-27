using Backups.Entities;

namespace BackupsExtra.PointsExtractor
{
    public interface IPointsExtractor
    {
        void ExtractPointToDirectory(RestorePoint restorePoint);
    }
}