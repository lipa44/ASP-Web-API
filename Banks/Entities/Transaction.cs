using System;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class Transaction<T> : ITransaction<T>
        where T : IAccount
    {
        private readonly T _fromAccount;
        private bool _isBeenCanceled;
        private bool _isBeenCompleted;

        public Transaction(T fromAccount, decimal moneyToTransfer)
        {
            if (fromAccount is null)
                throw new BankException("Account to transfer money from doesn't exist");

            if (moneyToTransfer <= 0)
                throw new BankException("Money to transfer must be positive");

            _fromAccount = fromAccount;
            MoneyToTransfer = moneyToTransfer;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public decimal MoneyToTransfer { get; }

        public Transaction<T> Transfer(T toAccount)
        {
            if (toAccount is null)
                throw new BankException("Account to accept money transfer doesn't exist");

            if (_isBeenCompleted)
                throw new BankException("Transaction {Id} been already completed");

            _fromAccount.WithdrawMoney(MoneyToTransfer);
            toAccount.ChargeMoney(MoneyToTransfer);
            _isBeenCompleted = true;

            return this;
        }

        public Transaction<T> Withdraw()
        {
            if (_isBeenCompleted)
                throw new BankException($"Transaction {Id} been already completed");

            _fromAccount.WithdrawMoney(MoneyToTransfer);
            _isBeenCompleted = true;

            return this;
        }

        public void Cancel()
        {
            if (_isBeenCanceled)
                throw new BankException($"Transaction {Id} been canceled earlier");

            _fromAccount.ChargeMoney(MoneyToTransfer);
            _isBeenCanceled = true;
        }
    }
}