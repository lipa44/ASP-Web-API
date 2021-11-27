#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client : ISubscriber
    {
        private readonly List<string> _updates;

        public Client(string name, string surname, Guid passportId)
        {
            if (string.IsNullOrEmpty(name))
                throw new BankException("Name to create client is empty", new ArgumentNullException(nameof(name)));

            if (string.IsNullOrEmpty(surname))
                throw new BankException("Surname to create client is empty", new ArgumentNullException(nameof(surname)));

            if (passportId == Guid.Empty)
                throw new BankException("Passport Id to create client is empty", new ArgumentNullException(nameof(passportId)));

            Name = name;
            Surname = surname;
            PassportId = passportId;

            _updates = new List<string>();
        }

        public string Name { get; }
        public string Surname { get; }
        public Guid PassportId { get; }
        public string? Address { get; private set; }
        public string? PhoneNumber { get; private set; }

        public Client SetPhoneNumber(string phoneNumber)
        {
            if (!new Regex(@"^[7|8]{1}\(?\d{3}\)?-? *\d{3}-? *-?\d{4}$").IsMatch(phoneNumber))
                throw new BankException("Phone number is invalid");

            PhoneNumber = phoneNumber;
            return this;
        }

        public Client SetAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new BankException("Address is invalid");

            Address = address;
            return this;
        }

        public void AddBankParametersUpdates(string update) => _updates.Add(update);

        public bool IsVerified() => !string.IsNullOrEmpty(Address) && PhoneNumber is not null;

        public List<string> GetBankParametersUpdates()
        {
            var updates = _updates.ToList();
            _updates.Clear();

            return updates;
        }

        public override string ToString() => $"{Name} {Surname}";
        public override int GetHashCode() => HashCode.Combine(Name, Surname, PassportId);
    }
}