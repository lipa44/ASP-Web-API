using System;
using Banks.Entities;
using Banks.Tools;

namespace Banks.Accounts
{
    internal class CreditAccount : Account
    {
        public CreditAccount(Guid bankId, Client client, decimal creditMaxLimit, decimal creditMinLimit, decimal commission, decimal transactionLimit, decimal transactionLimitForUnverified, string name)
            : base(bankId, client, transactionLimit, transactionLimitForUnverified, creditMaxLimit, name)
        {
            CreditMinLimit = creditMinLimit;
            TransactionLimit = transactionLimit;
            Commission = commission;
        }

        public decimal CreditMinLimit { get; private set; }
        public decimal Commission { get; private set; }

        public override void WithdrawMoney(decimal moneyToWithdraw)
        {
            if (moneyToWithdraw < 0)
                throw new BankException("Money to withdraw must be positive", new ArgumentOutOfRangeException(nameof(moneyToWithdraw)));

            if (!IsEnoughMoneyToWithdraw(moneyToWithdraw))
                throw new BankException("Not enough money to withdraw");

            IsClientAbleToWithdrawSumOrException(moneyToWithdraw);

            Money -= Money - moneyToWithdraw >= 0 ? moneyToWithdraw : moneyToWithdraw + Commission;
        }

        public override void ChargeMoney(decimal moneyToAdd)
        {
            if (moneyToAdd < 0)
                throw new BankException("Money to withdraw must be positive", new ArgumentOutOfRangeException(nameof(moneyToAdd)));

            if (Money < 0 && moneyToAdd < Commission)
                throw new BankException("Commission for opposite amount of money is larger then amount money to add");

            Money += Money >= 0 ? moneyToAdd : moneyToAdd - Commission;
        }

        public override void ChangeBankTerms(BankTerms newBankTerms)
        {
            CreditMinLimit = newBankTerms.CreditMinLimit;
            TransactionLimit = newBankTerms.TransactionLimit;
            TransactionLimitForUnverified = newBankTerms.TransactionLimitForUnverified;
            Commission = newBankTerms.CreditCommission;
        }

        private bool IsEnoughMoneyToWithdraw(decimal moneyToWithdraw) => Money - moneyToWithdraw >= CreditMinLimit;
    }
}