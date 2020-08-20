using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using Expensely.Domain.Users;

namespace Expensely.Infrastructure.Authentication.Permissions
{
    /// <summary>
    /// Contains extension methods for working with permissions.
    /// </summary>
    internal static class PermissionExtensions
    {
        /// <summary>
        /// Unpacks the permissions string into an enumerable collection of permissions.
        /// </summary>
        /// <param name="packedPermissions">The permissions string.</param>
        /// <returns>The enumerable collection of permissions.</returns>
        internal static IEnumerable<Permission> UnpackPermissions(this string packedPermissions) =>
            packedPermissions.Split(',').Select(Enum.Parse<Permission>);

        /// <summary>
        /// Finds the permissions claim in the collection.
        /// </summary>
        /// <param name="claims">The claims collection.</param>
        /// <returns>The permissions claim if it exists, otherwise null.</returns>
        internal static Claim? FindPermissionsClaim(this IEnumerable<Claim> claims) =>
            claims.SingleOrDefault(c => c.Type == PermissionConstants.PermissionsClaimType);

        /// <summary>
        /// Checks if the permission with specified name is allowed.
        /// </summary>
        /// <param name="packedPermissions">The permissions string.</param>
        /// <param name="permissionName">The permission name.</param>
        /// <returns>True if the permission with the specified name is allowed, otherwise false.</returns>
        internal static bool CheckIfPermissionIsAllowed(this string packedPermissions, string permissionName)
        {
            Permission[] permissions = packedPermissions.UnpackPermissions().ToArray();

            if (!Enum.TryParse(permissionName, true, out Permission permission))
            {
                throw new InvalidEnumArgumentException($"{permissionName} could not be converted to a {nameof(Permission)}.");
            }

            return permissions.CheckIfPermissionIsAllowed(permission);
        }

        /// <summary>
        /// Checks if the specified permission is allowed.
        /// </summary>
        /// <param name="permissions">The permissions array.</param>
        /// <param name="permission">The permission.</param>
        /// <returns>True if the specified permission is allowed, otherwise false.</returns>
        internal static bool CheckIfPermissionIsAllowed(this Permission[] permissions, Permission permission) =>
            permissions.Contains(permission) || permissions.Contains(Permission.AccessEverything);
    }
}
