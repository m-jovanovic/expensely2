using System;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Transactions;
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
            var command = new DeleteExpenseCommand(Guid.NewGuid(), Guid.NewGuid());

            Result result = await SendAsync(command);

            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Should_return_failure_result_given_expense_with_invalid_user_id()
        {
            Expense expense = TransactionData.CreateExpense();
            await AddAsync(expense);
            var command = new DeleteExpenseCommand(expense.Id, Guid.NewGuid());

            Result result = await SendAsync(command);

            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Should_delete_an_expense_given_existing_expense_id_with_valid_user_id()
        {
            Expense expense = TransactionData.CreateExpense(UserId);
            await AddAsync(expense);
            var command = new DeleteExpenseCommand(expense.Id, expense.UserId);

            Result result = await SendAsync(command);
            
            result.IsSuccess.Should().BeTrue();
            Expense? foundExpense = await FindAsync<Expense>(expense.Id);
            foundExpense.Should().BeNull();
        }
    }
}
