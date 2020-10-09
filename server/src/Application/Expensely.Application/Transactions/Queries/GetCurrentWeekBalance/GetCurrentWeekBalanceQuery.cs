using System;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;

namespace Expensely.Application.Transactions.Queries.GetCurrentWeekBalance
{
    /// <summary>
    /// Represents the query for getting the current week balance.
    /// </summary>
    public sealed class GetCurrentWeekBalanceQuery : IQuery<Result<BalanceResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentWeekBalanceQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <param name="firstDayOfWeek">The first day of the current week.</param>
        public GetCurrentWeekBalanceQuery(Guid userId, int currencyId, DateTime firstDayOfWeek)
        {
            UserId = userId;
            CurrencyId = currencyId;
            FirstDayOfWeek = firstDayOfWeek;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        public int CurrencyId { get; }

        /// <summary>
        /// Gets the first day of the week.
        /// </summary>
        public DateTime FirstDayOfWeek { get; }
    }
}
