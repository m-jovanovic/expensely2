using System;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseDeleted
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
