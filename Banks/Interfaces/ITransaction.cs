using Banks.Entities;

namespace Banks.Interfaces
{
    public interface ITransaction<T>
        where T : IAccount
    {
        Transaction<T> Transfer(T toAccount);
        Transaction<T> Withdraw();
        void Cancel();
    }
}