using System;
using Backups.Tools;

namespace BackupsExtra.Tools
{
    public class DateTimeProvider
    {
        private static DateTime CurrentDate { get; set; } = DateTime.Today;

        public static void SetDataTimeToday() => CurrentDate = DateTime.Today;

        public static bool IsNeedToClearPoints(DateTime pointCreationTime, TimeSpan timeBeforeExpiring)
            => CurrentDate >= pointCreationTime + timeBeforeExpiring;

        public static void RewindTime(int daysAmount)
        {
            if (daysAmount <= 0)
                throw new BackupException("Days amount to rewind must be positive");

            CurrentDate = CurrentDate.AddDays(daysAmount);
        }
    }
}