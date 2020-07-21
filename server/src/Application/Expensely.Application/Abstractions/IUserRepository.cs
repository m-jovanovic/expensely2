using System.Threading.Tasks;

namespace Expensely.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueAsync(string email);
    }
}
