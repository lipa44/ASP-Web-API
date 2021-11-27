using System;
using Banks.Entities;

namespace Banks.Interfaces
{
    public interface IBank
    {
        void RegisterAccount(Client client, IAccount account);
        Transaction<IAccount> AddTransaction(IAccount fromAccount, decimal moneyToTransfer);
        void CancelTransaction(Guid transactionId);
        public void UpdatePercentsInAccounts(int daysAmount);
        public void AccrualMonthPercentsInAccounts();
        bool IsClientExists(Client client);
    }
}