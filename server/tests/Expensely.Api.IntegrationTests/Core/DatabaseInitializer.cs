using System.Collections.Generic;
using System.Linq;
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
                ExpenseData.CreateExpense(),
                ExpenseData.CreateExpense(),
                ExpenseData.CreateExpense()
            });

            await dbContext.SaveChangesAsync();
            
            TestData.ExpenseIdForReading = dbContext.Set<Expense>().First().Id;
            TestData.ExpenseIdForDeleting = dbContext.Set<Expense>().Last().Id;
        }
    }
}
