using System.Collections.Generic;
using Expensely.Contracts.Expenses;

namespace Expensely.Presentation.Store.ExpensesList.Actions
{
    public class GetExpensesDataResultAction
    {
        public GetExpensesDataResultAction(IReadOnlyCollection<ExpenseDto> expenses)
        {
            Expenses = expenses;
        }

        public IReadOnlyCollection<ExpenseDto> Expenses { get; }
    }
}
