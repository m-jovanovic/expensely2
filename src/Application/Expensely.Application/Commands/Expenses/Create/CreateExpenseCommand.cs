using Expensely.Application.Messaging;
using Expensely.Common.Primitives;

namespace Expensely.Application.Commands.Expenses.Create
{
    /// <summary>
    /// Represents the command for creating an expense.
    /// </summary>
    public sealed class CreateExpenseCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommand"/> class.
        /// </summary>
        /// <param name="amount">The amount of the expense.</param>
        public CreateExpenseCommand(decimal amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }
    }
}
