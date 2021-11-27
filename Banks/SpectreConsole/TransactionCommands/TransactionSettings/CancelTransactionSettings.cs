using System;
using Banks.Entities;

namespace Banks.SpectreConsole.TransactionCommands.TransactionSettings
{
    public class CancelTransactionSettings : TransactionCommandsSettings
    {
        public string BankName { get; set; }
        public Guid ClientId { get; set; }
        public Guid TransactionId { get; set; }
        public Client AccountOwner { get; set; }
    }
}