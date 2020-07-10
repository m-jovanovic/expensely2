using System.Threading.Tasks;
using Expensely.Common.Contracts.Authentication;

namespace Expensely.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);

        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    }
}
