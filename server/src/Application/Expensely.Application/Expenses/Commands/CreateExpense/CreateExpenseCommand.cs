﻿using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Common;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Application.Expenses.Commands.CreateExpense
{
    /// <summary>
    /// Represents the command for creating an expense.
    /// </summary>
    public sealed class CreateExpenseCommand : ICommand<Result<EntityCreatedResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        public CreateExpenseCommand(Guid userId, string name, decimal amount, string currencyCode, DateTime occurredOn)
        {
            UserId = userId;
            Name = name;
            Amount = amount;
            CurrencyCode = currencyCode;
            OccurredOn = occurredOn;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        public string CurrencyCode { get; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; }
    }
}
