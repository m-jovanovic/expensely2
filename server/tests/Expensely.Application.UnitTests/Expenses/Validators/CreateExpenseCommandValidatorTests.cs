using System;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using Expensely.Tests.Common;
using FluentValidation.TestHelper;
using Xunit;
using static Expensely.Tests.Common.Entities.ExpenseData;

namespace Expensely.Application.UnitTests.Expenses.Validators
{
    public class CreateExpenseCommandValidatorTests
    {
        private static readonly DateTime Date = Time.Now();

        [Fact]
        public void Should_fail_if_currency_id_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Name, ZeroAmount, InvalidCurrencyId, Date);

            validator.ShouldHaveValidationErrorFor(x => x.CurrencyId, command).WithErrorCode(Errors.Expense.CurrencyIsRequired);
        }

        [Fact]
        public void Should_fail_if_date_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Name, ZeroAmount, Currency.Id, default);

            validator.ShouldHaveValidationErrorFor(x => x.Date, command).WithErrorCode(Errors.Expense.DateIsRequired);
        }

        [Fact]
        public void Should_not_fail_if_command_is_valid()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Name, ZeroAmount, Currency.Id, Date);

            TestValidationResult<CreateExpenseCommand> result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.CurrencyId);
            result.ShouldNotHaveValidationErrorFor(x => x.Date);
        }
    }
}
