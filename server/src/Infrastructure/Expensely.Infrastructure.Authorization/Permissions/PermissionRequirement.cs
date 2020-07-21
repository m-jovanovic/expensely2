using Microsoft.AspNetCore.Authorization;

namespace Expensely.Infrastructure.Authorization.Permissions
{
    internal sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }
}
