using Banks.Entities;
using Banks.Tools;

namespace Banks.Interfaces
{
    public interface IPublisher
    {
        void AddOnUpdateSubscriber(Client client, AccountType accountType);
        void RemoveOnUpdateSubscriber(Client client, AccountType accountType);
    }
}