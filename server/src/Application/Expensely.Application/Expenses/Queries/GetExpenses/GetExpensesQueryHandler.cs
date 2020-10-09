using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;
using Expensely.Application.Core.Utilities;
using Expensely.Domain.Core.Maybe;
using Expensely.Domain.Core.Maybe.Extensions;
using Expensely.Domain.Transactions;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
        private static readonly string Sql =
            $@"SELECT id, name, amount, currency currencyId, occurred_on occurredOn, created_on_utc createdOnUtc
                FROM transactions
                WHERE transaction_type = {(int)TransactionType.Expense} AND
                      NOT deleted AND
                      user_id = @UserId AND
                      (occurred_on, created_on_utc) <= (@OccurredOn, @CreatedOnUtc)
                ORDER BY occurred_on DESC, created_on_utc DESC
                LIMIT @Limit";

        private readonly IDbExecutor _dbExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbExecutor">The database executor.</param>
        public GetExpensesQueryHandler(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        /// <inheritdoc />
        public async Task<Maybe<ExpenseListResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken) =>
            await Maybe<GetExpensesQuery>.From(request)
                .Bind(query => GetExpenses(_dbExecutor, Sql, query))
                .Bind(expenses => ConvertToList(expenses, request.Limit));

        private static async Task<Maybe<ExpenseResponse[]>> GetExpenses(
            IDbExecutor dbExecutor,
            string sql,
            GetExpensesQuery query) =>
            await dbExecutor.QueryAsync<ExpenseResponse>(sql, query);

        private static Maybe<ExpenseListResponse> ConvertToList(ExpenseResponse[] expenses, int limit)
        {
            if (expenses.Length < limit)
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