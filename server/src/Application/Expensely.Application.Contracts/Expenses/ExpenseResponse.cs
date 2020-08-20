using System;
using Expensely.Domain.ValueObjects;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the expense response.
    /// </summary>
    public sealed class ExpenseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseResponse"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The date and time in UTC format.</param>
        public ExpenseResponse(
            Guid id,
            string name,
            decimal amount,
            string currencyCode,
            DateTime occurredOn,
            DateTime createdOnUtc)
        {
            Id = id;
            Name = name;
            Amount = amount;
            CurrencyCode = currencyCode;
            Value = $"{amount:n2} {Currency.FromCode(currencyCode)!.Symbol}";
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
