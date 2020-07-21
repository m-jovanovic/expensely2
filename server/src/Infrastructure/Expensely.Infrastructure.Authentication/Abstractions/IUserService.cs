using System.Threading.Tasks;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Authentication.Entities;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    internal interface IUserService
    {
        Task<AuthenticatedUser?> GetByEmailAsync(string email);

        Task<Result<AuthenticatedUser>> CreateAsync(string firstName, string lastName, Email email, string password);
    }
}
