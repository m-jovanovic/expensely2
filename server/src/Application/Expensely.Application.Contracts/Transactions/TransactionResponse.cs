using System;
using Expensely.Domain.ValueObjects;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction response.
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionResponse"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="transactionType">The transaction type.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The date and time in UTC format.</param>
        public TransactionResponse(
            Guid id,
            string name,
            decimal amount,
            string currencyCode,
            int transactionType,
            DateTime occurredOn,
            DateTime createdOnUtc)
        {
            Id = id;
            Name = name;
            Amount = amount;
            CurrencyCode = currencyCode;
            Value = $"{amount:n2} {Currency.FromCode(currencyCode)!.Symbol}";
            TransactionType = transactionType;
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
        /// Gets the transaction type.
        /// </summary>
        public int TransactionType { get; }

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
