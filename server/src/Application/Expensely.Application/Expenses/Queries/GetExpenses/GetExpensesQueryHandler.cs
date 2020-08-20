using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;
using Expensely.Application.Core.Utilities;
using Expensely.Domain.Transactions;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, ExpenseListResponse>
    {
        private readonly IDbExecutor _dbExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbExecutor">The database executor.</param>
        public GetExpensesQueryHandler(IDbExecutor dbExecutor) => _dbExecutor = dbExecutor;

        /// <inheritdoc />
        public async Task<ExpenseListResponse> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return new ExpenseListResponse(Array.Empty<ExpenseResponse>());
            }

            string sql =
                $@"SELECT id, name, amount, currency_code currencyCode, occurred_on occurredOn, created_on_utc createdOnUtc
                FROM transactions
                WHERE NOT deleted AND
                      transaction_type = {(int)TransactionType.Expense} AND
                      user_id = @UserId AND
                      (occurred_on, created_on_utc) <= (@OccurredOn, @CreatedOnUtc)
                ORDER BY occurred_on DESC, created_on_utc DESC
                LIMIT @Limit";

            ExpenseResponse[] expenses = await _dbExecutor.QueryAsync<ExpenseResponse>(sql, request);

            if (expenses.Length < request.Limit)
            {
                return new ExpenseListResponse(expenses);
            }

            ExpenseResponse lastExpense = expenses[^1];

            string cursor = Cursor.Create(
                lastExpense.OccurredOn.ToString(DateTimeFormats.DatePrecision),
                lastExpense.CreatedOnUtc.ToString(DateTimeFormats.MillisecondPrecision));

            return new ExpenseListResponse(expenses[..^1], cursor);
        }
    }
}