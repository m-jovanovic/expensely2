using Expensely.Application.Interfaces;
using Expensely.Domain.Entities;

namespace Expensely.Persistence.Repositories
{
    internal sealed class ExpenseRepository : IExpenseRepository
    {
        private readonly IDbContext _dbContext;

        public ExpenseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public void Insert(Expense expense) => _dbContext.Insert(expense);
    }
}
