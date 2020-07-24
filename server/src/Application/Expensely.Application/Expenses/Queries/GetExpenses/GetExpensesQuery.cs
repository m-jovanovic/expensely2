using System.Collections.Generic;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the query for getting expenses.
    /// </summary>
    public sealed class GetExpensesQuery : ICacheableQuery<IReadOnlyCollection<ExpenseResponse>>
    {
        /// <inheritdoc />
        public string GetCacheKey() => CacheKeys.Expenses;
    }
}
