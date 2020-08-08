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
            var query = new GetExpenseByIdQuery(Guid.NewGuid(), UserId);

            ExpenseResponse? result = await SendAsync(query);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_null_response_given_existing_expense_id_with_invalid_user_id()
        {
            var expense = ExpenseData.CreateExpense();
            await AddAsync(expense);
            var query = new GetExpenseByIdQuery(expense.Id, UserId);

            ExpenseResponse? result = await SendAsync(query);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_null_response_given_existing_expense_id_with_valid_user_id()
        {
            var expense = ExpenseData.CreateExpense(UserId);
            await AddAsync(expense);
            var query = new GetExpenseByIdQuery(expense.Id, UserId);

            ExpenseResponse? result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Id.Should().Be(expense.Id);
            result.Name.Should().Be(expense.Name);
            result.Amount.Should().Be(expense.Money.Amount);
            result.CurrencyId.Should().Be(expense.Money.Currency.Id);
            result.CurrencyCode.Should().Be(expense.Money.Currency.Code);
            result.Date.Should().Be(expense.Date);
            result.CreatedOnUtc.Should().Be(expense.CreatedOnUtc);
        }
    }
}
