using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain.Core;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests
    {
        [Fact]
        public async Task Should_return_failure_result_given_non_existing_expense_id()
        {
            var query = new GetExpenseByIdQuery(Guid.NewGuid(), UserId);

            Result<ExpenseResponse> result = await SendAsync(query);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_failure_result_given_existing_expense_id_with_invalid_user_id()
        {
            var expense = TransactionData.CreateExpense();
            await AddAsync(expense);
            var query = new GetExpenseByIdQuery(expense.Id, UserId);

            Result<ExpenseResponse> result = await SendAsync(query);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_success_result_given_existing_expense_id_with_valid_user_id()
        {
            var expense = TransactionData.CreateExpense(UserId);
            await AddAsync(expense);
            var query = new GetExpenseByIdQuery(expense.Id, UserId);

            Result<ExpenseResponse> result = await SendAsync(query);

            var response = result.Value();
            response.Id.Should().Be(expense.Id);
            response.Name.Should().Be(expense.Name);
            response.Amount.Should().Be(expense.Money.Amount);
            response.CurrencyCode.Should().Be(expense.Money.Currency.Code);
            response.OccurredOn.Should().Be(expense.OccurredOn);
            response.CreatedOnUtc.Should().Be(expense.CreatedOnUtc);
        }
    }
}
