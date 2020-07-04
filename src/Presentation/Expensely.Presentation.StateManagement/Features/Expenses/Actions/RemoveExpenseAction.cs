using System;

namespace Expensely.Presentation.StateManagement.Features.Expenses.Actions
{
    public class RemoveExpenseAction
    {
        public RemoveExpenseAction(Guid expenseId)
        {
            ExpenseId = expenseId;
        }

        public Guid ExpenseId { get; }
    }
}
