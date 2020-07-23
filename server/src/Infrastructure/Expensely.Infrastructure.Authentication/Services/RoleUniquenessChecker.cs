using System.Threading.Tasks;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Authentication.Services
{
    /// <summary>
    /// Represents the role uniqueness checker.
    /// </summary>
    internal sealed class RoleUniquenessChecker : IRoleUniquenessChecker
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUniquenessChecker"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RoleUniquenessChecker(ExpenselyAuthenticationDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<bool> IsUniqueAsync(string roleName)
            => !await _dbContext.Set<Role>().AnyAsync(role => role.Name == roleName);
    }
}