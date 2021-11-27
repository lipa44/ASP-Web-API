using System;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Accounts.Creators
{
    public class CreditAccountCreator : IAccountCreator
    {
        private readonly Client _client;
        private readonly decimal _creditMaxLimit;
        private readonly decimal _creditMinLimit;
        private readonly decimal _commission;
        private readonly decimal _transactionLimit;
        private readonly decimal _transactionLimitForUnverified;
        private readonly string _name;

        public CreditAccountCreator(Client client, Bank bank, string accountName)
        {
            if (client is null)
                throw new BankException("Client to create account is null", new ArgumentNullException(nameof(client)));

            if (string.IsNullOrEmpty(accountName))
                throw new BankException("Account name is empty");

            _client = client;
            _creditMaxLimit = bank.BankTerms.CreditMaxLimit;
            _creditMinLimit = bank.BankTerms.CreditMinLimit;
            _commission = bank.BankTerms.CreditCommission;
            _transactionLimit = bank.BankTerms.TransactionLimit;
            _transactionLimitForUnverified = bank.BankTerms.TransactionLimitForUnverified;
            BankOwnerId = bank.Id;
            _name = accountName;
        }

        public Guid BankOwnerId { get; }

        public IAccount CreateAccount()
            => new CreditAccount(
                BankOwnerId,
                _client,
                _creditMaxLimit,
                _creditMinLimit,
                _commission,
                _transactionLimit,
                _transactionLimitForUnverified,
                _name);
    }
}