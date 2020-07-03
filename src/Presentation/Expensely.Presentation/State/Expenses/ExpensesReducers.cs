using Fluxor;

namespace Expensely.Presentation.State.Expenses
{
    public static class ExpensesReducers
    {
        [ReducerMethod]
        public static ExpensesState ReduceGetExpensesDataAction(ExpensesState state, GetExpensesDataAction action)
            => new ExpensesState(true, null);

        [ReducerMethod]
        public static ExpensesState ReduceGetExpensesDataResultAction(ExpensesState state, GetExpensesDataResultAction action)
            => new ExpensesState(false, action.Expenses);
    }
}
