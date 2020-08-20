using System;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Users;

namespace Expensely.Infrastructure.Authentication.Permissions
{
    /// <summary>
    /// Represents the permission calculator.
    /// </summary>
    internal sealed class PermissionCalculator
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionCalculator"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        internal PermissionCalculator(IDbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// Calculates the permissions for the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The array of permissions for the specified user identifier.</returns>
        internal static async Task<Permission[]> CalculatePermissionsForUserIdAsync(Guid userId)
        {
            await Task.Delay(1);

            return new[]
            {
                Permission.AccessEverything
            };
        }
    }
}
