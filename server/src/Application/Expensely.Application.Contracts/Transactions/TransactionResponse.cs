using System;

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
        public TransactionResponse()
        {
            Name = string.Empty;
            CurrencyCode = string.Empty;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets the expense value.
        /// </summary>
        public string Value => $"{Amount:n2} {CurrencyCode}";

        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        public int TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; set; }

        /// <summary>
        /// Gets or sets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
