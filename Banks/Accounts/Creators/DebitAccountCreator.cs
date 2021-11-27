using System;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts.Creators
{
    public class DebitAccountCreator : IAccountCreator
    {
        private readonly Client _client;
        private readonly decimal _debitPercent;
        private readonly decimal _transactionLimit;
        private readonly decimal _transactionLimitForUnverified;
        private readonly string _name;

        public DebitAccountCreator(Client client, Bank bank, string accountName)
        {
            if (client is null)
                throw new BankException("Client to create account is null", new ArgumentNullException(nameof(client)));

            if (string.IsNullOrEmpty(accountName))
                throw new BankException("Account name is empty");

            _client = client;
            _debitPercent = bank.BankTerms.DebitPercent;
            _transactionLimit = bank.BankTerms.TransactionLimit;
            _transactionLimitForUnverified = bank.BankTerms.TransactionLimitForUnverified;
            BankOwnerId = bank.Id;
            _name = accountName;
        }

        public Guid BankOwnerId { get; }

        public IAccount CreateAccount()
            => new DebitAccount(
                BankOwnerId,
                _client,
                _debitPercent,
                _transactionLimit,
                _transactionLimitForUnverified,
                _name);
    }
}