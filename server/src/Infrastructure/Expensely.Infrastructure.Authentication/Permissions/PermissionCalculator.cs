using System;
using System.Threading.Tasks;
using Expensely.Infrastructure.Authorization;

namespace Expensely.Infrastructure.Authentication.Permissions
{
    /// <summary>
    /// Represents the permission calculator.
    /// </summary>
    internal sealed class PermissionCalculator
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionCalculator"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        internal PermissionCalculator(ExpenselyAuthenticationDbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// Calculates the permissions for the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The array of permissions for the specified user identifier.</returns>
        internal async Task<Permission[]> CalculatePermissionsForUserIdAsync(Guid userId)
        {
            await _dbContext.SaveChangesAsync();

            return new[]
            {
                Permission.AccessEverything
            };
        }
    }
}
