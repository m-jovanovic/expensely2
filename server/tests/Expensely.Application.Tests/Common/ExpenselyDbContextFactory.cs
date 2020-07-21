using System;
using Expensely.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Tests.Common
{
    public static class ExpenselyDbContextFactory
    {
        public static ExpenselyDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ExpenselyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenselyDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(ExpenselyDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}