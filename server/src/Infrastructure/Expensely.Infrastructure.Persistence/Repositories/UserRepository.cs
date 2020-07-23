using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Represents the user repository.
    /// </summary>
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<bool> IsUniqueAsync(string email)
            => !await _dbContext.Set<User>().AnyAsync(user => user.Email.Value == email);
    }
}
