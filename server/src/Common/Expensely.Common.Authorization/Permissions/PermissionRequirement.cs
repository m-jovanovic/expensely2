using Microsoft.AspNetCore.Authorization;

namespace Expensely.Common.Authorization.Permissions
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }
}
