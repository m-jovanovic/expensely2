using System.Threading.Tasks;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Authentication.Services
{
    internal sealed class RoleUniquenessChecker : IRoleUniquenessChecker
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        public RoleUniquenessChecker(ExpenselyAuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUniqueAsync(string roleName)
            => !await _dbContext.Set<Role>().AnyAsync(role => role.Name == roleName);
    }
}