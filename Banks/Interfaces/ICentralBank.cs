using System;
using Banks.Entities;

namespace Banks.Interfaces
{
    public interface ICentralBank
    {
        Bank RegisterBank(string name, BankTerms bankTerms);
        Client RegisterClient(string name, string surname, Guid passportId);
        void TryUpdateAllPercentAccounts();
        Bank GetBank(string bankName);
        Client GetClient(Guid passportId);
    }
}