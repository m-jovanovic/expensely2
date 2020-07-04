using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Contracts.Expenses;
using Expensely.Presentation.Services.Interfaces;
using Expensely.Presentation.Store.ExpensesList.Actions;
using Fluxor;

namespace Expensely.Presentation.Store.ExpensesList.Effects
{
    public class GetExpensesActionEffect : Effect<GetExpensesDataAction>
    {
        private readonly IExpenseService _expenseService;

        public GetExpensesActionEffect(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        protected override async Task HandleAsync(GetExpensesDataAction action, IDispatcher dispatcher)
        {
            IReadOnlyCollection<ExpenseDto> expenses = await _expenseService.GetExpensesAsync();

            dispatcher.Dispatch(new GetExpensesDataResultAction(expenses));
        }
    }
}
