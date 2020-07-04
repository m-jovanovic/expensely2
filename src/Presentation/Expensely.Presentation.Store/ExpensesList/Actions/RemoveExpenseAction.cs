using System;

namespace Expensely.Presentation.Store.ExpensesList.Actions
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
