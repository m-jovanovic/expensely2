using System.Collections.Generic;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;
using Expensely.Common.Contracts.Expenses;

namespace Expensely.Application.Queries.Expenses.GetExpenses
{
    public sealed class GetExpensesQuery : ICacheableQuery<IReadOnlyCollection<ExpenseDto>>
    {
        public string GetCacheKey() => CacheKeys.Expenses;
    }
}
