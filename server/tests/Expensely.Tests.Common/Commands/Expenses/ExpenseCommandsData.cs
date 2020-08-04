using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Tests.Common.Entities;

namespace Expensely.Tests.Common.Commands.Expenses
{
    public static class ExpenseCommandsData
    {
        public static CreateExpenseCommand ValidCreateExpenseCommand()
            => new CreateExpenseCommand(
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.Currency.Id,
                Time.Now());

        public static CreateExpenseCommand CreateExpenseCommandWithInvalidCurrencyId()
            => new CreateExpenseCommand(
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.InvalidCurrencyId,
                Time.Now());

        public static CreateExpenseCommand CreateExpenseCommandWithInvalidDate()
            => new CreateExpenseCommand(
                ExpenseData.Name,
                ExpenseData.ZeroAmount,
                ExpenseData.Currency.Id,
                default);
    }
}
