#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank : ICentralBank
    {
        private static CentralBank? _instance;
        private readonly List<Bank> _banks;
        private readonly List<Client> _clients;

        private CentralBank()
        {
            _banks = new List<Bank>();
            _clients = new List<Client>();
        }

        public IReadOnlyList<Bank> Banks => _banks;
        public IReadOnlyList<Client> Clients => _clients;

        public static CentralBank GetInstance() => _instance is null
            ? _instance = new CentralBank()
            : throw new BankException("Central bank can be created only once");

        public Bank RegisterBank(string name, BankTerms bankTerms)
        {
            var bank = new Bank(name, bankTerms);

            if (IsBankExist(bank))
                throw new BankException($"Bank {bank} already exists");

            _banks.Add(bank);
            return bank;
        }

        public Client RegisterClient(string name, string surname, Guid passportId)
        {
            var client = new Client(name, surname, passportId);

            if (IsClientExist(client))
                throw new BankException($"Client {client} already exists");

            _clients.Add(client);
            return client;
        }

        public void TryUpdateAllPercentAccounts()
        {
            if (!DateTimeProvider.IsTimeToUpdatePercents())
                throw new BankException("Time to update percents hasn't come yet");

            int monthsAmount = DateTimeProvider.AmountOfMonthToRewind();

            foreach (Bank bank in _banks)
            {
                for (int i = 0; i < monthsAmount; ++i)
                {
                    double daysBetweenTwoMonths =
                        DateTimeProvider.CalculateDaysBetweenMonthsFromLastUpdateTime(i + 1, i);

                    int daysToRewind = (int)Math.Floor(daysBetweenTwoMonths);
                    bank.UpdatePercentsInAccounts(daysToRewind);
                    bank.AccrualMonthPercentsInAccounts();
                }
            }

            DateTimeProvider.SetLastUpdateTime();
        }

        public Bank GetBank(string bankName) => Banks.SingleOrDefault(b => b.Name == bankName) ??
                                                throw new BankException("Bank doesn't exist");

        public Client GetClient(Guid passportId) => Clients.SingleOrDefault(c => c.PassportId == passportId) ??
                                                    throw new BankException("Client doesn't exist");

        private bool IsBankExist(Bank bank) => _banks.Any(b => b.Name == bank.Name);
        private bool IsClientExist(Client client) => _clients.Any(c => c.PassportId == client.PassportId);
    }
}