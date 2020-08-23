using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Transactions.Queries.GetCurrentWeekBalance
{
    /// <summary>
    /// Represents the <see cref="GetCurrentWeekBalanceQuery"/> handler.
    /// </summary>
    internal sealed class GetCurrentWeekBalanceQueryHandler : IQueryHandler<GetCurrentWeekBalanceQuery, BalanceResponse?>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentWeekBalanceQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetCurrentWeekBalanceQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<BalanceResponse?> Handle(GetCurrentWeekBalanceQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return null;
            }

            decimal amount = await _dbContext.Set<Transaction>().AsNoTracking()
                .Where(t => t.UserId == request.UserId &&
                            t.Money.Currency.Value == request.CurrencyId &&
                            t.OccurredOn >= request.FirstDayOfWeek)
                .SumAsync(t => t.Money.Amount, cancellationToken);

            var balanceResponse = new BalanceResponse(amount, request.CurrencyId);

            return balanceResponse;
        }
    }
}
