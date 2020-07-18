using System.Threading.Tasks;

namespace Expensely.Authentication.Abstractions
{
    internal interface IUserService
    {
        // TODO: Replace object with actual user class.
        Task<object> GetByEmailAsync(string email);

        // TODO: Replace return type with something more meaningful.
        Task<string[]> CreateAsync(string registerRequestEmail, string registerRequestPassword);
    }
}
