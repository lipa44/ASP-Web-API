#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class BankTerms : IPublisher
    {
        private static readonly string CurrencyIcon = NumberFormatInfo.CurrentInfo.CurrencySymbol;
        private readonly Dictionary<Client, List<AccountType>> _updateSubscribers;

        public BankTerms(
            decimal debitPercent,
            decimal creditCommission,
            decimal creditMaxLimit,
            decimal creditMinLimit,
            decimal transactionLimit,
            decimal transactionLimitForUnverified,
            List<(Range, decimal)> firstDepositAndPercents)
        {
            DebitPercent = debitPercent;
            CreditCommission = creditCommission;
            CreditMaxLimit = creditMaxLimit;
            CreditMinLimit = creditMinLimit;
            TransactionLimit = transactionLimit;
            TransactionLimitForUnverified = transactionLimitForUnverified;
            FirstDepositAndPercents = firstDepositAndPercents;
            _updateSubscribers = new Dictionary<Client, List<AccountType>>();
        }

        public decimal DebitPercent { get; private set; }
        public decimal CreditCommission { get; private set; }
        public decimal CreditMaxLimit { get; private set; }
        public decimal CreditMinLimit { get; private set; }
        public decimal TransactionLimit { get; private set; }
        public decimal TransactionLimitForUnverified { get; private set; }
        public List<(Range, decimal)> FirstDepositAndPercents { get; private set; }

        public void SetDebitPercent(decimal debitPercent)
        {
            if (debitPercent < 0)
                throw new BankException("Credit percent must be positive");

            DebitPercent = debitPercent;
            NotifySubscribersAboutUpdate(nameof(debitPercent), debitPercent);
        }

        public void SetCreditCommission(decimal creditCommission)
        {
            if (creditCommission < 0)
                throw new BankException("Credit commission must be positive");

            CreditCommission = creditCommission;
            NotifySubscribersAboutUpdate(nameof(creditCommission), creditCommission);
        }

        public void SetCreditMaxLimit(decimal creditMaxLimit)
        {
            if (creditMaxLimit < 0)
                throw new BankException("Credit max limit must be positive");

            CreditMaxLimit = creditMaxLimit;
            NotifySubscribersAboutUpdate(nameof(creditMaxLimit), creditMaxLimit);
        }

        public void SetCreditMinLimit(decimal creditMinLimit)
        {
            if (creditMinLimit > 0)
                throw new BankException("Credit min limit must be opposite");

            CreditMinLimit = creditMinLimit;
            NotifySubscribersAboutUpdate(nameof(creditMinLimit), creditMinLimit);
        }

        public void SetTransactionLimit(decimal transactionLimit)
        {
            if (transactionLimit < 0)
                throw new BankException("Transaction limit must be positive");

            TransactionLimit = transactionLimit;
            NotifySubscribersAboutUpdate(nameof(transactionLimit), transactionLimit);
        }

        public void SetTransactionLimitForUnverified(decimal transactionLimitForUnverified)
        {
            if (transactionLimitForUnverified < 0)
                throw new BankException("Transaction limit for unverified must be positive");

            if (transactionLimitForUnverified > TransactionLimit)
                throw new BankException("Transaction limit for unverified must less then for verified ones");

            TransactionLimitForUnverified = transactionLimitForUnverified;
            NotifySubscribersAboutUpdate(nameof(transactionLimitForUnverified), transactionLimitForUnverified);
        }

        public void SetFirstDepositAndPercents(List<(Range, decimal)> firstDepositAndPercents)
        {
            if (firstDepositAndPercents is null)
                throw new BankException("First deposits and percents are null");

            IsFirstDepositAndPercentsAreValidOrException(firstDepositAndPercents);

            FirstDepositAndPercents = firstDepositAndPercents;
            NotifySubscribersAboutDepositsUpdate(firstDepositAndPercents);
        }

        public decimal CalculatePercentByDeposit(decimal firstDeposit)
        {
            if (FirstDepositAndPercents is null)
                throw new BankException("First deposit and percents list to calculate percent by deposit is null");

            foreach ((Range depositRange, decimal percent) in FirstDepositAndPercents)
                if (depositRange.Start.Value <= firstDeposit && depositRange.End.Value > firstDeposit) return percent;

            return FirstDepositAndPercents.Last().Item2;
        }

        public void AddOnUpdateSubscriber(Client client, AccountType accountType)
        {
            if (client is null)
                throw new BankException("Client to subscribe on updates is null");

            if (_updateSubscribers.ContainsKey(client))
            {
                if (_updateSubscribers[client].Contains(accountType))
                    throw new BankException("Subscription has been already added");

                _updateSubscribers[client].Add(accountType);
            }
            else
            {
                _updateSubscribers.Add(client, new List<AccountType> { accountType });
            }
        }

        public void RemoveOnUpdateSubscriber(Client client, AccountType accountType)
        {
            if (client is null)
                throw new BankException("Client to subscribe on updates is null");

            if (!_updateSubscribers.ContainsKey(client))
                throw new BankException("Client doesn't exist in update subscribers list");

            if (!_updateSubscribers[client].Contains(accountType))
                throw new BankException("Subscription hasn't been added");

            _updateSubscribers[client].Remove(accountType);
        }

        private void NotifySubscribersAboutUpdate(string nameOfParameter, decimal newValue)
        {
            foreach (Client client in GetUpdatingSubscribersByAccountType(AccountType.CreditAccount))
            {
                client.AddBankParametersUpdates(
                    $"[bold green]Bank terms updated[/]: new {nameOfParameter} is {newValue:C}\n");
            }
        }

        private void NotifySubscribersAboutDepositsUpdate(List<(Range, decimal)> newFirstDepositsAndPercents)
        {
            string updateStr = newFirstDepositsAndPercents
                .Aggregate("[bold green]Deposit terms updated[/]: ", (current, percentRange) =>
                    current + $"{percentRange.Item1.Start.Value} - {percentRange.Item1.End.Value} " +
                    $"[grey]{CurrencyIcon}[/] is {percentRange.Item2} %\n");

            foreach (Client client in GetUpdatingSubscribersByAccountType(AccountType.CreditAccount))
                client.AddBankParametersUpdates($"{updateStr}\n");
        }

        private void IsFirstDepositAndPercentsAreValidOrException(List<(Range, decimal)> firstDepositAndPercents)
        {
            firstDepositAndPercents = firstDepositAndPercents.OrderBy(range => range.Item1.Start.Value).ToList();

            foreach ((Range, decimal) rangePair in firstDepositAndPercents)
                IsFirstDepositAndPercentsAreValidValuesOrException(rangePair);

            for (int i = 0; i < firstDepositAndPercents.Count; ++i)
            {
                for (int j = i + 1; j < firstDepositAndPercents.Count; ++j)
                {
                    if (firstDepositAndPercents[i].Item1.End.Value > firstDepositAndPercents[j].Item1.Start.Value)
                        throw new BankException("Deposits ranges to create bank are not valid");
                }
            }
        }

        private void IsFirstDepositAndPercentsAreValidValuesOrException((Range, decimal) rangePair)
        {
            (Range range, decimal percent) = rangePair;

            if (percent is <= 0 or >= 100)
                throw new BankException("Percent must be in range 1..99");

            if (range.Start.Value < 0 || range.End.Value < 0)
                throw new BankException("Range start (and end) value must be positive");

            if (range.Start.Value > range.End.Value)
                throw new BankException("Range start must be less then range end");
        }

        private List<Client> GetUpdatingSubscribersByAccountType(AccountType type) => _updateSubscribers
            .Where(kvp => kvp.Value.Contains(type))
            .Select(x => x.Key).ToList();
    }
}