using System;
using Expensely.Presentation.Store.ExpensesList.Actions;
using Fluxor;

namespace Expensely.Presentation.Store.ExpensesList.Facades
{
    internal class ExpensesFacade : IExpensesFacade
    {
        private readonly IDispatcher _dispatcher;

        public ExpensesFacade(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void GetExpenses() => _dispatcher.Dispatch(new GetExpensesDataAction());

        public void RemoveExpense(Guid expenseId) => _dispatcher.Dispatch(new RemoveExpenseAction(expenseId));
    }
}
