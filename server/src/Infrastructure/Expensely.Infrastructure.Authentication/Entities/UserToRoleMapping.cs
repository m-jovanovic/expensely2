using System;

namespace Expensely.Infrastructure.Authentication.Entities
{
    /// <summary>
    /// Represents the User-to-Role mapping entity.
    /// </summary>
    internal class UserToRoleMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserToRoleMapping"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleName">The role name.</param>
        public UserToRoleMapping(Guid userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
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