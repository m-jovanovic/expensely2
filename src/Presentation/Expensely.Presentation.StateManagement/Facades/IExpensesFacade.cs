using System;

namespace Expensely.Presentation.StateManagement.Facades
{
    public interface IExpensesFacade
    {
        void GetExpenses();

        void RemoveExpense(Guid expenseId);
    }
}