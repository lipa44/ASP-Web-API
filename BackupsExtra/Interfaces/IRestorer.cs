using Backups.Interfaces;

namespace BackupsExtra.Interfaces
{
    public interface IRestorer
    {
        IRepository RestoreRepository();
    }
}