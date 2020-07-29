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
    public class GetExpenseByIdQueryTests : BaseTest
    {
        private const string Name = "Expense";
        private static readonly Money Money = new Money(decimal.Zero, Currency.Usd);

        [Fact]
        public async Task Handle_should_return_null_given_empty_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            var query = new GetExpenseByIdQuery(Guid.Empty);

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_should_return_null_given_non_existing_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_should_return_ExpenseDto_given_existing_id()
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
            var expense = new Expense(Guid.NewGuid(), Name, Money, DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
