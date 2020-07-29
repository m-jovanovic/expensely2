using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests : DbContextTest
    {
        [Fact]
        public async Task Handle_should_return_null_given_non_existing_expense_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_should_return_expense_response_given_existing_expense_id()
        {
            await SeedExpenses();
            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            Expense expense = DbContext.Set<Expense>().First();
            var query = new GetExpenseByIdQuery(expense.Id);

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result!.Id.Should().Be(expense.Id);
            result.Name.Should().Be(expense.Name);
            result.Amount.Should().Be(expense.Money.Amount);
            result.CurrencyId.Should().Be(expense.Money.Currency.Id);
            result.CurrencyCode.Should().Be(expense.Money.Currency.Code);
            result.Date.Should().Be(expense.Date);
            result.CreatedOnUtc.Should().Be(expense.CreatedOnUtc);
            result.ModifiedOnUtc.Should().Be(expense.ModifiedOnUtc);
            result.Deleted.Should().Be(expense.Deleted);
        }

        private async Task SeedExpenses()
        {
            var expense = new Expense(Guid.NewGuid(), "Expense", new Money(decimal.Zero, Currency.Usd), DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
