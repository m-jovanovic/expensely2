using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Entities;
using Expensely.Domain.Enums;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Constants;
using Expensely.Infrastructure.Authentication.Permissions;

namespace Expensely.Infrastructure.Authentication.Providers
{
    /// <summary>
    /// Represents the claims provider.
    /// </summary>
    internal sealed class ClaimsProvider : IClaimsProvider
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsProvider"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ClaimsProvider(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Claim[]> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ExpenselyJwtClaimTypes.UserId, user.Id.ToString()),
                new Claim(ExpenselyJwtClaimTypes.Email, user.Email.Value),
                new Claim(ExpenselyJwtClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var permissionCalculator = new PermissionCalculator(_dbContext);

            Permission[] permissions = await PermissionCalculator.CalculatePermissionsForUserIdAsync(user.Id);

            IEnumerable<Claim> permissionClaims = permissions
                .Select(permission => new Claim(ExpenselyJwtClaimTypes.Permissions, permission.ToString()));

            claims.AddRange(permissionClaims);

            return claims.ToArray();
        }
    }
}