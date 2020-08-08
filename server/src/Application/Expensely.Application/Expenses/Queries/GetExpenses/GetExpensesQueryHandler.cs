using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Utilities;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, ExpenseListResponse>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetExpensesQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<ExpenseListResponse> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return new ExpenseListResponse(Array.Empty<ExpenseResponse>());
            }

            ExpenseResponse[] expenses = await _dbContext.Set<Expense>().AsNoTracking()
                .Where(e => e.UserId == request.UserId &&
                            e.Date <= request.Date &&
                            e.CreatedOnUtc <= request.CreatedOnUtc)
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.CreatedOnUtc)
                .Take(request.Limit)
                .Select(e => new ExpenseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Amount = e.Money.Amount,
                    CurrencyId = e.Money.Currency.Id,
                    CurrencyCode = e.Money.Currency.Code,
                    Date = e.Date,
                    CreatedOnUtc = e.CreatedOnUtc
                }).ToArrayAsync(cancellationToken);

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