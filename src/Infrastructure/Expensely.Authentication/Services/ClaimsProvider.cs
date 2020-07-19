using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Authentication.Constants;
using Expensely.Authentication.Entities;
using Expensely.Authentication.Permissions;
using Expensely.Common.Authorization;
using Expensely.Common.Authorization.Permissions;

namespace Expensely.Authentication.Services
{
    internal class ClaimsProvider : IClaimsProvider
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        public ClaimsProvider(ExpenselyAuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Claim[]> GetClaimsAsync(AuthenticatedUser authenticatedUser)
        {
            var permissionCalculator = new PermissionCalculator(_dbContext);

            Permission[] permissions = await permissionCalculator.CalculatePermissionsForUserIdAsync(authenticatedUser.Id);

            Claim[] claims =
            {
                new Claim(ExpenselyJwtClaimTypes.UserId, authenticatedUser.Id.ToString()),
                new Claim(ExpenselyJwtClaimTypes.Email, authenticatedUser.Email.Value),
                new Claim(ExpenselyJwtClaimTypes.Permissions, permissions.PackPermissions())
            };

            return claims;
        }
    }
}