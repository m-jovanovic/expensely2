using System;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Common
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
