using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Tests.Common.Entities;

namespace Expensely.Tests.Common.Commands.Expenses
{
    public static class CreateExpenseCommandData
    {
        public static CreateExpenseCommand CreateValidCommand()
            => new CreateExpenseCommand(
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.Currency.Id,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidCurrencyId()
            => new CreateExpenseCommand(
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.InvalidCurrencyId,
                Time.Now());

        public static CreateExpenseCommand CreateCommandWithInvalidDate()
            => new CreateExpenseCommand(
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.Currency.Id,
                default);
    }
}
