using System;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using FluentValidation.TestHelper;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Validators
{
    public class CreateExpenseCommandValidatorTests
    {
        private const string Name = "Expense";
        private const decimal Amount = 1.0m;
        private const int CurrencyId = 1;
        private static readonly DateTime Date = DateTime.Now;

        [Fact]
        public void Should_fail_if_name_is_null()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(null, Amount, CurrencyId, Date);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command).WithErrorCode(Errors.Expense.NameIsRequired);
        }

        [Fact]
        public void Should_fail_if_name_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(string.Empty, Amount, CurrencyId, Date);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command).WithErrorCode(Errors.Expense.NameIsRequired);
        }

        [Fact]
        public void Should_fail_if_currency_id_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Name, Amount, 0, Date);

            validator.ShouldHaveValidationErrorFor(x => x.CurrencyId, command).WithErrorCode(Errors.Expense.CurrencyIsRequired);
        }

        [Fact]
        public void Should_fail_if_date_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, default);

            validator.ShouldHaveValidationErrorFor(x => x.Date, command).WithErrorCode(Errors.Expense.DateIsRequired);
        }

        [Fact]
        public void Should_not_fail_if_command_is_valid()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, Date);

            TestValidationResult<CreateExpenseCommand> result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.CurrencyId);
            result.ShouldNotHaveValidationErrorFor(x => x.Date);
        }
    }
}
