using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Domain.Users;
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
        public async Task<bool> IsEmailUniqueAsync(string email)
            => !await _dbContext.Set<User>().AnyAsync(user => user.Email.Value == email);

        /// <inheritdoc />
        public async Task<User?> GetByEmailAsync(string email)
            => await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.Email.Value == email);

        /// <inheritdoc />
        public void Insert(User user) => _dbContext.Set<User>().Add(user);
    }
}
