using System;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;
using Expensely.Domain.Entities;

namespace Expensely.Persistence.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IDbContext _dbContext;

        public ExpenseRepository(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Expense?> GetByIdAsync(Guid id) => await _dbContext.GetBydIdAsync<Expense>(id);

        /// <inheritdoc />
        public void Insert(Expense expense) => _dbContext.Insert(expense);

        /// <inheritdoc />
        public void Remove(Expense expense) => _dbContext.Remove(expense);
    }
}
