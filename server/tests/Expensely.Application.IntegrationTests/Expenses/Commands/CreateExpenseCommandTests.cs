using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Exceptions;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class CreateExpenseCommandTests
    {
        private const string Name = "Expense";
        private const decimal Amount = 1.0m;
        private const int CurrencyId = 1;
        private static readonly DateTime Date = DateTime.Now;

        [Fact]
        public async Task Should_create_an_expense_given_valid_command()
        {
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, Date);

            Result<EntityCreatedResponse> result = await SendAsync(command);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Invoking(r => r.Value()).Should().NotThrow();
            EntityCreatedResponse entityCreatedResponse = result.Value();
            entityCreatedResponse.Id.Should().NotBeEmpty();
            Expense? expense = await FindAsync<Expense>(result.Value().Id);
            expense.Should().NotBeNull();
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_name_is_empty()
        {
            var command = new CreateExpenseCommand(string.Empty, Amount, CurrencyId, Date);
            
            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Expense.NameIsRequired);
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_currency_id_is_empty()
        {
            var command = new CreateExpenseCommand(Name, Amount, default, Date);

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Expense.CurrencyIsRequired);
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_date_is_empty()
        {
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, default);

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Expense.DateIsRequired);
        }
    }
}
