using System;
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
        /// <param name="name">The name of the expense.</param>
        /// <param name="amount">The amount of the expense.</param>
        /// <param name="currencyId">The currency identifier of the expense.</param>
        /// <param name="date">The date of the expense.</param>
        public CreateExpenseCommand(string name, decimal amount, int currencyId, DateTime date)
        {
            Name = name;
            Amount = amount;
            CurrencyId = currencyId;
            Date = date;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        public int CurrencyId { get; }

        /// <summary>
        /// Gets the date.
        /// </summary>
        public DateTime Date { get; }
    }
}
