using System;
using Expensely.Presentation.Infrastructure;
using Expensely.Presentation.StateManagement.Facades;
using Expensely.Presentation.StateManagement.State;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Expensely.Presentation.Components.Expenses
{
    public class ExpensesListBase : ExpenselyComponent
    {
        [Inject]
        protected IState<ExpensesState> State { get; set; }

        [Inject]
        private IExpensesFacade Facade { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Facade.GetExpenses();
        }

        protected void RemoveExpense(Guid expenseId) => Facade.RemoveExpense(expenseId);
    }
}