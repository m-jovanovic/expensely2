using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using Expensely.Common.Authorization;

namespace Expensely.Authentication.Permissions
{
    internal static class PermissionExtensions
    {
        internal static string PackPermissions(this IEnumerable<Permission> permissions) =>
            permissions.Aggregate(string.Empty, (aggregated, permission) => aggregated + (char)permission);

        internal static IEnumerable<Permission> UnpackPermissions(this string packedPermissions) =>
            packedPermissions.Select(permission => (Permission)permission);

        internal static Permission? ToPermission(this string permissionName) =>
            Enum.TryParse(permissionName, out Permission permission)
                ? (Permission?)permission
                : null;

        internal static Claim FindPermissionsClaim(this IEnumerable<Claim> claims) =>
            claims.SingleOrDefault(c => c.Type == PermissionConstants.PermissionsClaimType);

        internal static bool CheckIfPermissionIsAllowed(this string packedPermissions, string permissionName)
        {
            Permission[] permissions = packedPermissions.UnpackPermissions().ToArray();

            if (!Enum.TryParse(permissionName, true, out Permission permission))
            {
                throw new InvalidEnumArgumentException($"{permissionName} could not be converted to a {nameof(Permission)}.");
            }

            return permissions.CheckIfPermissionIsAllowed(permission);
        }

        internal static bool CheckIfPermissionIsAllowed(this Permission[] permissions, Permission permission) =>
            permissions.Contains(permission) || permissions.Contains(Permission.AccessEverything);
    }
}
