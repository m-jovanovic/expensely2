using System;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Tests.Common.Entities;

namespace Expensely.Tests.Common.Commands.Expenses
{
    public static class CreateExpenseCommandData
    {
        public static CreateExpenseCommand CreateValidCommand(Guid? userId = null)
            => new CreateExpenseCommand(
                userId ?? Guid.NewGuid(),
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.Currency.Id,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidUserId()
            => new CreateExpenseCommand(
                Guid.Empty,
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.InvalidCurrencyId,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidCurrencyId()
            => new CreateExpenseCommand(
                Guid.NewGuid(),
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.InvalidCurrencyId,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidDate()
            => new CreateExpenseCommand(
                Guid.NewGuid(),
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.Currency.Id,
                default);
    }
}
