using System.Threading.Tasks;

namespace Expensely.Presentation.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsAuthenticatedAsync();

        Task SignInAsync(string email, string password);

        Task SignOutAsync();
    }
}
