using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Domain.Transactions;

namespace Expensely.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Represents the income repository.
    /// </summary>
    internal sealed class IncomeRepository : IIncomeRepository
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public IncomeRepository(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public void Insert(Income income) => _dbContext.Insert(income);
    }
}