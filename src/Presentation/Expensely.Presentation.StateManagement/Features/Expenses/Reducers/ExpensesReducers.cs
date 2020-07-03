using System.Collections.Generic;
using System.Linq;
using Expensely.Contracts.Expenses;
using Expensely.Presentation.StateManagement.Features.Expenses.Actions;
using Expensely.Presentation.StateManagement.State;
using Fluxor;

namespace Expensely.Presentation.StateManagement.Features.Expenses.Reducers
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
            List<ExpenseDto> remainingExpenses = state.Expenses.Where(e => e.Id != action.ExpenseId).ToList();

            return new ExpensesState(false, remainingExpenses);
        }
    }
}
