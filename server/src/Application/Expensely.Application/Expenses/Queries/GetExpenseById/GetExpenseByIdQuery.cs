using System;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;

namespace Expensely.Application.Expenses.Queries.GetExpenseById
{
    /// <summary>
    /// Represents the query for getting an expense by identifier.
    /// </summary>
    public sealed class GetExpenseByIdQuery : ICacheableQuery<ExpenseResponse?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpenseByIdQuery"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public GetExpenseByIdQuery(Guid expenseId, Guid userId)
        {
            ExpenseId = expenseId;
            UserId = userId;
        }

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <inheritdoc />
        public string GetCacheKey() => string.Format(CacheKeys.Expenses.ExpenseById, UserId, ExpenseId);
    }
}
