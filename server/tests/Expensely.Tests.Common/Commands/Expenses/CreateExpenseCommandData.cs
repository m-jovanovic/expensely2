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
                TransactionData.Name,
                TransactionData.MinusOneAmount,
                TransactionData.Currency.Value,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidUserId()
            => new CreateExpenseCommand(
                Guid.Empty,
                TransactionData.Name,
                TransactionData.MinusOneAmount,
                TransactionData.InvalidCurrencyId,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidCurrencyId()
            => new CreateExpenseCommand(
                Guid.NewGuid(),
                TransactionData.Name,
                TransactionData.MinusOneAmount,
                TransactionData.InvalidCurrencyId,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidDate()
            => new CreateExpenseCommand(
                Guid.NewGuid(),
                TransactionData.Name,
                TransactionData.MinusOneAmount,
                TransactionData.Currency.Value,
                default);
    }
}
