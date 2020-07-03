using System;
using Expensely.Presentation.StateManagement.Features.Expenses.Actions;
using Fluxor;

namespace Expensely.Presentation.StateManagement.Facades
{
    public class ExpensesFacade
    {
        private readonly IDispatcher _dispatcher;

        public ExpensesFacade(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void GetExpenses()
        {
            _dispatcher.Dispatch(new GetExpensesDataAction());
        }

        public void RemoveExpense(Guid expenseId)
        {
            _dispatcher.Dispatch(new RemoveExpenseAction(expenseId));
        }
    }
}
