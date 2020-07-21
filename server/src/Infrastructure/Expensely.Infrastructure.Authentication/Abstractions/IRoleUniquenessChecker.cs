using System.Threading.Tasks;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    internal interface IRoleUniquenessChecker
    {
        Task<bool> IsUniqueAsync(string roleName);
    }
}