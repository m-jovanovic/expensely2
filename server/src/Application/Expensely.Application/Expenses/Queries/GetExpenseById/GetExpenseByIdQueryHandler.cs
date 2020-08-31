using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Extensions;
using Expensely.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Expenses.Queries.GetExpenseById
{
    /// <summary>
    /// Represents the <see cref="GetExpenseByIdQuery"/> handler.
    /// </summary>
    internal sealed class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, Result<ExpenseResponse>>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpenseByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetExpenseByIdQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result<ExpenseResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken) =>
            await Result.Create(request)
                .Ensure(query => query.ExpenseId != Guid.Empty && query.UserId != Guid.Empty, Errors.General.EntityNotFound)
                .Bind(query =>
                    _dbContext
                        .Set<Expense>()
                        .AsNoTracking()
                        .Where(e => e.Id == query.ExpenseId &&
                                    e.UserId == query.UserId)
                        .Select(e => new ExpenseResponse(
                            e.Id,
                            e.Name,
                            e.Money.Amount,
                            e.Money.Currency.Value,
                            e.OccurredOn,
                            e.CreatedOnUtc))
                        .FirstOrDefaultAsync(cancellationToken))
                .Ensure(expenseResponse => expenseResponse != null, Errors.General.EntityNotFound);
    }
}
