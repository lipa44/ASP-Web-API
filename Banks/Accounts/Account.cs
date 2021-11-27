using System;
using System.Collections.Generic;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts
{
    public abstract class Account : IAccount
    {
        protected Account(Guid bankId, Client client, decimal transactionLimit, decimal transactionLimitForUnverified, decimal money, string name)
        {
            TransactionLimit = transactionLimit;
            TransactionLimitForUnverified = transactionLimitForUnverified;
            Money = money;
            BankId = bankId;
            Client = client;
            Name = name;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        protected decimal TransactionLimit { get; set; }
        protected decimal TransactionLimitForUnverified { get; set; }
        protected decimal Money { get; set; }
        protected Guid BankId { get; }
        protected Client Client { get; }
        protected string Name { get; }

        public abstract void WithdrawMoney(decimal moneyToWithdraw);
        public abstract void ChargeMoney(decimal moneyToAdd);
        public abstract void ChangeBankTerms(BankTerms newBankTerms);

        public decimal GetAmountOfMoney() => Money;
        public string GetAccountName() => Name;
        public Guid GetBankOwnerId() => BankId;

        public override string ToString() => Name;

        protected void IsClientAbleToWithdrawSumOrException(decimal moneyToWithdraw)
        {
            if (Client.IsVerified())
            {
                if (IsGreaterThenTransactionLimit(moneyToWithdraw))
                    throw new BankException($"Transaction limit is {TransactionLimit:C}");
            }
            else
            {
                if (IsGreaterThenTransactionLimitForUnverified(moneyToWithdraw))
                    throw new BankException($"Transaction limit for unverified clients is {TransactionLimitForUnverified:C}");
            }
        }

        private bool IsGreaterThenTransactionLimit(decimal moneyToWithdraw) => moneyToWithdraw > TransactionLimit;
        private bool IsGreaterThenTransactionLimitForUnverified(decimal moneyToWithdraw)
            => moneyToWithdraw > TransactionLimitForUnverified;
    }
}