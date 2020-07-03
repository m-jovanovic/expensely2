using System.Collections.Generic;
using Expensely.Contracts.Expenses;

namespace Expensely.Presentation.State.Expenses
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
