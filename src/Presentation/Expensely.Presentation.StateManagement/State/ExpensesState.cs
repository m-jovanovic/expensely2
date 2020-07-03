using System.Collections.Generic;
using Expensely.Contracts.Expenses;

namespace Expensely.Presentation.StateManagement.State
{
    public class ExpensesState
    {
        public ExpensesState(bool isLoading, IReadOnlyCollection<ExpenseDto>? expenses)
            : this()
        {
            IsLoading = isLoading;
            Expenses = expenses;
        }

        public ExpensesState()
        {
        }

        public bool IsLoading { get; }

        public IReadOnlyCollection<ExpenseDto>? Expenses { get; }
    }
}
