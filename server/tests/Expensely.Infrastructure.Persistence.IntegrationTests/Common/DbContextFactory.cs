using System;
using Expensely.Infrastructure.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Common
{
    internal static class DbContextFactory
    {
        internal static ExpenselyDbContext Create()
        {
            DbContextOptions options = new DbContextOptionsBuilder<ExpenselyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenselyDbContext(options, new MachineDateTime());

            context.Database.EnsureCreated();

            return context;
        }

        internal static void Destroy(ExpenselyDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}