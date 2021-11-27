using System;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.SpectreConsole.TransactionCommands.TransactionSettings
{
    public class TransferMoneySettings : TransactionCommandsSettings
    {
        public Guid ClientId { get; set; }
        public Client AccountOwner { get; set; }
        public IAccount SenderAccount { get; set; }
    }
}