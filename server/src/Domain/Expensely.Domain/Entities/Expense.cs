using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Utility;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    /// <summary>
    /// Represents the expense entity.
    /// </summary>
    public class Expense : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <param name="userId">The expense user identifier.</param>
        /// <param name="name">The expense name.</param>
        /// <param name="money">The expense money instance.</param>
        /// <param name="date">The expense date.</param>
        public Expense(Guid id, Guid userId, string name, Money money, DateTime date)
            : this()
        {
            Ensure.NotEmpty(id, "The identifier is required.", nameof(id));
            Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
            Ensure.NotEmpty(money, "The money is required.", nameof(money));
            Ensure.NotEmpty(date, "The date is required.", nameof(date));

            Id = id;
            UserId = userId;
            Name = name;
            Money = money;
            Date = date.Date;
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

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the expense name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the expense money instance.
        /// </summary>
        public Money Money { get; private set; }

        /// <summary>
        /// Gets the expense date.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? DeletedOnUtc { get; }

        /// <inheritdoc />
        public bool Deleted { get; }
    }
}
