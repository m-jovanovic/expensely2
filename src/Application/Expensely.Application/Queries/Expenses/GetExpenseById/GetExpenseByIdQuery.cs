using System;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;
using Expensely.Contracts.Expenses;

namespace Expensely.Application.Queries.Expenses.GetExpenseById
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
