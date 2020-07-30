using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Persistence.IntegrationTests.Common;
using Expensely.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Xunit;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Repositories
{
    public class ExpenseRepositoryTests : DbContextTest
    {
        private static readonly Guid ExpenseId = new Guid("949ee71f-bbc5-4d58-a8f7-793a2bf0e6b1");

        [Fact]
        public async Task Get_by_id_should_return_null_if_no_expenses_exist()
        {
            var expenseRepository = new ExpenseRepository(DbContext);

            Expense? expense = await expenseRepository.GetByIdAsync(ExpenseId);

            expense.Should().BeNull();
        }

        [Fact]
        public async Task Get_by_id_should_return_null_if_expense_with_specified_id_does_not_exist()
        {
            await SeedExpense(Guid.NewGuid());
            var expenseRepository = new ExpenseRepository(DbContext);

            Expense? expense = await expenseRepository.GetByIdAsync(ExpenseId);

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
            var expense = new Expense(ExpenseId, "Name", new Money(1.0m, Currency.Usd), DateTime.Now);

            expenseRepository.Insert(expense);

            Expense? expenseEntity = await DbContext.FindAsync<Expense>(expense.Id);

            expenseEntity.Should().NotBeNull();
            expenseEntity.Should().Be(expense);
            expenseEntity.Should().BeEquivalentTo(expense);
        }

        [Fact]
        public async Task Remove_should_remove_expense_from_database()
        {
            await SeedExpense(ExpenseId);
            var expenseRepository = new ExpenseRepository(DbContext);
            Expense expense = await DbContext.FindAsync<Expense>(ExpenseId);

            expenseRepository.Remove(expense);
            await DbContext.SaveChangesAsync();

            DbContext.Set<Expense>().FirstOrDefault(e => e.Id == ExpenseId).Should().BeNull();
        }

        private async Task SeedExpense(Guid expenseId)
        {
            var expense = new Expense(expenseId, string.Empty, new Money(1.0m, Currency.Usd), DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
