using Expensely.Domain.Entities;

namespace Expensely.Application.Interfaces
{
    /// <summary>
    /// Represents the repository interface for managing expenses.
    /// </summary>
    public interface IExpenseRepository
    {
        /// <summary>
        /// Inserts the specified <paramref name="expense"/> instance to repository.
        /// </summary>
        /// <param name="expense">The <see cref="Expense"/> instance to be inserted to the repository.</param>
        void Insert(Expense expense);
    }
}