using System.Threading.Tasks;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Application.Abstractions
{
    public interface IAuthenticationService
    {
        Task<Result<string>> RegisterAsync(string firstName, string lastName, Email email, Password password);

        Task<Result<string>> LoginAsync(string email, string password);
    }
}
