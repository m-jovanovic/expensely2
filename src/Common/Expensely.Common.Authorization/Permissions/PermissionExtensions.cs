using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;

namespace Expensely.Common.Authorization.Permissions
{
    public static class PermissionExtensions
    {
        public static string PackPermissions(this IEnumerable<Permission> permissions) =>
            permissions.Aggregate(string.Empty, (aggregated, permission) => aggregated + (char)permission);

        public static IEnumerable<Permission> UnpackPermissions(this string packedPermissions) =>
            packedPermissions.Select(permission => (Permission)permission);

        public static Permission? ToPermission(this string permissionName) =>
            Enum.TryParse(permissionName, out Permission permission)
                ? (Permission?)permission
                : null;

        public static Claim FindPermissionsClaim(this IEnumerable<Claim> claims) =>
            claims.SingleOrDefault(c => c.Type == PermissionConstants.PermissionsClaimType);

        public static bool CheckIfPermissionIsAllowed(this string packedPermissions, string permissionName)
        {
            Permission[] permissions = packedPermissions.UnpackPermissions().ToArray();

            if (!Enum.TryParse(permissionName, true, out Permission permission))
            {
                throw new InvalidEnumArgumentException($"{permissionName} could not be converted to a {nameof(Permission)}.");
            }

            return permissions.CheckIfPermissionIsAllowed(permission);
        }

        public static bool CheckIfPermissionIsAllowed(this Permission[] permissions, Permission permission) =>
            permissions.Contains(permission) || permissions.Contains(Permission.AccessEverything);
    }
}
