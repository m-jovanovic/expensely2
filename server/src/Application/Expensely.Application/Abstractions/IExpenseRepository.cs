using System;
using System.Threading.Tasks;
using Expensely.Domain.Entities;

namespace Expensely.Application.Abstractions
{
    /// <summary>
    /// Represents the repository interface for managing expenses.
    /// </summary>
    public interface IExpenseRepository
    {
        /// <summary>
        /// Gets the expense with the specified identifier.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <returns>The expense if the specified identifier if it exists, otherwise null.</returns>
        Task<Expense?> GetByIdAsync(Guid id);

        /// <summary>
        /// Inserts the specified expense instance to repository.
        /// </summary>
        /// <param name="expense">The <see cref="Expense"/> instance to be inserted to the repository.</param>
        void Insert(Expense expense);

        /// <summary>
        /// Removes the specified expense instance from the repository.
        /// </summary>
        /// <param name="expense">The <see cref="Expense"/> instance to be removed from the repository.</param>
        void Remove(Expense expense);
    }
}