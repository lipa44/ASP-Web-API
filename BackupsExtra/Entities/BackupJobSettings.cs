using Backups.Interfaces;
using Backups.Tools;
using BackupsExtra.Interfaces;

namespace BackupsExtra.Entities
{
    public class BackupJobSettings
    {
        public BackupJobSettings(ICleaningAlgorithm cleaningAlgorithm, IAlgorithm compressingAlgorithm)
        {
            if (cleaningAlgorithm is null)
                throw new BackupException("Cleaning algorithm to create backup settings is null");

            if (compressingAlgorithm is null)
                throw new BackupException("Compressing algorithm to create backup settings is null");

            CleaningAlgorithm = cleaningAlgorithm;
            CompressingAlgorithm = compressingAlgorithm;
        }

        public ICleaningAlgorithm CleaningAlgorithm { get; private set; }
        public IAlgorithm CompressingAlgorithm { get; private set; }

        public void SetCleaningAlgorithm(ICleaningAlgorithm cleaningAlgorithm) =>
            CleaningAlgorithm = cleaningAlgorithm ??
                                               throw new BackupException("Cleaning algorithm to set is null");
    }
}