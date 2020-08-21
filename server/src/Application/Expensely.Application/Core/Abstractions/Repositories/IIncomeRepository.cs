using Expensely.Domain.Transactions;

namespace Expensely.Application.Core.Abstractions.Repositories
{
    /// <summary>
    /// Represents the income repository interface.
    /// </summary>
    public interface IIncomeRepository
    {
        /// <summary>
        /// Inserts the specified income instance to the repository.
        /// </summary>
        /// <param name="income">The income instance to be inserted to the repository.</param>
        void Insert(Income income);
    }
}