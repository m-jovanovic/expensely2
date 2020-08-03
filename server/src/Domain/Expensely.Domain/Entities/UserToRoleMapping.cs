using System;

namespace Expensely.Domain.Entities
{
    /// <summary>
    /// Represents the User-to-Role mapping entity.
    /// </summary>
    public sealed class UserToRoleMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserToRoleMapping"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleName">The role name.</param>
        public UserToRoleMapping(Guid userId, string roleName)
            : this()
        {
            UserId = userId;
            RoleName = roleName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserToRoleMapping"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private UserToRoleMapping()
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