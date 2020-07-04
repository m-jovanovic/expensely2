using System;

namespace Expensely.Presentation.Store.ExpensesList.Facades
{
    public interface IExpensesFacade
    {
        void GetExpenses();

        void RemoveExpense(Guid expenseId);
    }
}