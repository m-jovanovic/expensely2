using System;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseDeleted
{
    /// <summary>
    /// Represents the event that is raised when an expense is deleted.
    /// </summary>
    public class ExpenseDeletedEvent : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseDeletedEvent"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        public ExpenseDeletedEvent(Guid expenseId) => ExpenseId = expenseId;

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }
    }
}
