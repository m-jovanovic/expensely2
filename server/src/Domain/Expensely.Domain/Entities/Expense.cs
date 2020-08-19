using System;
using Expensely.Domain.Utility;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    /// <summary>
    /// Represents the expense entity.
    /// </summary>
    public class Expense : Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <param name="userId">The expense user identifier.</param>
        /// <param name="name">The expense name.</param>
        /// <param name="money">The expense money instance.</param>
        /// <param name="occurredOn">The expense occurred on date.</param>
        public Expense(Guid id, Guid userId, string name, Money money, DateTime occurredOn)
            : base(id, userId, name, money, occurredOn, TransactionType.Expense)
        {
            Ensure.NotGreaterThanZero(money.Amount, "The amount can not be greater than zero.", nameof(money.Amount));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Expense()
        {
            Name = string.Empty;
            Money = Money.None;
        }
    }
}
