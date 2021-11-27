using Banks.Entities;

namespace Banks.SpectreConsole.TransactionCommands.TransactionSettings
{
    public class AddMoneySettings : TransactionCommandsSettings
    {
        public Client AccountOwner { get; set; }
    }
}