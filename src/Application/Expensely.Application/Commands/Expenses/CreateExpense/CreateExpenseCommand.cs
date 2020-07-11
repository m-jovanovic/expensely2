using Expensely.Application.Messaging;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Application.Commands.Expenses.CreateExpense
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
        /// <param name="currencyId">The currency identifier.</param>
        public CreateExpenseCommand(decimal amount, int currencyId)
        {
            Amount = amount;
            CurrencyId = currencyId;
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        public int CurrencyId { get; }
    }
}
