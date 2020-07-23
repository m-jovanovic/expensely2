﻿using System;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the create expense request DTO.
    /// </summary>
    public sealed class CreateExpenseRequestDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseRequestDto"/> class.
        /// </summary>
        public CreateExpenseRequestDto() => Name = string.Empty;

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
