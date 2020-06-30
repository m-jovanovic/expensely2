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
                .Select(x => new ExpenseDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    CreatedOnUtc = x.CreatedOnUtc,
                    ModifiedOnUtc = x.ModifiedOnUtc,
                    Deleted = x.Deleted
                }).ToListAsync(cancellationToken);

            return expenses;
        }
    }
}