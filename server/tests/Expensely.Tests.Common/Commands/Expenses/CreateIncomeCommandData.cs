using System;
using Expensely.Application.Incomes.Commands.CreateIncome;
using Expensely.Tests.Common.Entities;

namespace Expensely.Tests.Common.Commands.Expenses
{
    public static class CreateIncomeCommandData
    {
        public static CreateIncomeCommand CreateValidCommand(Guid? userId = null)
            => new CreateIncomeCommand(
                userId ?? Guid.NewGuid(),
                TransactionData.Name,
                TransactionData.PlusOneAmount,
                TransactionData.Currency.Value,
                Time.Now());

        public static CreateIncomeCommand CreateCommandWithInvalidUserId()
            => new CreateIncomeCommand(
                Guid.Empty,
                TransactionData.Name,
                TransactionData.PlusOneAmount,
                TransactionData.InvalidCurrencyId,
                Time.Now());

        public static CreateIncomeCommand CreateCommandWithInvalidCurrencyId()
            => new CreateIncomeCommand(
                Guid.NewGuid(),
                TransactionData.Name,
                TransactionData.PlusOneAmount,
                TransactionData.InvalidCurrencyId,
                Time.Now());

        public static CreateIncomeCommand CreateCommandWithInvalidDate()
            => new CreateIncomeCommand(
                Guid.NewGuid(),
                TransactionData.Name,
                TransactionData.PlusOneAmount,
                TransactionData.Currency.Value,
                default);
    }
}