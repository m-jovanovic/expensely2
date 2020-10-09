using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Core.Exceptions;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Transactions;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;
using static Expensely.Tests.Common.Commands.Expenses.CreateIncomeCommandData;

namespace Expensely.Application.IntegrationTests.Incomes.Commands
{
    public class CreateIncomeCommandTests
    {
        [Fact]
        public void Handle_should_throw_validation_exception_if_user_id_is_empty()
        {
            var command = CreateCommandWithInvalidUserId();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Income.UserIdIsRequired);
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_currency_id_is_empty()
        {
            var command = CreateCommandWithInvalidCurrencyId();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Income.CurrencyIdIsRequired);
        }

        [Fact]
        public void Handle_should_throw_validation_exception_if_date_is_empty()
        {
            var command = CreateCommandWithInvalidDate();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Income.OccurredOnIsRequired);
        }

        [Fact]
        public async Task Should_create_an_income_given_valid_command()
        {
            var command = CreateValidCommand();

            Result<EntityCreatedResponse> result = await SendAsync(command);

            result.IsSuccess.Should().BeTrue();
            EntityCreatedResponse entityCreatedResponse = result.Value();
            entityCreatedResponse.Id.Should().NotBeEmpty();
            Income? income = await FindAsync<Income>(result.Value().Id);
            income.Should().NotBeNull();
        }
    }
}
