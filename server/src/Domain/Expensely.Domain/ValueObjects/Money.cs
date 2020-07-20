using System;
using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public static readonly Money Null = new Money(decimal.Zero, Currency.Null);

        public Money(decimal amount, Currency currency)
            : this()
        {
            Amount = amount;
            Currency = currency;
        }

        private Money()
        {
            Currency = Currency.Null;
        }

        public decimal Amount { get; }

        public Currency Currency { get; }

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new InvalidOperationException($"Can not add currencies {left.Currency.Code} and {right.Currency.Code}.");
            }

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new InvalidOperationException($"Can not add currencies {left.Currency.Code} and {right.Currency.Code}.");
            }

            return new Money(left.Amount - right.Amount, left.Currency);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
