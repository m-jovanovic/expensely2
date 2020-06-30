using System;
using Expensely.Application.Messaging;

namespace Expensely.Application.Events.Expenses.ExpenseCreated
{
    public sealed class ExpenseCreatedEvent : IEvent
    {
        public ExpenseCreatedEvent(Guid expenseId)
        {
            ExpenseId = expenseId;
        }

        public Guid ExpenseId { get; }
    }
}
