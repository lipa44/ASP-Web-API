using System;
using Banks.Entities;

namespace Banks.SpectreConsole.TransactionCommands.TransactionSettings
{
    public class WithdrawMoneySettings : TransactionCommandsSettings
    {
        public Guid ClientId { get; set; }
        public Client AccountOwner { get; set; }
    }
}