using System;
using Expensely.Application.Messaging;

namespace Expensely.Application.Events.Expenses.ExpenseDeleted
{
    public class ExpenseDeletedEvent : IEvent
    {
        public ExpenseDeletedEvent(Guid expenseId)
        {
            ExpenseId = expenseId;
        }

        public Guid ExpenseId { get; }
    }
}
