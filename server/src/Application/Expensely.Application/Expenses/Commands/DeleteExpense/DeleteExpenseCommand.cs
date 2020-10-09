using System;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;

namespace Expensely.Application.Expenses.Commands.DeleteExpense
{
    /// <summary>
    /// Represents the command for deleting an expense.
    /// </summary>
    public sealed class DeleteExpenseCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommand"/> class.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public DeleteExpenseCommand(Guid expenseId, Guid userId)
        {
            ExpenseId = expenseId;
            UserId = userId;
        }

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }
    }
}
