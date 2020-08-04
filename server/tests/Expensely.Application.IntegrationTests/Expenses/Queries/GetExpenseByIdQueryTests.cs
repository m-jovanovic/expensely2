using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests
    {
        [Fact]
        public async Task Should_return_null_given_non_existing_expense_id()
        {
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            ExpenseResponse? result = await SendAsync(query);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_expense_response_given_existing_expense_id()
        {
            var expense = ExpenseData.Expense;
            await AddAsync(expense);
            var query = new GetExpenseByIdQuery(expense.Id);

            ExpenseResponse? result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Id.Should().Be(expense.Id);
            result.Name.Should().Be(expense.Name);
            result.Amount.Should().Be(expense.Money.Amount);
            result.CurrencyId.Should().Be(expense.Money.Currency.Id);
            result.CurrencyCode.Should().Be(expense.Money.Currency.Code);
            result.Date.Should().Be(expense.Date);
            result.CreatedOnUtc.Should().Be(expense.CreatedOnUtc);
            result.ModifiedOnUtc.Should().Be(expense.ModifiedOnUtc);
            result.Deleted.Should().Be(expense.Deleted);
        }
    }
}
