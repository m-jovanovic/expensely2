using System;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Domain.Entities;

namespace Expensely.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Represents the expense repository.
    /// </summary>
    internal sealed class ExpenseRepository : IExpenseRepository
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ExpenseRepository(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Expense?> GetByIdAsync(Guid id) => await _dbContext.GetBydIdAsync<Expense>(id);

        /// <inheritdoc />
        public void Insert(Expense expense) => _dbContext.Insert(expense);

        /// <inheritdoc />
        public void Remove(Expense expense) => _dbContext.Remove(expense);
    }
}
