﻿using System;
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
        public void Should_fail_if_user_id_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.Empty, Name, MinusOneAmount, Currency.Code, Date);

            validator.ShouldHaveValidationErrorFor(x => x.UserId, command).WithErrorCode(Errors.Expense.UserIdIsRequired);
        }

        [Fact]
        public void Should_fail_if_currency_id_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.NewGuid(), Name, MinusOneAmount, InvalidCurrencyCode, Date);

            validator.ShouldHaveValidationErrorFor(x => x.CurrencyCode, command).WithErrorCode(Errors.Expense.CurrencyIsRequired);
        }

        [Fact]
        public void Should_fail_if_date_is_empty()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.NewGuid(), Name, MinusOneAmount, Currency.Code, default);

            validator.ShouldHaveValidationErrorFor(x => x.OccurredOn, command).WithErrorCode(Errors.Expense.OccurredOnIsRequired);
        }

        [Fact]
        public void Should_not_fail_if_command_is_valid()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand(Guid.NewGuid(), Name, MinusOneAmount, Currency.Code, Date);

            TestValidationResult<CreateExpenseCommand> result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldNotHaveValidationErrorFor(x => x.CurrencyCode);
            result.ShouldNotHaveValidationErrorFor(x => x.OccurredOn);
        }
    }
}
