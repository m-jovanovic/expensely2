using System;
using System.Threading.Tasks;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Authentication.Services
{
    /// <summary>
    /// Represents the user service.
    /// </summary>
    internal sealed class UserService : IUserService
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        public UserService(ExpenselyAuthenticationDbContext dbContext, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        /// <inheritdoc />
        public async Task<AuthenticatedUser?> GetByEmailAsync(string email)
            => await _dbContext.Set<AuthenticatedUser>().FirstOrDefaultAsync(user => user.Email.Value == email);

        /// <inheritdoc />
        public async Task<Result<AuthenticatedUser>> CreateAsync(
            string firstName, string lastName, Email email, Password password)
        {
            string hashedPassword = _passwordHasher.HashPassword(password);

            // TODO: Add initial roles here.
            var user = new AuthenticatedUser(Guid.NewGuid(), firstName, lastName, email, hashedPassword);

            _dbContext.Set<AuthenticatedUser>().Add(user);

            await _dbContext.SaveChangesAsync();

            return Result.Ok(user);
        }
    }
}