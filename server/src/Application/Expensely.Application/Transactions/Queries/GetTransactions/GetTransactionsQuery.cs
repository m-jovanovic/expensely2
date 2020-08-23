using System;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;
using Expensely.Application.Core.Utilities;

namespace Expensely.Application.Transactions.Queries.GetTransactions
{
    /// <summary>
    /// Represents the query for getting transactions.
    /// </summary>
    public sealed class GetTransactionsQuery : ICacheableQuery<TransactionListResponse>
    {
        private readonly string _cursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionsQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        public GetTransactionsQuery(Guid userId, int limit, string? cursor, DateTime utcNow)
        {
            UserId = userId;
            Limit = limit + 1;
            _cursor = cursor ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cursor))
            {
                OccurredOn = utcNow.Date;
                CreatedOnUtc = utcNow;
            }
            else
            {
                string[] cursorValues = Cursor.Parse(cursor, 2);
                OccurredOn = DateTime.TryParse(cursorValues[0], out DateTime date) ? date : utcNow.Date;
                CreatedOnUtc = DateTime.TryParse(cursorValues[1], out DateTime createdOn) ? createdOn : utcNow;
            }
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Gets the date.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public string GetCacheKey() => string.Format(CacheKeys.Transactions.TransactionsList, UserId, Limit, _cursor);
    }
}
