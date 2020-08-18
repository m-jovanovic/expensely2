using System;

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
        public ExpenseResponse()
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
        /// Gets or sets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; set; }

        /// <summary>
        /// Gets or sets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
