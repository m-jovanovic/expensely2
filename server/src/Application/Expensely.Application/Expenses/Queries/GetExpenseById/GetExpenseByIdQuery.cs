using System;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Queries.GetExpenseById
{
    /// <summary>
    /// Represents the query for getting an expense by identifier.
    /// </summary>
    public sealed class GetExpenseByIdQuery : ICacheableQuery<ExpenseDto?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpenseByIdQuery"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        public GetExpenseByIdQuery(Guid expenseId) => ExpenseId = expenseId;

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }

        /// <inheritdoc />
        public string GetCacheKey() => string.Format(CacheKeys.ExpenseById, ExpenseId);
    }
}
