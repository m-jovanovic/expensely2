using System;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class DeleteExpenseCommandTests
    {
        [Fact]
        public async Task Should_return_failure_result_given_non_existing_expense_id()
        {
            var command = new DeleteExpenseCommand(Guid.NewGuid());

            Result result = await SendAsync(command);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Should_delete_an_expense_given_existing_expense_id()
        {
            Expense expense = ExpenseData.Expense;
            await AddAsync(expense);
            var command = new DeleteExpenseCommand(expense.Id);

            Result result = await SendAsync(command);
            
            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            Expense? foundExpense = await FindAsync<Expense>(expense.Id);
            foundExpense.Should().BeNull();
        }
    }
}
