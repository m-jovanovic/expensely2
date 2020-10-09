using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Domain;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Core.Result.Extensions;
using Expensely.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Transactions.Queries.GetCurrentWeekBalance
{
    /// <summary>
    /// Represents the <see cref="GetCurrentWeekBalanceQuery"/> handler.
    /// </summary>
    internal sealed class GetCurrentWeekBalanceQueryHandler : IQueryHandler<GetCurrentWeekBalanceQuery, Result<BalanceResponse>>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentWeekBalanceQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetCurrentWeekBalanceQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result<BalanceResponse>> Handle(
            GetCurrentWeekBalanceQuery request, CancellationToken cancellationToken) =>
            await Result.Success(request)
                .Ensure(
                    query => query.UserId != Guid.Empty && Currency.ContainsValue(query.CurrencyId),
                    Errors.General.EntityNotFound)
                .BindScalar(query =>
                    _dbContext
                        .Set<Transaction>()
                        .AsNoTracking()
                        .Where(t =>
                            t.UserId == query.UserId &&
                            t.Money.Currency.Value == query.CurrencyId &&
                            t.OccurredOn >= query.FirstDayOfWeek)
                        .SumAsync(x => x.Money.Amount, cancellationToken))
                .Map(sum => new BalanceResponse(request.CurrencyId, sum));
    }
}
