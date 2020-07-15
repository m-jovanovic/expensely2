using System;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseCreated
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
