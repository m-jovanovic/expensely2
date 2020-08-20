using System;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Transactions
{
    /// <summary>
    /// Represents the income entity.
    /// </summary>
    public class Income : Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <param name="id">The income identifier.</param>
        /// <param name="userId">The income user identifier.</param>
        /// <param name="name">The income name.</param>
        /// <param name="money">The income money instance.</param>
        /// <param name="occurredOn">The income occurred on date.</param>
        public Income(Guid id, Guid userId, string name, Money money, DateTime occurredOn)
            : base(id, userId, name, money, occurredOn, TransactionType.Income)
        {
            Ensure.NotLessThanZero(money.Amount, "The amount must be greater than zero.", nameof(money.Amount));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Income()
        {
            Name = string.Empty;
            Money = Money.None;
        }
    }
}