using System.Threading.Tasks;
using Expensely.Presentation.Services.Interfaces;
using Expensely.Presentation.Store.ExpensesList.Actions;
using Fluxor;

namespace Expensely.Presentation.Store.ExpensesList.Effects
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
