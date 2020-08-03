using System;
using Expensely.Application.Abstractions.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseCreated
{
    /// <summary>
    /// Represents the event that is raised when an event is created.
    /// </summary>
    public sealed class ExpenseCreatedEvent : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseCreatedEvent"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        public ExpenseCreatedEvent(Guid expenseId) => ExpenseId = expenseId;

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }
    }
}
