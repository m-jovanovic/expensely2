using System;
using Expensely.Persistence;

namespace Expensely.Application.Tests.Common
{
    public class BaseTest : IDisposable
    {
        protected readonly ExpenselyDbContext _dbContext;

        public BaseTest()
        {
            _dbContext = ExpenselyDbContextFactory.Create();
        }

        public void Dispose()
        {
            ExpenselyDbContextFactory.Destroy(_dbContext);
        }
    }
}
