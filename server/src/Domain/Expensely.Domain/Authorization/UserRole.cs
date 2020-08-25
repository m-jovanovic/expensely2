using System;

namespace Expensely.Domain.Authorization
{
    /// <summary>
    /// Represents the User-to-Role mapping entity.
    /// </summary>
    public sealed class UserRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleName">The role name.</param>
        public UserRole(Guid userId, string roleName)
            : this()
        {
            UserId = userId;
            RoleName = roleName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private UserRole()
        {
            RoleName = string.Empty;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the role name.
        /// </summary>
        public string RoleName { get; }
    }
}