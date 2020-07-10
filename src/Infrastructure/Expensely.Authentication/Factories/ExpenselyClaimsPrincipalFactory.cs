using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Authentication.Permissions;
using Expensely.Common.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Expensely.Authentication.Factories
{
    internal class ExpenselyClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser>
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        public ExpenselyClaimsPrincipalFactory(
            UserManager<IdentityUser> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            ExpenselyAuthenticationDbContext dbContext)
            : base(userManager, optionsAccessor)
        {
            _dbContext = dbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
        {
            ClaimsIdentity claimsIdentity = await base.GenerateClaimsAsync(user);

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            var permissionsCalculator = new PermissionCalculator(_dbContext);

            Permission[] permissions = await permissionsCalculator.CalculatePermissionsForUserIdAsync(Guid.Parse(user.Id));

            claimsIdentity.AddClaim(new Claim(PermissionConstants.PermissionsClaimType, permissions.PackPermissions()));

            return claimsIdentity;
        }
    }
}
