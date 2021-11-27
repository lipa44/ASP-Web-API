using System;
using System.IO;
using Backups.Algorithms;
using Backups.Entities;
using NUnit.Framework;

namespace Backups
{
    internal class Program
    {
        private static readonly string? Pwd = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        private static readonly string LoggerPath = Path.Combine(Pwd!, "restore.xml");

        private static void Main()
        {
            var logger = XmlFileLogger.CreateInstance(LoggerPath);
            var repo = new FileSystemRepository(Path.Combine(Pwd!, "Repo"));

            var backupJob = new BackupJob("OOP_Labs", new SingleStorage());

            var fileA = new JobObject("fileA.md", Path.Combine(Pwd!, "FilesToBackup"));
            var fileB = new JobObject("fileB.txt", Path.Combine(Pwd!, "FilesToBackup"));
            var fileC = new JobObject("fileC.rtf", Path.Combine(Pwd!, "FilesToBackup"));

            backupJob.AddFileToBackup(fileA);
            backupJob.AddFileToBackup(fileB);
            backupJob.RemoveFileFromBackup(fileB);
            backupJob.CreateRestorePoint("Some files");

            repo.Save(backupJob);

            backupJob.AddFileToBackup(fileB);
            backupJob.CreateRestorePoint("Work");

            repo.Save(backupJob);

            backupJob.SetCompressionAlgorithm(new SplitStorage());

            backupJob.AddFileToBackup(fileC);
            backupJob.CreateRestorePoint("Studying");

            repo.Save(backupJob);
            logger.Save();

            string restoreDir = Path.Combine(repo.RepoDirectory.FullName, backupJob.Name);

            Assert.AreEqual(3, backupJob.RestorePoints.Count);
            Assert.IsTrue(Directory.Exists(restoreDir));

            // Assert.IsTrue(File.Exists(Path.Combine(restoreDir, "RestorePoint_1 - SingleAlgorithm", "RestorePoint_1 - SingleAlgorithm.zip")));
        }
    }
}