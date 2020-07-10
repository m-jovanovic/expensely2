using System;
using System.Threading.Tasks;
using Expensely.Common.Authorization;

namespace Expensely.Authentication.Permissions
{
    internal class PermissionCalculator
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        internal PermissionCalculator(ExpenselyAuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal async Task<Permission[]> CalculatePermissionsForUserIdAsync(Guid userId)
        {
            await _dbContext.Users.FindAsync(userId.ToString());

            return new[]
            {
                Permission.AccessEverything
            };
        }
    }
}
