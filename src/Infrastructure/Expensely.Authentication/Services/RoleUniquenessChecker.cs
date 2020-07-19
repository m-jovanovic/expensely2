using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Authentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Authentication.Services
{
    internal class RoleUniquenessChecker : IRoleUniquenessChecker
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        public RoleUniquenessChecker(ExpenselyAuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUniqueAsync(string roleName) => await _dbContext.Set<Role>().AnyAsync(x => x.Name == roleName);
    }
}