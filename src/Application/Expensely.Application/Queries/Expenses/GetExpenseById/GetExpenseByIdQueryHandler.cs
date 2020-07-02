using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Contracts.Expenses;
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
            if (request.Id == Guid.Empty)
            {
                return null;
            }

            Expense expense = await _dbContext.Set<Expense>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (expense is null)
            {
                return null;
            }

            var expenseDto = new ExpenseDto
            {
                Id = expense.Id,
                Amount = expense.Money.Amount,
                CreatedOnUtc = expense.CreatedOnUtc,
                ModifiedOnUtc = expense.ModifiedOnUtc,
                Deleted = expense.Deleted
            };

            return expenseDto;
        }
    }
}
