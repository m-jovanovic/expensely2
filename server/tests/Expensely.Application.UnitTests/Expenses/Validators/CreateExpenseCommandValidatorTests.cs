using System;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using Expensely.Tests.Common;
using FluentValidation.TestHelper;
using Xunit;
using static Expensely.Tests.Common.Entities.TransactionData;

namespace Expensely.Application.UnitTests.Expenses.Validators
{
    public class CreateExpenseCommandValidatorTests
    {
        private static readonly DateTime Date = Time.Now();

        [Fact]
        public void Should_fail_if_user_id_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.Empty, Name, MinusOneAmount, Currency.Value, Date);

            validator.ShouldHaveValidationErrorFor(x => x.UserId, command).WithErrorCode(Errors.Expense.UserIdIsRequired);
        }

        [Fact]
        public void Should_fail_if_currency_id_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.NewGuid(), Name, MinusOneAmount, InvalidCurrencyId, Date);

            validator.ShouldHaveValidationErrorFor(x => x.CurrencyId, command).WithErrorCode(Errors.Expense.CurrencyIdIsRequired);
        }

        [Fact]
        public void Should_fail_if_date_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.NewGuid(), Name, MinusOneAmount, Currency.Value, default);

            validator.ShouldHaveValidationErrorFor(x => x.OccurredOn, command).WithErrorCode(Errors.Expense.OccurredOnIsRequired);
        }

        [Fact]
        public void Should_not_fail_if_command_is_valid()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.NewGuid(), Name, MinusOneAmount, Currency.Value, Date);

            TestValidationResult<CreateExpenseCommand> result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldNotHaveValidationErrorFor(x => x.CurrencyId);
            result.ShouldNotHaveValidationErrorFor(x => x.OccurredOn);
        }
    }
}
