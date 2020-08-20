using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Utility;
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
        /// <param name="passwordHash">The user password hash.</param>
        public User(Guid id, FirstName firstName, LastName lastName, Email email, string passwordHash)
            : this()
        {
            Ensure.NotEmpty(id, "The identifier is required.", nameof(id));
            Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
            Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
            Ensure.NotEmpty(email, "The email is required.", nameof(email));
            Ensure.NotEmpty(passwordHash, "The password hash is required", nameof(passwordHash));

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
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
            FirstName = FirstName.Empty;
            LastName = LastName.Empty;
            Email = Email.Empty;
            PasswordHash = string.Empty;
        }

        /// <summary>
        /// Gets the user first name.
        /// </summary>
        public FirstName FirstName { get; private set; }

        /// <summary>
        /// Gets the user last name.
        /// </summary>
        public LastName LastName { get; private set; }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public Email Email { get; private set; }

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        public string PasswordHash { get; }

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
