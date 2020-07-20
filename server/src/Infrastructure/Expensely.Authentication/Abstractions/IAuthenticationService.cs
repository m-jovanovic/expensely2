using System.Threading.Tasks;
using Expensely.Common.Contracts.Authentication;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Authentication.Abstractions
{
    public interface IAuthenticationService
    {
        Task<Result> RegisterAsync(RegisterRequest registerRequest);

        Task<Result<string>> LoginAsync(LoginRequest loginRequest);
    }
}
