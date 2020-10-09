using System;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;

namespace Expensely.Application.Incomes.Queries.GetExpenseById
{
    /// <summary>
    /// Represents the query for getting an income by identifier.
    /// </summary>
    public sealed class GetIncomeByIdQuery : ICacheableQuery<Result<IncomeResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetIncomeByIdQuery"/> class.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public GetIncomeByIdQuery(Guid incomeId, Guid userId)
        {
            IncomeId = incomeId;
            UserId = userId;
        }

        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <inheritdoc />
        public string GetCacheKey() => string.Format(CacheKeys.Incomes.IncomeById, UserId, IncomeId);
    }
}
