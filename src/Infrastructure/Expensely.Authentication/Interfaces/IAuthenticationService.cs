using System.Threading.Tasks;
using Expensely.Contracts.Authentication;

namespace Expensely.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegisterUserResponse> RegisterUserAsync(string email, string password);
    }
}
