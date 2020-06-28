using Expensely.Application.Messaging;
using Expensely.Common.Primitives;

namespace Expensely.Application.Commands.Expenses.Create
{
    public sealed class CreateExpenseCommand : ICommand<Result>
    {
        public CreateExpenseCommand(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}
