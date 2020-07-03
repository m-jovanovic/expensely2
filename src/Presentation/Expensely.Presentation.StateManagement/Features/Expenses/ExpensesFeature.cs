using Expensely.Presentation.StateManagement.State;
using Fluxor;

namespace Expensely.Presentation.StateManagement.Features.Expenses
{
    public class ExpensesFeature : Feature<ExpensesState>
    {
        public override string GetName() => "Expenses";

        protected override ExpensesState GetInitialState() => new ExpensesState(false, null);
    }
}
