using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Domain.Entities;
using Expensely.Infrastructure.Persistence;
using Expensely.Tests.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Api.IntegrationTests.Core
{
    internal static class DatabaseInitializer
    {
        internal static async Task InitializeExpensesForTestsAsync(ExpenselyDbContext dbContext)
        {
            if (await dbContext.Set<Expense>().AnyAsync())
            {
                return;
            }

            dbContext.Set<Expense>().AddRange(new List<Expense>
            {
                ExpenseData.Expense,
                ExpenseData.Expense,
                ExpenseData.Expense
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
