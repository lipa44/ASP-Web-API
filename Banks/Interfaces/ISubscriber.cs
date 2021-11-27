using System.Collections.Generic;

namespace Banks.Interfaces
{
    public interface ISubscriber
    {
        void AddBankParametersUpdates(string update);
        List<string> GetBankParametersUpdates();
    }
}