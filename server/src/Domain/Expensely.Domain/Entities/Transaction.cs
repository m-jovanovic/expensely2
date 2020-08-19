using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Utility;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    /// <summary>
    /// Represents the transaction entity.
    /// </summary>
    public abstract class Transaction : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="id">The transaction identifier.</param>
        /// <param name="userId">The transaction user identifier.</param>
        /// <param name="name">The transaction name.</param>
        /// <param name="money">The transaction money instance.</param>
        /// <param name="occurredOn">The transaction occurredOn.</param>
        /// <param name="transactionType">The transaction type.</param>
        protected Transaction(Guid id, Guid userId, string name, Money money, DateTime occurredOn, TransactionType transactionType)
            : this()
        {
            Ensure.NotEmpty(id, "The identifier is required.", nameof(id));
            Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotEmpty(money, "The money is required.", nameof(money));
            Ensure.NotEmpty(occurredOn, "The occurred on date is required.", nameof(occurredOn));

            Id = id;
            UserId = userId;
            Name = name;
            Money = money;
            OccurredOn = occurredOn.Date;
            TransactionType = transactionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        protected Transaction()
        {
            Name = string.Empty;
            Money = Money.None;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets or sets the transaction name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the transaction money instance.
        /// </summary>
        public Money Money { get; protected set; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public TransactionType TransactionType { get; private set; }

        /// <summary>
        /// Gets the date the transaction occurred on.
        /// </summary>
        public DateTime OccurredOn { get; private set; }

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
