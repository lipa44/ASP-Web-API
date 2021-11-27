using System;
using System.Collections.Generic;
using System.ComponentModel;
using Spectre.Console.Cli;

namespace Banks.SpectreConsole.CreateCommands.CreateSettings
{
    public class CreateBankSettings : CreateCommandsSettings
    {
        [CommandArgument(0, "<BANK_NAME>")]
        [Description("Create a brand new bank.")]
        public string BankName { get; init; }
        public decimal DebitPercent { get; set; }
        public decimal CreditCommission { get; set; }
        public decimal CreditMaxLimit { get; set; }
        public decimal CreditMinLimit { get; set; }
        public decimal TransactionLimit { get; set; }
        public decimal TransactionLimitForUnverified { get; set; }
        public List<(Range, decimal)> DepositsAndPercents { get; set; }
    }
}