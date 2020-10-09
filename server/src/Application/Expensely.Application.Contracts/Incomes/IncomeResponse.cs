using System;
using Expensely.Domain.Transactions;

namespace Expensely.Application.Contracts.Incomes
{
    /// <summary>
    /// Represents the expense response.
    /// </summary>
    public sealed class IncomeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeResponse"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyId">The currency idenifier.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The date and time in UTC format.</param>
        public IncomeResponse(
            Guid id,
            string name,
            decimal amount,
            int currencyId,
            DateTime occurredOn,
            DateTime createdOnUtc)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Currency currency = Currency.FromValue(currencyId).Value;
            CurrencyCode = currency.Code;
            Value = currency.Format(amount);
            OccurredOn = occurredOn;
            CreatedOnUtc = createdOnUtc;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string CurrencyCode { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; }
    }
}
