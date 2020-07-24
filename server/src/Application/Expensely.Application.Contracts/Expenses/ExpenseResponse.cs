using System;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the expense.
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
        /// Gets or sets the currency identifier.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets the formatted expense.
        /// </summary>
        public string FormattedExpense => $"{Amount:n2} {CurrencyCode}";

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the created on date in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the modified on date in UTC format.
        /// </summary>
        public DateTime? ModifiedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the expense has been deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
}
