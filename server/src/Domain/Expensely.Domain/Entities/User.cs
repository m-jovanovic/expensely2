using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    /// <summary>
    /// Represents the user entity.
    /// </summary>
    public class User : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="firstName">The user first name.</param>
        /// <param name="lastName">The user last name.</param>
        /// <param name="email">The user email instance.</param>
        public User(Guid id, string firstName, string lastName, Email email)
            : this()
        {
            Id = id;

            // TODO: Create first name value object.
            FirstName = firstName;

            // TODO: Create last name value object.
            LastName = lastName;
            Email = email;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        ///
        /// This constructor is marked as protected so that this class can be extended
        /// in the infrastructure layer to support authentication & authorization.
        /// </remarks>
        protected User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = Email.Empty;
        }

        /// <summary>
        /// Gets the user first name.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the user last name.
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public Email Email { get; private set; }

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
