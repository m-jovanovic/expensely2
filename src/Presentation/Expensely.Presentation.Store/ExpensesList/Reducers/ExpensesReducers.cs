using System.Collections.Generic;
using System.Linq;
using Expensely.Common.Contracts.Expenses;
using Expensely.Presentation.Store.ExpensesList.Actions;
using Fluxor;

namespace Expensely.Presentation.Store.ExpensesList.Reducers
{
    public static class ExpensesReducers
    {
        [ReducerMethod]
        public static ExpensesState ReduceGetExpensesDataAction(ExpensesState state, GetExpensesDataAction action)
            => new ExpensesState(true, null);

        [ReducerMethod]
        public static ExpensesState ReduceGetExpensesDataResultAction(ExpensesState state, GetExpensesDataResultAction action)
            => new ExpensesState(false, action.Expenses);

        [ReducerMethod]
        public static ExpensesState ReduceRemoveExpenseAction(ExpensesState state, RemoveExpenseAction action)
        {
            IReadOnlyCollection<ExpenseDto> remainingExpenses = state.Expenses.Where(e => e.Id != action.ExpenseId).ToList();

            return new ExpensesState(false, remainingExpenses);
        }
    }
}
