using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Infrastructure.Authentication.Permissions
{
    /// <summary>
    /// Represents the permission authorization handler.
    /// </summary>
    internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            Claim? permissionsClaim = context.User?.Claims.FindPermissionsClaim();

            if (permissionsClaim is null)
            {
                return Task.CompletedTask;
            }

            if (permissionsClaim.Value.CheckIfPermissionIsAllowed(requirement.PermissionName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
