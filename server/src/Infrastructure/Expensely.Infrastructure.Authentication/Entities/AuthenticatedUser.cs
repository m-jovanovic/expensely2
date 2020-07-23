using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

namespace Expensely.Infrastructure.Authentication.Entities
{
    /// <summary>
    /// Represents the authenticated user entity.
    /// </summary>
    internal sealed class AuthenticatedUser : User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedUser"/> class.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="firstName">The user first name.</param>
        /// <param name="lastName">The user last name.</param>
        /// <param name="email">The user email.</param>
        /// <param name="passwordHash">The user password hash.</param>
        public AuthenticatedUser(Guid id, string firstName, string lastName, Email email, string passwordHash)
            : base(id, firstName, lastName, email)
        {
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedUser"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private AuthenticatedUser()
        {
            PasswordHash = string.Empty;
        }

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        public string PasswordHash { get; }
    }
}
