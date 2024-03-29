﻿using System;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the create expense request.
    /// </summary>
    public sealed class CreateExpenseRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseRequest"/> class.
        /// </summary>
        public CreateExpenseRequest() => Name = string.Empty;

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }

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
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
