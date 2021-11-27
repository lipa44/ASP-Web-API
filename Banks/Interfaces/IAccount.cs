using System;
using Banks.Entities;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        void WithdrawMoney(decimal moneyToWithdraw);
        void ChargeMoney(decimal moneyToAdd);
        void ChangeBankTerms(BankTerms newBankTerms);
        decimal GetAmountOfMoney();
        string GetAccountName();
        Guid GetBankOwnerId();
    }
}