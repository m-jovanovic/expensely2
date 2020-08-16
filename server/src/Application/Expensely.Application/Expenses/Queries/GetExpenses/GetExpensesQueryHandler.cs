using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Utilities;

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

            const string sql =
                @"SELECT ""Id"", ""Name"", ""Amount"", ""CurrencyId"", ""CurrencyCode"", ""Date"", ""CreatedOnUtc""
                FROM ""Expense""
                WHERE NOT ""Deleted"" AND ""UserId"" = @UserId AND (""Date"", ""CreatedOnUtc"") <= (@Date, @CreatedOnUtc)
                ORDER BY ""Date"" DESC, ""CreatedOnUtc"" DESC
                LIMIT @Limit";

            ExpenseResponse[] expenses = await _dbExecutor.QueryAsync<ExpenseResponse>(sql, request);

            if (expenses.Length < request.Limit)
            {
                return new ExpenseListResponse(expenses);
            }

            ExpenseResponse lastExpense = expenses[^1];

            string cursor = Cursor.Create(
                lastExpense.Date.ToString(DateTimeFormats.DatePrecision),
                lastExpense.CreatedOnUtc.ToString(DateTimeFormats.MillisecondPrecision));

            return new ExpenseListResponse(expenses[..^1], cursor);
        }
    }
}