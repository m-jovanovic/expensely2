using System;
using System.Collections.Generic;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
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
