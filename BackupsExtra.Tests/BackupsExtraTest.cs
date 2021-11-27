using System;
using System.Collections.Generic;
using System.IO;
using Backups.Algorithms;
using Backups.Entities;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.ConflictsResolverState;
using BackupsExtra.Entities;
using BackupsExtra.Interfaces;
using BackupsExtra.Tools;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    [TestFixture]
    public class BackupsExtraTest
    {
        private List<JobObject> _jobsToRestore;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XmlFileLogger.CreateInstance("LoggerPath");
            new FileSystemRepository("TestRepo");

            _jobsToRestore = new List<JobObject>
            {
                new ("JOBA.txt", "FilesToBackup"),
                new ("fileA.md", "FilesToBackup"),
                new ("fileB.txt", "FilesToBackup"),
                new ("fileC.rtf", "FilesToBackup"),
            };
        }

        [SetUp]
        public void SetUp() => DateTimeProvider.SetDataTimeToday();

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void PointsLimitCleaningAlgorithm_PointsDeletedAfterOverflow(int maxAmountOfPoints)
        {
            var extraJob = new ExtraBackupJob(
                "OOP",
                new SoftConflictsResolverState(),
                new BackupJobSettings(new PointsLimitAlgorithm(maxAmountOfPoints),
                    new SplitStorage()));

            Directory.CreateDirectory("FilesToBackup");
            _jobsToRestore.ForEach(jobObject =>
                File.Create(Path.Combine("FilesToBackup", jobObject.FileInfo.Name)).Dispose());

            extraJob.AddFileToBackup(_jobsToRestore[0]);
            extraJob.SafetyCreateRestorePoint("Point1");

            extraJob.AddFileToBackup(_jobsToRestore[1]);
            extraJob.SafetyCreateRestorePoint("Point2");

            extraJob.AddFileToBackup(_jobsToRestore[2]);
            extraJob.SafetyCreateRestorePoint("Point3");

            extraJob.AddFileToBackup(_jobsToRestore[3]);
            extraJob.SafetyCreateRestorePoint("Point4");

            Assert.AreEqual(maxAmountOfPoints, extraJob.RestorePoints.Count);
        }

        [Test]
        [TestCase(5, 5, 10, 15, 16, 1)]
        [TestCase(10, 2, 4, 22, 13, 2)]
        [TestCase(20, 2, 4, 22, 13, 3)]
        public void DateLimitCleaningAlgorithm_PointsDeletedAfterExpiringDate(
            int daysBeforeExpiring,
            int firstPointTime,
            int secondPointTime,
            int thirdPointTime,
            int daysToRewind,
            int pointsSaved)
        {
            var extraJob = new ExtraBackupJob(
                "OOP",
                new SoftConflictsResolverState(),
                new BackupJobSettings(
                    new DateLimitAlgorithm(TimeSpan.FromDays(daysBeforeExpiring)),
                    new SplitStorage()));

            Directory.CreateDirectory("FilesToBackup");
            _jobsToRestore.ForEach(jobObject =>
                File.Create(Path.Combine("FilesToBackup", jobObject.FileInfo.Name)).Dispose());

            extraJob.AddFileToBackup(_jobsToRestore[0]);
            extraJob.SafetyCreateRestorePoint("Point1", DateTime.Today.AddDays(firstPointTime));

            extraJob.AddFileToBackup(_jobsToRestore[1]);
            extraJob.SafetyCreateRestorePoint("Point2", DateTime.Today.AddDays(secondPointTime));

            extraJob.AddFileToBackup(_jobsToRestore[2]);
            extraJob.SafetyCreateRestorePoint("Point3", DateTime.Today.AddDays(thirdPointTime));

            DateTimeProvider.RewindTime(daysToRewind);

            extraJob.CleanRestorePoints();

            Assert.AreEqual(pointsSaved, extraJob.RestorePoints.Count);
        }

        [Test]
        public void HybridLimitCleaningAlgorithm_PointsDeletedAfterOverflow()
        {
            var extraJob = new ExtraBackupJob(
                "OOP",
                new SoftConflictsResolverState(),
                new BackupJobSettings(
                    new HybridAlgorithm(new List<ICleaningAlgorithm>
                    {
                        new DateLimitAlgorithm(TimeSpan.FromDays(10)),
                        new PointsLimitAlgorithm(3)
                    }, PointsCleaningRule.AllRule),
                    new SplitStorage()));

            Directory.CreateDirectory("FilesToBackup");
            _jobsToRestore.ForEach(jobObject =>
                File.Create(Path.Combine("FilesToBackup", jobObject.FileInfo.Name)).Dispose());

            extraJob.AddFileToBackup(_jobsToRestore[0]);
            extraJob.SafetyCreateRestorePoint("Point1", DateTime.Today.AddDays(10));

            extraJob.AddFileToBackup(_jobsToRestore[1]);
            extraJob.SafetyCreateRestorePoint("Point2", DateTime.Today.AddDays(11));

            extraJob.AddFileToBackup(_jobsToRestore[2]);
            extraJob.SafetyCreateRestorePoint("Point3", DateTime.Today.AddDays(12));

            DateTimeProvider.RewindTime(25);
            
            extraJob.AddFileToBackup(_jobsToRestore[3]);
            extraJob.SafetyCreateRestorePoint("Point4", DateTime.Today.AddDays(40));

            Assert.AreEqual(3, extraJob.RestorePoints.Count);
        }
    }
}