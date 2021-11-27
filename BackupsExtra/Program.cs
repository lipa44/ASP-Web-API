using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Interfaces;
using BackupsExtra.BackupsLogger;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.ConflictsResolverState;
using BackupsExtra.Entities;
using BackupsExtra.PointsExtractor;
using BackupsExtra.Restorer;
using Logger = BackupsExtra.BackupsLogger.BackupsLogger;

namespace BackupsExtra
{
    internal class Program
    {
        private static readonly DirectoryInfo RuntimeDir = Directory.GetParent(Environment.CurrentDirectory);
        private static readonly string Pwd = RuntimeDir?.Parent?.Parent?.FullName!;
        private static readonly string BackupsPath = RuntimeDir?.Parent?.Parent?.Parent?.FullName!;

        private static void Main()
        {
            var logger = XmlFileLogger.CreateInstance(Path.Combine(Pwd, "newRestore.xml"));

            Serilog.Core.Logger loggerConfig = new FileLoggerBuilder()
                .SetFilePath(new FileInfo(Path.Combine(Pwd, "file.log")))
                .SetIsWithTime("HH:MM:ss:ffffff")
                .SetLevelSymbolsAmount(3)
                .CreateInstance();

            Logger.SetInstance(loggerConfig);

            var configFilePath = new FileInfo(Path.Combine(BackupsPath, "Backups", "restore.xml"));
            var newRepoPath = new FileInfo(Path.Combine(Pwd, "NewRepo"));
            var repo = new FileSystemRepository(newRepoPath.FullName);

            var recover = new FileSystemRestorer(configFilePath);
            IReadOnlyCollection<BackupJob> restoredBackups = recover.RestoreRepository();

            restoredBackups.ToList().ForEach(backupJob => repo.AddBackupJob(backupJob));
            repo.BackupJobs.ToList().ForEach(b => repo.Save(b));

            var file1 = new JobObject("JOBA.txt", Path.Combine(Pwd, "NewFilesToBackup"));

            var extraBackup = new ExtraBackupJob(
                "OOP",
                new SoftConflictsResolverState(),
                new BackupJobSettings(
                    new PointsLimitAlgorithm(5),
                    new SplitStorage()));

            var file2 = new JobObject("fileA.md", Path.Combine(BackupsPath, "Backups", "FilesToBackup"));
            var file3 = new JobObject("fileB.txt", Path.Combine(BackupsPath, "Backups", "FilesToBackup"));

            extraBackup.AddFileToBackup(file1);
            extraBackup.AddFileToBackup(file2);
            extraBackup.SafetyCreateRestorePoint("Aboba");

            var file4 = new JobObject("fileC.rtf", Path.Combine(BackupsPath, "Backups", "FilesToBackup"));
            extraBackup.AddFileToBackup(file4);

            extraBackup.SafetyCreateRestorePoint("AAAA");

            extraBackup.AddFileToBackup(file3);
            RestorePoint point3 = extraBackup.LastRestorePoint;

            extraBackup.SafetyCreateRestorePoint("BBBB");
            RestorePoint point4 = extraBackup.LastRestorePoint;

            extraBackup.MergePoints(point3, point4);
            repo.AddBackupJob(extraBackup);

            repo.Save(extraBackup);
            RestorePoint third = extraBackup.RestorePoints[1];

            var extractor = new FileSystemPointsExtractor(
                repo,
                new DirectoryInfo(Path.Combine(Pwd, "ExtractedPoint")));

            extractor.ExtractPointToDirectory(third);
            logger.Save();
        }
    }
}