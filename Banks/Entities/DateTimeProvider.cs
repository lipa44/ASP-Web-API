using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class DateTimeProvider
    {
        private static DateTime CentralBankTime { get; set; } = DateTime.Today;
        private static DateTime LastUpdateTime { get; set; } = DateTime.Today;

        public static void SetDataTimeToday()
        {
            CentralBankTime = DateTime.Today;
            LastUpdateTime = DateTime.Today;
        }

        public static void RewindTime(int monthsAmount)
        {
            if (monthsAmount <= 0)
                throw new BankException("Months amount to rewind must be positive");

            CentralBankTime = CentralBankTime.AddMonths(monthsAmount);
        }

        public static void SetLastUpdateTime() => LastUpdateTime = CentralBankTime.Date;
        public static bool IsTimeToUpdatePercents() => CentralBankTime >= LastUpdateTime.AddMonths(1);
        public static bool IsAbleToWithdraw(DateTime unfreezeDate) => CentralBankTime >= unfreezeDate;
        public static int AmountOfMonthToRewind() => (int)Math.Round((CentralBankTime - LastUpdateTime).TotalDays / 31);

        public static int CalculateDaysBetweenMonthsFromLastUpdateTime(int nextMonth, int pervMonth) =>
            (int)(LastUpdateTime.AddMonths(nextMonth) - LastUpdateTime.AddMonths(pervMonth)).TotalDays;
    }
}