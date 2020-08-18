using System;
using System.Threading.Tasks;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Persistence.IntegrationTests.Common;
using Expensely.Infrastructure.Persistence.Repositories;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Tests.Common.Entities.ExpenseData;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Repositories
{
    public class ExpenseRepositoryTests : DbContextTest
    {
        private static readonly Guid ExpenseId = new Guid("949ee71f-bbc5-4d58-a8f7-793a2bf0e6b1");

        [Fact]
        public async Task Get_by_id_should_return_null_if_expense_with_specified_id_does_not_exist()
        {
            var expenseRepository = new ExpenseRepository(DbContext);

            Expense? expense = await expenseRepository.GetByIdAsync(Guid.NewGuid());

            expense.Should().BeNull();
        }

        [Fact]
        public async Task Get_by_id_should_return_expense_if_expense_with_specified_id_exists()
        {
            await SeedExpense(ExpenseId);
            var expenseRepository = new ExpenseRepository(DbContext);

            Expense? expense = await expenseRepository.GetByIdAsync(ExpenseId);

            expense.Should().NotBeNull();
        }

        [Fact]
        public async Task Insert_should_add_expense_to_database()
        {
            var expenseRepository = new ExpenseRepository(DbContext);
            var expense = ExpenseData.CreateExpense();

            expenseRepository.Insert(expense);
            await SaveChangesAsync();

            Expense? expenseEntity = await FindAsync<Expense>(expense.Id);
            expenseEntity.Should().NotBeNull();
            expenseEntity.Should().Be(expense);
            expenseEntity.Should().BeEquivalentTo(expense);
        }

        [Fact]
        public async Task Remove_should_remove_expense_from_database()
        {
            var expenseRepository = new ExpenseRepository(DbContext);
            Expense expense = await SeedExpense(ExpenseId);

            expenseRepository.Remove(expense);
            await SaveChangesAsync();

            Expense? foundExpense = await FindAsync<Expense>(expense.Id);
            foundExpense.Should().BeNull();
        }

        private async Task<Expense> SeedExpense(Guid expenseId)
        {
            var expense = new Expense(expenseId, Guid.NewGuid(), Name, MinusOne, DateTime.Now);

            return await InsertAsync(expense);
        }
    }
}
