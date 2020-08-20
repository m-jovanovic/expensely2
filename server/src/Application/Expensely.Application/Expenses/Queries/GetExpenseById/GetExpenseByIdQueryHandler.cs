using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Expenses;
using Expensely.Domain.Transactions;
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
            if (request.ExpenseId == Guid.Empty || request.UserId == Guid.Empty)
            {
                return null;
            }

            ExpenseResponse? expense = await _dbContext.Set<Expense>().AsNoTracking()
                .Where(e => e.Id == request.ExpenseId &&
                            e.UserId == request.UserId)
                .Select(e => new ExpenseResponse(
                    e.Id,
                    e.Name,
                    e.Money.Amount,
                    e.Money.Currency.Code,
                    e.OccurredOn,
                    e.CreatedOnUtc))
                .FirstOrDefaultAsync(cancellationToken);

            return expense;
        }
    }
}
