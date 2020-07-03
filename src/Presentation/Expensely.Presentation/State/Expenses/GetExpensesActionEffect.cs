using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Contracts.Expenses;
using Expensely.Presentation.Services;
using Fluxor;

namespace Expensely.Presentation.State.Expenses
{
    public class GetExpensesActionEffect : Effect<GetExpensesDataAction>
    {
        private readonly ExpenseService _expenseService;

        public GetExpensesActionEffect(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        protected override async Task HandleAsync(GetExpensesDataAction action, IDispatcher dispatcher)
        {
            IReadOnlyCollection<ExpenseDto> expenses = await _expenseService.GetExpenses();

            dispatcher.Dispatch(new GetExpensesDataResultAction(expenses));
        }
    }
}
