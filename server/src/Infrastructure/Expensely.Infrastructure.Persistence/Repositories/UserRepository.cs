using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IDbContext _dbContext;

        public UserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<bool> IsUniqueAsync(string email)
            => !await _dbContext.Set<User>().AnyAsync(user => user.Email.Value == email);
    }
}
