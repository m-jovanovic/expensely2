using System;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;
using Expensely.Common.Contracts.Expenses;

namespace Expensely.Application.Expenses.Queries.GetExpenseById
{
    public sealed class GetExpenseByIdQuery : ICacheableQuery<ExpenseDto?>
    {
        public GetExpenseByIdQuery(Guid expenseId)
        {
            ExpenseId = expenseId;
        }

        public Guid ExpenseId { get; }

        public string GetCacheKey() => string.Format(CacheKeys.ExpenseById, ExpenseId);
    }
}
