using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;
using Expensely.Application.Core.Utilities;

namespace Expensely.Application.Transactions.Queries.GetTransactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> handler.
    /// </summary>
    internal sealed class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, TransactionListResponse>
    {
        private readonly IDbExecutor _dbExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionsQueryHandler"/> class.
        /// </summary>
        /// <param name="dbExecutor">The database executor.</param>
        public GetTransactionsQueryHandler(IDbExecutor dbExecutor) => _dbExecutor = dbExecutor;

        /// <inheritdoc />
        public async Task<TransactionListResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return new TransactionListResponse(Array.Empty<TransactionResponse>());
            }

            const string sql =
                @"SELECT id, name, amount, currency_code currencyCode, transaction_type transactionType,
                         occurred_on occurredOn, created_on_utc createdOnUtc
                FROM transactions
                WHERE NOT deleted AND
                      user_id = @UserId AND
                      (occurred_on, created_on_utc) <= (@OccurredOn, @CreatedOnUtc)
                ORDER BY occurred_on DESC, created_on_utc DESC
                LIMIT @Limit";

            TransactionResponse[] transactions = await _dbExecutor.QueryAsync<TransactionResponse>(sql, request);

            if (transactions.Length < request.Limit)
            {
                return new TransactionListResponse(transactions);
            }

            TransactionResponse lastTransaction = transactions[^1];

            string cursor = Cursor.Create(
                lastTransaction.OccurredOn.ToString(DateTimeFormats.DatePrecision),
                lastTransaction.CreatedOnUtc.ToString(DateTimeFormats.MillisecondPrecision));

            return new TransactionListResponse(transactions[..^1], cursor);
        }
    }
}