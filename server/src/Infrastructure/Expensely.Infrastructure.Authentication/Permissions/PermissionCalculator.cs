using System;
using System.Threading.Tasks;
using Expensely.Infrastructure.Authorization;

namespace Expensely.Infrastructure.Authentication.Permissions
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
            await _dbContext.SaveChangesAsync();

            return new[]
            {
                Permission.AccessEverything
            };
        }
    }
}
