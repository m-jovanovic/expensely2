using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
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
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="sqlConnectionFactory">The SQL connection factory.</param>
        public GetExpensesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        /// <inheritdoc />
        public async Task<ExpenseListResponse> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return new ExpenseListResponse(Array.Empty<ExpenseResponse>());
            }

            using IDbConnection connection = await _sqlConnectionFactory.CreateSqlConnectionAsync(cancellationToken);

            ExpenseResponse[] expenses = (await connection.QueryAsync<ExpenseResponse>(
                    @"SELECT ""Id"", ""Name"", ""Amount"", ""CurrencyId"", ""CurrencyCode"", ""Date"", ""CreatedOnUtc""
                      FROM ""Expense""
                      WHERE NOT ""Deleted"" AND ""UserId"" = @UserId AND (""Date"", ""CreatedOnUtc"") <= (@Date, @CreatedOnUtc)
                      ORDER BY ""Date"" DESC, ""CreatedOnUtc"" DESC
                      LIMIT @Limit",
                    new
                    {
                        request.UserId,
                        request.Date,
                        request.CreatedOnUtc,
                        request.Limit
                    }))
                .ToArray();

            if (expenses.Length != request.Limit)
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