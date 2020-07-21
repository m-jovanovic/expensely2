using System.Collections.Generic;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    public sealed class GetExpensesQuery : ICacheableQuery<IReadOnlyCollection<ExpenseDto>>
    {
        public string GetCacheKey() => CacheKeys.Expenses;
    }
}
