using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Cryptography;
using Expensely.Domain.Transactions;
using Expensely.Domain.Users;
using Expensely.Infrastructure.Persistence;
using Expensely.Tests.Common;
using Expensely.Tests.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.IntegrationTests.Core
{
    internal static class DatabaseInitializer
    {
        internal static async Task InitializeDatabaseForTestsAsync(
            ExpenselyDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            await InitializeExpensesForTestsAsync(dbContext);
            await InitializeUsersForTestsAsync(dbContext, serviceProvider);
        }

        internal static async Task InitializeExpensesForTestsAsync(ExpenselyDbContext dbContext)
        {
            if (await dbContext.Set<Expense>().AnyAsync())
            {
                return;
            }

            DateTime date = Time.Now().Date;

            dbContext.Set<Expense>().AddRange(new List<Expense>
            {
                TransactionData.CreateExpense(TestData.UserId, date),
                TransactionData.CreateExpense(TestData.UserId, date.AddDays(-1)),
                TransactionData.CreateExpense(TestData.UserId, date.AddDays(-2)),
                TransactionData.CreateExpense(TestData.UserId, date.AddDays(-3)),
                TransactionData.CreateExpense(TestData.UserId, date.AddDays(-4))
            });

            await dbContext.SaveChangesAsync();
            
            TestData.ExpenseIdForReading = dbContext.Set<Expense>().First().Id;
            TestData.ExpenseIdForDeleting = dbContext.Set<Expense>().Last().Id;
        }

        internal static async Task InitializeUsersForTestsAsync(
            ExpenselyDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            if (await dbContext.Set<User>().AnyAsync())
            {
                return;
            }

            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

            string passwordHash = passwordHasher.HashPassword(Password.Create(UserData.Password).Value());

            User entity = new User(
                Guid.NewGuid(),
                UserData.ValidFirstName,
                UserData.ValidLastName,
                UserData.ValidEmail,
                passwordHash);
            
            dbContext.Set<User>().Add(entity);

            await dbContext.SaveChangesAsync();
        }
    }
}
