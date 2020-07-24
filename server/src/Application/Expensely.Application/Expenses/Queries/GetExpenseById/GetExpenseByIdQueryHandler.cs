using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Messaging;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Expenses.Queries.GetExpenseById
{
    /// <summary>
    /// Represents the <see cref="GetExpenseByIdQuery"/> handler.
    /// </summary>
    internal sealed class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, ExpenseResponse?>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpenseByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetExpenseByIdQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<ExpenseResponse?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ExpenseId == Guid.Empty)
            {
                return null;
            }

            ExpenseResponse? expense = await _dbContext.Set<Expense>()
                .AsNoTracking()
                .Where(e => e.Id == request.ExpenseId)
                .Select(e => new ExpenseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Amount = e.Money.Amount,
                    CurrencyId = e.Money.Currency.Id,
                    CurrencyCode = e.Money.Currency.Code,
                    Date = e.Date,
                    CreatedOnUtc = e.CreatedOnUtc,
                    ModifiedOnUtc = e.ModifiedOnUtc,
                    Deleted = e.Deleted
                }).FirstOrDefaultAsync(cancellationToken);

            return expense;
        }
    }
}
