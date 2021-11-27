using System;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts
{
    internal class DebitAccount : Account, IPercentAccount
    {
        private decimal _monthCommission;

        public DebitAccount(
            Guid bankId,
            Client client,
            decimal percent,
            decimal transactionLimit,
            decimal transactionLimitForUnverified,
            string name)
            : base(bankId, client, transactionLimit, transactionLimitForUnverified, 0, name)
        {
            Percent = percent;
            TransactionLimit = transactionLimit;
        }

        public decimal Percent { get; private set; }

        public override void WithdrawMoney(decimal moneyToWithdraw)
        {
            if (moneyToWithdraw < 0)
                throw new BankException("Money to withdraw must be positive", new ArgumentOutOfRangeException(nameof(moneyToWithdraw)));

            if (!IsEnoughMoneyToWithdraw(moneyToWithdraw))
                throw new BankException("Not enough money to withdraw");

            IsClientAbleToWithdrawSumOrException(moneyToWithdraw);

            Money -= moneyToWithdraw;
        }

        public override void ChargeMoney(decimal moneyToAdd)
        {
            if (moneyToAdd < 0)
                throw new BankException("Money to withdraw must be positive", new ArgumentOutOfRangeException(nameof(moneyToAdd)));

            Money += moneyToAdd;
        }

        public override void ChangeBankTerms(BankTerms newBankTerms)
        {
            Percent = newBankTerms.DebitPercent;
            TransactionLimit = newBankTerms.TransactionLimit;
            TransactionLimitForUnverified = newBankTerms.TransactionLimitForUnverified;
        }

        public void UpdatePercents() => _monthCommission += Money * (Percent / 100) / 365;

        public void AccrualCommission()
        {
            ChargeMoney(_monthCommission);
            _monthCommission = 0;
        }

        private bool IsEnoughMoneyToWithdraw(decimal moneyToWithdraw) => Money - moneyToWithdraw >= 0;
    }
}