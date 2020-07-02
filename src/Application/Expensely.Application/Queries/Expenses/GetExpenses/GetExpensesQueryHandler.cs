using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Queries.Expenses.GetExpenses
{
    public sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, IReadOnlyCollection<ExpenseDto>>
    {
        private readonly IDbContext _dbContext;

        public GetExpensesQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<ExpenseDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ExpenseDto> expenses = await _dbContext.Set<Expense>()
                .AsNoTracking()
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Amount = e.Money.Amount,
                    CreatedOnUtc = e.CreatedOnUtc,
                    ModifiedOnUtc = e.ModifiedOnUtc,
                    Deleted = e.Deleted
                }).ToListAsync(cancellationToken);

            return expenses;
        }
    }
}