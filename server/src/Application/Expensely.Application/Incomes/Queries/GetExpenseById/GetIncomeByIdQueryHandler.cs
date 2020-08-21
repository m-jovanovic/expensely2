using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Incomes.Queries.GetExpenseById
{
    /// <summary>
    /// Represents the <see cref="GetExpenseByIdQuery"/> handler.
    /// </summary>
    internal sealed class GetIncomeByIdQueryHandler : IQueryHandler<GetIncomeByIdQuery, IncomeResponse?>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetIncomeByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetIncomeByIdQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<IncomeResponse?> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.IncomeId == Guid.Empty || request.UserId == Guid.Empty)
            {
                return null;
            }

            IncomeResponse? income = await _dbContext.Set<Income>().AsNoTracking()
                .Where(e => e.Id == request.IncomeId &&
                            e.UserId == request.UserId)
                .Select(e => new IncomeResponse(
                    e.Id,
                    e.Name,
                    e.Money.Amount,
                    e.Money.Currency.Code,
                    e.OccurredOn,
                    e.CreatedOnUtc))
                .FirstOrDefaultAsync(cancellationToken);

            return income;
        }
    }
}
