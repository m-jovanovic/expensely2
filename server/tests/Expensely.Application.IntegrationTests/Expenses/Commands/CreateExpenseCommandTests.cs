using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Exceptions;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;
using static Expensely.Tests.Common.Commands.Expenses.CreateExpenseCommandData;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class CreateExpenseCommandTests
    {
        [Fact]
        public void Handle_should_throw_validation_exception_if_user_id_is_empty()
        {
            var command = CreateCommandWithInvalidUserId();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Expense.UserIdIsRequired);
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_currency_id_is_empty()
        {
            var command = CreateCommandWithInvalidCurrencyId();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Expense.CurrencyIsRequired);
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_date_is_empty()
        {
            var command = CreateCommandWithInvalidDate();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Expense.OccurredOnIsRequired);
        }

        [Fact]
        public async Task Should_create_an_expense_given_valid_command()
        {
            var command = CreateValidCommand();

            Result<EntityCreatedResponse> result = await SendAsync(command);

            result.IsSuccess.Should().BeTrue();
            EntityCreatedResponse entityCreatedResponse = result.Value();
            entityCreatedResponse.Id.Should().NotBeEmpty();
            Expense? expense = await FindAsync<Expense>(result.Value().Id);
            expense.Should().NotBeNull();
        }
    }
}
