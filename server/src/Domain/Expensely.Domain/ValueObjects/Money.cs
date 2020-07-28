using System;
using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.ValueObjects
{
    /// <summary>
    /// Represents the money value object.
    /// </summary>
    public sealed class Money : ValueObject
    {
        public static readonly Money None = new Money();

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class.
        /// </summary>
        /// <param name="amount">The money amount.</param>
        /// <param name="currency">The currency instance.</param>
        public Money(decimal amount, Currency currency)
        {
            Ensure.NotEmpty(currency, "The currency is required", nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Money()
        {
            Currency = Currency.None;
        }

        /// <summary>
        /// Gets the money amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the money currency.
        /// </summary>
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

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
