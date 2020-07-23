using System;
using Expensely.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Tests.Common
{
    internal static class ExpenselyDbContextFactory
    {
        internal static ExpenselyDbContext Create()
        {
            DbContextOptions options = new DbContextOptionsBuilder<ExpenselyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenselyDbContext(options);

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