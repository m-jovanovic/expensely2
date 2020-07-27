using System;
using Expensely.Infrastructure.Persistence;

namespace Expensely.Application.IntegrationTests.Common
{
    public class BaseTest : IDisposable
    {
        private readonly ExpenselyDbContext _dbContext;

        public BaseTest()
        {
            _dbContext = ExpenselyDbContextFactory.Create();
        }

        public void Dispose()
        {
            ExpenselyDbContextFactory.Destroy(_dbContext);
        }

        internal ExpenselyDbContext DbContext => _dbContext;
    }
}
