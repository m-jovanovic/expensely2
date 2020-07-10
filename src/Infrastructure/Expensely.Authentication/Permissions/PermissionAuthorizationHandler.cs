using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authentication.Permissions
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
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
