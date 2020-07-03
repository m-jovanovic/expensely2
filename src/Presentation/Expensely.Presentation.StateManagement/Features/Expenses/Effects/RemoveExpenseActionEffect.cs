using System.Threading.Tasks;
using Expensely.Presentation.Services.Interfaces;
using Expensely.Presentation.StateManagement.Features.Expenses.Actions;
using Fluxor;

namespace Expensely.Presentation.StateManagement.Features.Expenses.Effects
{
    public class RemoveExpenseActionEffect : Effect<RemoveExpenseAction>
    {
        private readonly IExpenseService _expenseService;

        public RemoveExpenseActionEffect(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        protected override async Task HandleAsync(RemoveExpenseAction action, IDispatcher dispatcher)
        {
            await _expenseService.RemoveExpenseAsync(action.ExpenseId);
        }
    }
}
