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
    /// <summary>
    /// Represents the claims provider.
    /// </summary>
    internal sealed class ClaimsProvider : IClaimsProvider
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsProvider"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ClaimsProvider(ExpenselyAuthenticationDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Claim[]> GetClaimsAsync(AuthenticatedUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ExpenselyJwtClaimTypes.UserId, user.Id.ToString()),
                new Claim(ExpenselyJwtClaimTypes.Email, user.Email.Value),
                new Claim(ExpenselyJwtClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var permissionCalculator = new PermissionCalculator(_dbContext);

            Permission[] permissions = await permissionCalculator.CalculatePermissionsForUserIdAsync(user.Id);

            IEnumerable<Claim> permissionClaims = permissions
                .Select(permission => new Claim(ExpenselyJwtClaimTypes.Permissions, permission.ToString()));

            claims.AddRange(permissionClaims);

            return claims.ToArray();
        }
    }
}