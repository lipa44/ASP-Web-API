using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank : IBank
    {
        private readonly Dictionary<Client, List<IAccount>> _clientsAndClientsAndAccounts;
        private readonly List<Transaction<IAccount>> _transactions;

        public Bank(string name, BankTerms bankTerms)
        {
            if (string.IsNullOrEmpty(name))
                throw new BankException("Bank name is empty", new ArgumentNullException(nameof(name)));

            if (bankTerms is null)
                throw new BankException("Bank terms are null", new ArgumentNullException(nameof(bankTerms)));

            Name = name;
            BankTerms = bankTerms;

            Id = Guid.NewGuid();
            _clientsAndClientsAndAccounts = new Dictionary<Client, List<IAccount>>();
            _transactions = new List<Transaction<IAccount>>();
        }

        public string Name { get; }
        public Guid Id { get; }
        public BankTerms BankTerms { get; }
        public IReadOnlyDictionary<Client, List<IAccount>> ClientsAndAccounts => _clientsAndClientsAndAccounts;

        public void RegisterAccount(Client client, IAccount account)
        {
            if (client is null)
                throw new BankException("Client to register account is null");

            if (account is null)
                throw new BankException("Account to register account is null");

            if (account.GetBankOwnerId() != Id)
                throw new BankException($"It's an account of another bank, can't register account in {Name}");

            if (IsClientExists(client))
                _clientsAndClientsAndAccounts[client].Add(account);
            else
                _clientsAndClientsAndAccounts.Add(client, new List<IAccount> { account });
        }

        public Transaction<IAccount> AddTransaction(IAccount fromAccount, decimal moneyToTransfer)
        {
            var transaction = new Transaction<IAccount>(fromAccount, moneyToTransfer);
            _transactions.Add(transaction);

            return transaction;
        }

        public void CancelTransaction(Guid transactionId) => GetTransaction(transactionId).Cancel();

        public void UpdatePercentsInAccounts(int daysAmount)
        {
            for (int i = 0; i < daysAmount; ++i)
            {
                foreach (IAccount account in GetAllBankAccounts())
                    if (account is IPercentAccount percentAccount) percentAccount.UpdatePercents();
            }
        }

        public void AccrualMonthPercentsInAccounts()
        {
            foreach (IAccount account in GetAllBankAccounts())
                if (account is IPercentAccount percentAccount) percentAccount.AccrualCommission();
        }

        public void UpdateBankTermsInAccounts()
        {
            foreach (IAccount account in GetAllBankAccounts())
                account.ChangeBankTerms(BankTerms);
        }

        public bool IsClientExists(Client client) => _clientsAndClientsAndAccounts.ContainsKey(client);

        public override string ToString() => Name;

        private IEnumerable<IAccount> GetAllBankAccounts() => _clientsAndClientsAndAccounts.Values
            .SelectMany(list => list.Select(acc => acc));

        private Transaction<IAccount> GetTransaction(Guid transactionId)
            => _transactions.SingleOrDefault(t => t.Id == transactionId)
               ?? throw new BankException("Transaction doesn't exist");
    }
}