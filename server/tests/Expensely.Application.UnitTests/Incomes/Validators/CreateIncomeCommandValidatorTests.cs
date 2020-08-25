using System;
using Expensely.Application.Incomes.Commands.CreateIncome;
using Expensely.Domain;
using Expensely.Tests.Common;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;
using static Expensely.Tests.Common.Entities.TransactionData;

namespace Expensely.Application.UnitTests.Incomes.Validators
{
    public class CreateIncomeCommandValidatorTests
    {
        private static readonly DateTime Date = Time.Now();

        [Fact]
        public void Should_fail_if_user_id_is_empty()
        {
            var validator = new CreateIncomeCommandValidator();
            var command = new CreateIncomeCommand(Guid.Empty, Name, MinusOneAmount, Currency.Value, Date);

            validator.ShouldHaveValidationErrorFor(x => x.UserId, command).WithErrorCode(Errors.Income.UserIdIsRequired);
        }

        [Fact]
        public void Should_fail_if_currency_id_is_empty()
        {
            var validator = new CreateIncomeCommandValidator();
            var command = new CreateIncomeCommand(Guid.NewGuid(), Name, MinusOneAmount, InvalidCurrencyId, Date);

            validator.ShouldHaveValidationErrorFor(x => x.CurrencyId, command).WithErrorCode(Errors.Income.CurrencyIdIsRequired);
        }

        [Fact]
        public void Should_fail_if_date_is_empty()
        {
            var validator = new CreateIncomeCommandValidator();
            var command = new CreateIncomeCommand(Guid.NewGuid(), Name, MinusOneAmount, Currency.Value, default);

            validator.ShouldHaveValidationErrorFor(x => x.OccurredOn, command).WithErrorCode(Errors.Income.OccurredOnIsRequired);
        }

        [Fact]
        public void Should_not_fail_if_command_is_valid()
        {
            var validator = new CreateIncomeCommandValidator();
            var command = new CreateIncomeCommand(Guid.NewGuid(), Name, MinusOneAmount, Currency.Value, Date);

            TestValidationResult<CreateIncomeCommand> result = validator.TestValidate(command);
            
            result.IsValid.Should().BeTrue();
        }
    }
}
