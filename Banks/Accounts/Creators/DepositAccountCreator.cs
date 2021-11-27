using System;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts.Creators
{
    public class DepositAccountCreator : IAccountCreator
    {
        private readonly Client _client;
        private readonly decimal _firstDeposit;
        private readonly decimal _depositPercent;
        private readonly decimal _transactionLimit;
        private readonly decimal _transactionLimitForUnverified;
        private readonly DateTime _unfreezeDate;
        private readonly string _name;

        public DepositAccountCreator(Client client, Bank bank, decimal firstDeposit, DateTime unfreezeDate, string accountName)
        {
            if (client is null)
                throw new BankException("Client to create account is null", new ArgumentNullException(nameof(client)));

            if (firstDeposit <= 0)
                throw new BankException("Percent on deposit must be positive", new ArgumentOutOfRangeException(nameof(firstDeposit)));

            if (unfreezeDate <= DateTime.Today)
                throw new BankException("Unfreeze date for deposit must be later then today", new ArgumentOutOfRangeException(nameof(firstDeposit)));

            if (string.IsNullOrEmpty(accountName))
                throw new BankException("Account name is empty");

            _client = client;
            _depositPercent = bank.BankTerms.CalculatePercentByDeposit(firstDeposit);
            _transactionLimit = bank.BankTerms.TransactionLimit;
            _transactionLimitForUnverified = bank.BankTerms.TransactionLimitForUnverified;
            BankOwnerId = bank.Id;
            _firstDeposit = firstDeposit;
            _unfreezeDate = unfreezeDate;
            _name = accountName;
        }

        public Guid BankOwnerId { get; }

        public IAccount CreateAccount()
            => new DepositAccount(
                BankOwnerId,
                _client,
                _firstDeposit,
                _unfreezeDate,
                _transactionLimit,
                _transactionLimitForUnverified,
                _depositPercent,
                _name);
    }
}