using Fluxor;

namespace Expensely.Presentation.Store.ExpensesList
{
    public class ExpensesFeature : Feature<ExpensesState>
    {
        public override string GetName() => "Expenses";

        protected override ExpensesState GetInitialState() => new ExpensesState(false, null);
    }
}
