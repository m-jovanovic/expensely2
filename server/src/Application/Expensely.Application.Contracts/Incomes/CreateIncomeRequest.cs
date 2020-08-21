using System;

namespace Expensely.Application.Contracts.Incomes
{
    /// <summary>
    /// Represents the create income request.
    /// </summary>
    public sealed class CreateIncomeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeRequest"/> class.
        /// </summary>
        public CreateIncomeRequest()
        {
            Name = string.Empty;
            CurrencyCode = string.Empty;
        }

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
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
