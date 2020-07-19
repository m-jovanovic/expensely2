using System.Threading.Tasks;
using Expensely.Authentication.Entities;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Authentication.Abstractions
{
    internal interface IUserService
    {
        Task<AuthenticatedUser> GetByEmailAsync(string email);

        Task<Result> CreateAsync(string firstName, string lastName, string email, string password);
    }
}
