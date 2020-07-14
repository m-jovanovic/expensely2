using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Common.Contracts.Expenses;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Queries.Expenses.GetExpenseById
{
    public sealed class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, ExpenseDto?>
    {
        private readonly IDbContext _dbContext;

        public GetExpenseByIdQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ExpenseId == Guid.Empty)
            {
                return null;
            }

            ExpenseDto? expense = await _dbContext.Set<Expense>()
                .AsNoTracking()
                .Where(e => e.Id == request.ExpenseId)
                .Select(e => new ExpenseDto
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
