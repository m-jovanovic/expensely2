using System;
using Expensely.Infrastructure.Persistence;

namespace Expensely.Application.IntegrationTests.Common
{
    public abstract class DbContextTest : IDisposable
    {
        protected DbContextTest()
        {
            DbContext = ExpenselyDbContextFactory.Create();
        }

        public void Dispose()
        {
            ExpenselyDbContextFactory.Destroy(DbContext);
        }

        internal ExpenselyDbContext DbContext { get; }
    }
}
