using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts.Creators;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private CentralBank _cb;
        private Bank _bank;
        private BankTerms _bankTerms;

        [SetUp]
        public void SetUp()
        {
            DateTimeProvider.SetDataTimeToday();

            if (_cb is not null) return;

            var depositAndPercents = new List<(Range, decimal)>
            {
                (new Range(0, 50000), 5.3m),
                (new Range(50000, 100000), 6.88m),
            };

            _bankTerms = new BankTerms(
                10,
                5,
                10000000,
                -10000,
                50000,
                25000,
                depositAndPercents);

            _cb = CentralBank.GetInstance();
            _bank = _cb?.RegisterBank("Test Bank", _bankTerms);
        }

        [Test, Order(1)]
        public void CreatingEntitiesMethodsValidation_ExceptionsThrown()
        {
            var depositAndPercents = new List<(Range, decimal)>
            {
                (new Range(0, 50000), 5.3m),
                (new Range(50000, 100000), 6.88m),
            };

            _bankTerms.SetFirstDepositAndPercents(depositAndPercents);
            BankTerms bankTerms1 = _bankTerms;

            Assert.Catch<BankException>(() => _cb.RegisterBank("Test Bank", _bankTerms));
            Assert.AreEqual(1, _cb.Banks.Count);

            var clientPassportId = Guid.NewGuid();
            Client client = _cb.RegisterClient("Iskander", "Kudashev", clientPassportId)
                .SetAddress("Pushkina 44")
                .SetPhoneNumber("89999999999");

            var debitAccountCreator = new DebitAccountCreator(client, _bank, "Test debit account");
            IAccount debitAccount = debitAccountCreator.CreateAccount();

            _bank.RegisterAccount(client, debitAccount);

            var depositAccountCreator = new DepositAccountCreator(
                client,
                _bank,
                10000,
                DateTime.Today.AddMonths(1),
                "Test deposit account");

            IAccount depositAccount = depositAccountCreator.CreateAccount();

            _bank.RegisterAccount(client, depositAccount);

            var creditAccountCreator = new CreditAccountCreator(client, _bank, "Test credit account");
            IAccount creditAccount = creditAccountCreator.CreateAccount();

            _bank.RegisterAccount(client, creditAccount);

            Assert.AreEqual(1, _bank.ClientsAndAccounts.Count);
            Assert.AreEqual(3, _bank.ClientsAndAccounts[client].Count);
            Assert.Contains(debitAccount, _bank.ClientsAndAccounts[client]);
            Assert.Contains(depositAccount, _bank.ClientsAndAccounts[client]);
            Assert.Contains(creditAccount, _bank.ClientsAndAccounts[client]);

            Bank bank1 = _cb.RegisterBank("Black daddy's bank", bankTerms1);

            var accountCreatorWithForeignBankTerms = new DebitAccountCreator(client, _bank, "Test credit account");
            IAccount accountWithForeignBankTerms = accountCreatorWithForeignBankTerms.CreateAccount();

            Assert.Catch<BankException>(() => bank1.RegisterAccount(client, accountWithForeignBankTerms));
            Assert.IsFalse(bank1.IsClientExists(client));
            Assert.AreEqual(0, bank1.ClientsAndAccounts.Keys.Count());

            var accountCreatorWithRightBankTerms = new DebitAccountCreator(client, bank1, "Test credit account");
            IAccount accountWithRightBankTerms = accountCreatorWithRightBankTerms.CreateAccount();
            
            bank1.RegisterAccount(client, accountWithRightBankTerms);
            Assert.IsTrue(bank1.IsClientExists(client));
            Assert.AreEqual(1, bank1.ClientsAndAccounts.Keys.Count());
            Assert.AreEqual(1, bank1.ClientsAndAccounts[client].Count);
        }

        [Test, Order(2)]
        public void TransactionMethodsValidation_ExceptionsThrown()
        {
            var clientPassportId = Guid.NewGuid();
            Client client = _cb.RegisterClient("Misha", "Libchenko", clientPassportId)
                .SetAddress("Pushkina st., Kolotushkina h.")
                .SetPhoneNumber("89999998888");
            
            var depositAndPercents = new List<(Range, decimal)>
            {
                (new Range(0, 50000), 5.3m),
                (new Range(50000, 100000), 6.88m),
            };

            _bankTerms.SetFirstDepositAndPercents(depositAndPercents);

            BankTerms bankTerms = _bankTerms;
            
            Bank bank = _cb.RegisterBank("My bank", bankTerms);

            var debitAccountCreator = new DebitAccountCreator(client, bank, "My debit account");
            IAccount debitAccount = debitAccountCreator.CreateAccount();

            bank.RegisterAccount(client, debitAccount);

            int moneyToWithdraw = 5000;
            Assert.Catch<BankException>(() => debitAccount.WithdrawMoney(moneyToWithdraw));
            debitAccount.ChargeMoney(5000);
            debitAccount.WithdrawMoney(moneyToWithdraw);

            decimal firstDeposit = 322;
            var depositAccountCreator = new DepositAccountCreator(
                client,
                bank,
                firstDeposit,
                DateTime.Today.AddMonths(2),
                "My deposit account");

            IAccount depositAccount = depositAccountCreator.CreateAccount();

            bank.RegisterAccount(client, depositAccount);

            var creditAccountCreator = new CreditAccountCreator(client, bank, "My credit account");
            IAccount creditAccount = creditAccountCreator.CreateAccount();

            bank.RegisterAccount(client, creditAccount);

            int moneyToAdd = 5000;
            depositAccount.ChargeMoney(moneyToAdd);
            Assert.AreEqual(firstDeposit + moneyToAdd, depositAccount.GetAmountOfMoney());

            DateTimeProvider.RewindTime(2);
            _cb.TryUpdateAllPercentAccounts();

            Transaction<IAccount> transaction =
                bank.AddTransaction(depositAccount, depositAccount.GetAmountOfMoney()).Withdraw();

            Assert.AreEqual(0, depositAccount.GetAmountOfMoney());
            
            bank.CancelTransaction(transaction.Id);
        }

        [Test, Order(3)]
        public void SubscriptionMethodsValidation_Success()
        {
            var clientPassportId = Guid.NewGuid();
            Client client = _cb.RegisterClient("NeMisha", "NeLibchenko", clientPassportId)
                .SetAddress("Pushkina st., Kolotushkina h.")
                .SetPhoneNumber("79999997777");

            _bank.BankTerms.AddOnUpdateSubscriber(client, AccountType.CreditAccount);

            _bankTerms.SetCreditCommission(123123);

            Assert.AreEqual(1, client.GetBankParametersUpdates().Count);

            _bank.BankTerms.AddOnUpdateSubscriber(client, AccountType.DebitAccount);

            _bankTerms.SetDebitPercent(55);

            Assert.AreEqual(1, client.GetBankParametersUpdates().Count);

            _bank.BankTerms.AddOnUpdateSubscriber(client, AccountType.DepositAccount);

            var newDepositAndPercents = new List<(Range, decimal)>
            {
                (new Range(0, 228), 5.3m),
                (new Range(322, 777), 6.88m),
            };

            _bankTerms.SetFirstDepositAndPercents(newDepositAndPercents);

            Assert.AreEqual(1, client.GetBankParametersUpdates().Count);
        }

        [Test, Order(4)]
        public void DepositAccountWithdrawMethod_ExceptionsThrownThenTimeRewoundAndSuccess()
        {
            var clientPassportId = Guid.NewGuid();
            Client client = _cb.RegisterClient("NeMisha", "NeLibchenko", clientPassportId)
                .SetAddress("Pushkina st., Kolotushkina h.")
                .SetPhoneNumber("89999996666");

            decimal firstDeposit = 322.228m;

            var depositAccountCreator = new DepositAccountCreator(
                client,
                _bank,
                firstDeposit,
                DateTime.Today.AddMonths(2),
                "My deposit account");

            IAccount depositAccount = depositAccountCreator.CreateAccount();

            _bank.RegisterAccount(client, depositAccount);

            int moneyToWithdraw = 100;
            Assert.Catch<BankException>(() => depositAccount.WithdrawMoney(moneyToWithdraw));

            int monthsToRewind = 1;
            DateTimeProvider.RewindTime(monthsToRewind);
            _cb.TryUpdateAllPercentAccounts();

            Assert.Catch<BankException>(() => depositAccount.WithdrawMoney(moneyToWithdraw));

            DateTimeProvider.RewindTime(monthsToRewind);
            _cb.TryUpdateAllPercentAccounts();
            decimal moneyAfterSecondTimeRewind = depositAccount.GetAmountOfMoney();

            Transaction<IAccount> transaction = _bank.AddTransaction(depositAccount, moneyToWithdraw).Withdraw();

            Assert.IsTrue(Math.Abs(moneyAfterSecondTimeRewind - moneyToWithdraw - depositAccount.GetAmountOfMoney()) < 0.01m);
            
            _bank.CancelTransaction(transaction.Id);
            Assert.IsTrue(Math.Abs(depositAccount.GetAmountOfMoney() - moneyAfterSecondTimeRewind) < 0.01m);
        }
    }
}