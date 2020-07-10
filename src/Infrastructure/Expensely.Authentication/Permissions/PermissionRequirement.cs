using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authentication.Permissions
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        internal PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        internal string PermissionName { get; }
    }
}
