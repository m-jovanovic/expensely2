using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Constants;
using Expensely.Infrastructure.Authentication.Entities;
using Expensely.Infrastructure.Authentication.Permissions;
using Expensely.Infrastructure.Authorization;

namespace Expensely.Infrastructure.Authentication.Services
{
    internal sealed class ClaimsProvider : IClaimsProvider
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        public ClaimsProvider(ExpenselyAuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Claim[]> GetClaimsAsync(AuthenticatedUser authenticatedUser)
        {
            var claims = new List<Claim>()
            {
                new Claim(ExpenselyJwtClaimTypes.UserId, authenticatedUser.Id.ToString()),
                new Claim(ExpenselyJwtClaimTypes.Email, authenticatedUser.Email.Value),
                new Claim(ExpenselyJwtClaimTypes.Name, $"{authenticatedUser.FirstName} {authenticatedUser.LastName}")
            };

            var permissionCalculator = new PermissionCalculator(_dbContext);

            Permission[] permissions = await permissionCalculator.CalculatePermissionsForUserIdAsync(authenticatedUser.Id);

            IEnumerable<Claim> permissionClaims = permissions
                .Select(permission => new Claim(ExpenselyJwtClaimTypes.Permissions, permission.ToString()));

            claims.AddRange(permissionClaims);

            return claims.ToArray();
        }
    }
}