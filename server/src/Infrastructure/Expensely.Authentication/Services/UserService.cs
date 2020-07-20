using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Authentication.Entities;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Authentication.Services
{
    internal class UserService : IUserService
    {
        private readonly ExpenselyAuthenticationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(ExpenselyAuthenticationDbContext dbContext, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedUser> GetByEmailAsync(string email)
            => await _dbContext.Set<AuthenticatedUser>().SingleOrDefaultAsync(x => x.Email.Value == email);

        public async Task<Result> CreateAsync(string firstName, string lastName, string email, string password)
        {
            Result<Email> emailResult = Email.Create(email);

            if (emailResult.IsFailure)
            {
                return Result.Fail(emailResult.Error);
            }

            string hashedPassword = _passwordHasher.HashPassword(password);

            // TODO: Add initial roles here.
            var user = new AuthenticatedUser(firstName, lastName, emailResult.Value(), hashedPassword);

            _dbContext.Set<AuthenticatedUser>().Add(user);

            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}