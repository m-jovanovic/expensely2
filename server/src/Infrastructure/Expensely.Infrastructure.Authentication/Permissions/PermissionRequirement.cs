using Microsoft.AspNetCore.Authorization;

namespace Expensely.Infrastructure.Authentication.Permissions
{
    /// <summary>
    /// Represents the permission requirement.
    /// </summary>
    internal sealed class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
        /// </summary>
        /// <param name="permissionName">The permission name.</param>
        internal PermissionRequirement(string permissionName) => PermissionName = permissionName;

        /// <summary>
        /// Gets the permission name.
        /// </summary>
        internal string PermissionName { get; }
    }
}
