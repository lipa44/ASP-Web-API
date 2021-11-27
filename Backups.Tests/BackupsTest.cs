using System;
using System.Collections.Generic;
using System.IO;
using Backups.Algorithms;
using Backups.Entities;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupsTest
    {
        private static BackupJob _backupJob;
        private static List<JobObject> _jobsToRestore;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _jobsToRestore = new List<JobObject>
            {
                new ("JOBA.txt", "FilesToBackup"),
                new ("fileA.md", "FilesToBackup"),
                new ("fileB.txt", "FilesToBackup"),
            };
        }

        [SetUp]
        public void SetUp()
        {
            Directory.CreateDirectory("Repo");
            new FileSystemRepository("Repo");
            _backupJob = new BackupJob("OOP_Labs", new SplitStorage());
        }

        [Test]
        public void SplitStorageBackupTest_Success()
        {
            Directory.CreateDirectory("FilesToBackup");
            _jobsToRestore.ForEach(jobObject =>
                File.Create(Path.Combine("FilesToBackup", jobObject.FileInfo.Name)).Dispose());

            _backupJob.AddFileToBackup(_jobsToRestore[0]);
            _backupJob.AddFileToBackup(_jobsToRestore[1]);
            _backupJob.RemoveFileFromBackup(_jobsToRestore[1]);
            _backupJob.AddFileToBackup(_jobsToRestore[1]);
            
            _backupJob.CreateRestorePoint("First");

            _backupJob.AddFileToBackup(_jobsToRestore[2]);

            _backupJob.CreateRestorePoint("Second");

            Assert.IsTrue(_backupJob.RestorePoints.Count == 2);
            Assert.AreEqual(_backupJob.RestorePoints[0].StorageFiles.Count, 2);
            Assert.AreEqual(_backupJob.RestorePoints[1].StorageFiles.Count, 3);
            Assert.AreEqual(_backupJob.RestorePoints[0].StorageFiles.Count + _backupJob.RestorePoints[1].StorageFiles.Count, 5);
        }
    }
}