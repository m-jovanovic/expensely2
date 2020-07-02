using System;
using Expensely.Application.Messaging;
using Expensely.Common.Primitives;

namespace Expensely.Application.Commands.Expenses.DeleteExpense
{
    public class DeleteExpenseCommand : ICommand<Result>
    {
        public DeleteExpenseCommand(Guid expenseId)
        {
            ExpenseId = expenseId;
        }

        /// <summary>
        /// Gets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; }
    }
}
