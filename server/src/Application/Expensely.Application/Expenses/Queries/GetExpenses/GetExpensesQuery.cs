using System.Collections.Generic;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the query for getting expenses.
    /// </summary>
    public sealed class GetExpensesQuery : ICacheableQuery<IReadOnlyCollection<ExpenseResponse>>
    {
        /// <inheritdoc />
        public string GetCacheKey() => CacheKeys.Expense.List;
    }
}
