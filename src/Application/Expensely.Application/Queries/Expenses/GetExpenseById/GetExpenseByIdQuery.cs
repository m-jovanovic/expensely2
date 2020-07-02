using System;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;
using Expensely.Contracts.Expenses;

namespace Expensely.Application.Queries.Expenses.GetExpenseById
{
    public sealed class GetExpenseByIdQuery : ICacheableQuery<ExpenseDto?>
    {
        public GetExpenseByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public string GetCacheKey() => string.Format(CacheKeys.ExpenseById, Id);
    }
}
