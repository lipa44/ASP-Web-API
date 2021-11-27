using Banks.Interfaces;

namespace Banks.Accounts.Creators
{
    public interface IAccountCreator
    {
        public IAccount CreateAccount();
    }
}